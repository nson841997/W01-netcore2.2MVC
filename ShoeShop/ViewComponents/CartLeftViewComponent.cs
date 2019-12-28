using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Helpers;
using ShoeShop.Models;

namespace ShoeShop.ViewComponents
{
    [ViewComponent(Name ="CartLeft")]
    public class CartLeftViewComponent : ViewComponent
    {
        private DatabaseContext db;
        public CartLeftViewComponent(DatabaseContext _db)
        {
            this.db = _db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                ViewBag.countItem = 0;
                ViewBag.Total = 0;
            } else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                ViewBag.Total = cart.Sum(it => it.Price * it.Quantity);
                ViewBag.countItem = cart.Count;
            }
            return View("Index");
        }
    }
}
