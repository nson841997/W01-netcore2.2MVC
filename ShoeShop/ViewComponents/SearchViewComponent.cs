using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;

namespace ShoeShop.ViewComponents
{
    [ViewComponent(Name ="Search")]
    public class SearchViewComponent : ViewComponent
    {
        private DatabaseContext db;
        public SearchViewComponent(DatabaseContext _db)
        {
            this.db = _db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string keyword = HttpContext.Request.Query["keyword"].ToString();
            int categoryId = -1;
            if (HttpContext.Request.Query.ContainsKey("categoryId"))
            {
                categoryId = int.Parse(HttpContext.Request.Query["categoryId"].ToString());
            }
            List<Category> categories = db.Categories.Where(c => c.Status && c.Parent == null).ToList();
            return View("Index", new InvokeResult() { keyword = keyword, categoryId = categoryId, categories = categories } );
        }
    }
    public class InvokeResult
    {
        public string keyword { get; set; }
        public int categoryId { get; set; }
        public List<Category> categories { get; set; }
    }
}
