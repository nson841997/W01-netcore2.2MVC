﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;
using ShoeShop.Security;

namespace ShoeShop.Controllers
{
    [Route("customer")]
    public class CustomerController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private SecurityManager securityManager = new SecurityManager();
        public CustomerController(DatabaseContext _db)
        {
            db = _db;
        }
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            var account = new Account();
            return View("Register", account);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Account account)
        {
            var exists = db.Accounts.Count(a => a.Username.Equals(account.Username)) > 0;
            if (!exists)
            {
                account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                account.Status = true;
                db.Accounts.Add(account);
                db.SaveChanges();

                //Add role customer to new account
                var roleAccount = new RoleAccount()
                {
                    RoleId = 2,
                    AccountId = account.Id,
                    Status = true
                };
                db.RoleAccounts.Add(roleAccount);
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
            } else
            {
                ViewBag.error = "Username exists";
                return View("Register", account);
            }
            
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {

            var account = processLogin(username, password);
            if (account != null)
            {
                securityManager.SignIn(this.HttpContext, account, "User_Schema");
                return RedirectToAction("dashboard", "customer");
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Login");
            }
        }

        private Account processLogin(string username, string password)
        {
            var account = db.Accounts.SingleOrDefault(a => a.Username.Equals(username) && a.Status == true);
            
            if (account != null)
            {
                var role = account.RoleAccount.FirstOrDefault();
                if (role.RoleId == 2 && role.Status == true && BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    return account;
                }
            }
            return null;
        }
        [Route("signout")]
        public IActionResult SignOut()
        {
            securityManager.SignOut(this.HttpContext,  "User_Schema");
            return RedirectToAction("login", "customer");
        }

        [Authorize(Roles = "Customer", AuthenticationSchemes = "User_Schema")]
        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var customer = db.Accounts.SingleOrDefault(a => a.Username.Equals(user.Value));
            return View("Profile", customer);
        }

        [Authorize(Roles = "Customer", AuthenticationSchemes = "User_Schema")]
        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(Account account)
        {
            var currentCustomer = db.Accounts.Find(account.Id);
            if (!string.IsNullOrEmpty(account.Password))
            {
                currentCustomer.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            }
            currentCustomer.Fullname = account.Fullname;
            currentCustomer.Email = account.Email;
            currentCustomer.Address = account.Address;
            currentCustomer.Phone = account.Phone;
            db.SaveChanges();
            return View("Profile", currentCustomer);
        }
        [Authorize(Roles = "Customer", AuthenticationSchemes = "User_Schema")]
        [HttpGet]
        [Route("DashBoard")]
        public IActionResult DashBoard()
        {
            
            return View("DashBoard");
        }

        [HttpGet]
        [Route("acessdenied")]
        public IActionResult AcessDenied()
        {

            return View("AcessDenied");
        }

        [HttpGet]
        [Route("history")]
        public IActionResult History()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var customer = db.Accounts.SingleOrDefault(a => a.Username.Equals(user.Value));
            ViewBag.invoices = customer.Invoice.OrderByDescending(i => i.Id).ToList();
            return View("History");
        }
        [Authorize(Roles = "Customer", AuthenticationSchemes = "User_Schema")]
        [HttpGet]
        [Route("details/{id}")]
        public IActionResult Details(int id)
        {
            var a = ViewBag.invoicesDetail = db.InvoiceDetails.Where(i => i.InvoiceId == id).ToList();

            return View("Details");
        }
    }
}