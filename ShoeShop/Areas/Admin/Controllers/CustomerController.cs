﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/customer")]
    public class CustomerController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public CustomerController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.Customers = db.Accounts.Where(a => a.RoleAccount.FirstOrDefault().RoleId == 2).ToList();
            return View();
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var customer = db.Accounts.Find(id);
            return View("Edit", customer);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, Account account)
        {
            var customer = db.Accounts.Find(id);
            if (!string.IsNullOrEmpty(account.Password))
            {
                customer.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            }
            customer.Fullname = account.Fullname;
            customer.Email = account.Email;
            customer.Status = account.Status;
            db.SaveChanges();
            return RedirectToAction("Index", "Customer", new { area = "admin"});
        }
    }
}