using System;
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
    [Route("admin/invoice")]
    public class InvoiceController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public InvoiceController(DatabaseContext _db) 
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.invoices = db.Invoices.OrderByDescending(i => i.Id).ToList();
            return View();
        }

        [HttpGet]
        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            ViewBag.invoice = db.Invoices.Find(id);
            return View("Detail");
        }

        [HttpPost]
        [Route("process")]
        public IActionResult Process(int id)
        {
            var invoice = db.Invoices.Find(id);
            invoice.Status = false;
            db.SaveChanges();
            return RedirectToAction("Index", "Invoice", new { area = "admin"});
        }
    }
}