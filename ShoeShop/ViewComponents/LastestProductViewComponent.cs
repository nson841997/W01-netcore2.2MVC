using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;

namespace ShoeShop.ViewComponents
{
    [ViewComponent(Name = "LastestProduct")]
    public class LastestProductViewComponent : ViewComponent
    {
        private DatabaseContext db;
        public LastestProductViewComponent(DatabaseContext _db)
        {
            this.db = _db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> products = db.Products.OrderByDescending(p => p.Id).Where(p => p.Status).Take(3).ToList();
            return View("Index", products);
        }
    }
}
