using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/slideshow")]
    public class SlideShowController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private IHostingEnvironment ihostingEnvironment;
        public SlideShowController(DatabaseContext _db, IHostingEnvironment _ihostingEnvironment)
        {
            db = _db;
            ihostingEnvironment = _ihostingEnvironment;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.slideshows = db.SlideShows.ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var slideshow = new SlideShow();
            return View("Add", slideshow);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(SlideShow slideshow, IFormFile photo)
        {
            var fileName = DateTime.Now.ToString("MMddyyyy") + photo.FileName;
            var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "slideshows", fileName);
            var stream = new FileStream(path, FileMode.Create);
            photo.CopyToAsync(stream);
            slideshow.Name = fileName;
            db.SlideShows.Add(slideshow);
            db.SaveChanges();
            return RedirectToAction("Index", "slideshow", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                var slideshow = db.SlideShows.Find(id);
                db.SlideShows.Remove(slideshow);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }
            return RedirectToAction("Index", "slideshow", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var slideshow = db.SlideShows.Find(id);
            return View("Edit", slideshow);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, SlideShow slideshow, IFormFile photo)
        {
            var currentSlideshow = db.SlideShows.Find(slideshow.Id);
            
            if (photo != null && !string.IsNullOrEmpty(photo.FileName))
            {
                var fileName = DateTime.Now.ToString("MMddyyyy") + photo.FileName;
                var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "slideshows", fileName);
                var stream = new FileStream(path, FileMode.Create);
                photo.CopyToAsync(stream);
                currentSlideshow.Name = fileName;
            }
            currentSlideshow.Status = slideshow.Status;
            currentSlideshow.Title = slideshow.Title;
            currentSlideshow.Description = slideshow.Description;
            db.SaveChanges();
            return RedirectToAction("Index", "slideshow", new { area = "admin" });
        }
    }
}
