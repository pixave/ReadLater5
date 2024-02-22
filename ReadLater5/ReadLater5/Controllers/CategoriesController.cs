using Entity;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ReadLater5.Controllers
{
    public class CategoriesController : Controller
    {
        ICategoryService _categoryService;
        string _UserID;

        
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            
            if(this.User != null)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                if(currentUserID != null)
                {
                    _UserID = currentUserID;
                    ViewBag.UserID = _UserID;
                }
            }

        }
        // GET: Categories
        public IActionResult Index()
        {
            if(this != null)
            {
                if (this.User != null)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                    if(currentUserData != null)
                    {
                        var currentUserID = currentUserData.Value;
                        if (currentUserID != null)
                        {
                            _UserID = currentUserID;
                            ViewBag.UserID = _UserID;
                        }

                    }
                }
            }
            List<Category> model = _categoryService.GetCategories();
            
        
            return View(model);
        }

        // GET: Categories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserID != null)
            {
                _UserID = currentUserID;
            }

            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(category);

        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.UserID= _UserID;
                _categoryService.CreateCategory(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
                public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.UserID = _UserID;
                _categoryService.UpdateCategory(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryService.GetCategory(id);
            _categoryService.DeleteCategory(category);
            return RedirectToAction("Index");
        }
    }
}
