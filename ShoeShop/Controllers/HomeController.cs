using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;

namespace ShoeShop.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public HomeController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {
            ViewBag.isHome = true;
            var featuredProduct = db.Products.OrderByDescending(p => p.Id).Where(p => p.Status && p.Featured).Take(4).ToList();
            ViewBag.FeaturedProducts = featuredProduct;
            ViewBag.CountFeaturedProducts = featuredProduct.Count;
            ViewBag.LatestProduct = db.Products.OrderByDescending(p => p.Id).Where(p => p.Status).Take(6).ToList();
            return View();
        }
    }
}