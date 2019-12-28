using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoeShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoeShop.Areas.Admin.Models.ViewModels
{
    public class ProductViewModels
    {
        public Product Product { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
