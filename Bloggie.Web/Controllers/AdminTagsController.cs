﻿using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;


        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            // Mapping the AddTagRequest to the Domain Model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            bloggieDbContext.Tags.Add(tag);
            bloggieDbContext.SaveChanges();

            return RedirectToAction("List");
        }
        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            // use dbcontext to get all tags
            var tags = bloggieDbContext.Tags.ToList();
            return View(tags);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {   // ist method
            //var tag = bloggieDbContext.Tags.Find(id);

            // second method 
            var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        [ActionName("Edit")]
        public IActionResult Edit(EditTagRequest editTagRequest)
        {

            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName

            };
            var existingTag = bloggieDbContext.Tags.Find(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // save changes 
                bloggieDbContext.SaveChanges();

                // show success notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }

            // show error notification
            return RedirectToAction("List", new { id = editTagRequest.Id });



        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = bloggieDbContext.Tags.Find(editTagRequest.Id);
            if (tag != null)
            {
                bloggieDbContext.Tags.Remove(tag);
                bloggieDbContext.SaveChanges();
                // show success notification
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
