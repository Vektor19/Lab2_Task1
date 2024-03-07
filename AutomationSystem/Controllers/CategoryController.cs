using AutomationSystem.Areas.Identity.Data;
using AutomationSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AutomationSystem.Controllers
{
    [Authorize(Roles = "Manager")]
    public class CategoryController : Controller
    {
        private readonly AutomationSystemContext _context;
        private readonly UserManager<AutomationSystemUser> _userManager;
        public CategoryController(AutomationSystemContext context, UserManager<AutomationSystemUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public IActionResult CreateCategory()
        {
            return View(new FixCategory());
        }

        public IActionResult UpdateCategory(int fixCategoryId)
        {

            ViewBag.Id = fixCategoryId;
            FixCategory tempCategory = _context.FixCategories.FirstOrDefault(c => c.Id == fixCategoryId);
            return View(tempCategory);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(FixCategory fixCategory)
        {
            if (ModelState.IsValid)
            {

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return RedirectToAction("Error");
                }
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            _context.FixCategories.Add(fixCategory);
                            _context.SaveChanges();
                        }
                        transaction.Commit(); //функція транзакції
                        return RedirectToAction("Categories", "Home");
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); //відміна транзакції у випадку помилки
                        throw;
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(FixCategory fixCategory, int fixCategoryId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }

            var categoryToUpdate = _context.FixCategories.FirstOrDefault(c=>c.Id==fixCategoryId);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (categoryToUpdate != null)
                    {
                        categoryToUpdate.Description = fixCategory.Description;
                        categoryToUpdate.ImagePath = fixCategory.ImagePath;
                        _context.Entry(categoryToUpdate).State = EntityState.Modified;
                        _context.SaveChanges();
                        transaction.Commit();
                    }

                    return RedirectToAction("CategoryManaging", "Manager");
                }
                catch (Exception)
                {
                    transaction.Rollback(); //відміна транзакції у випадку помилки
                    throw;
                }
            }
            
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCategory(int fixCategoryId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }
            var categoryToDelete = _context.FixCategories.FirstOrDefault(c => c.Id == fixCategoryId);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (categoryToDelete != null)
                    {
                        _context.Entry(categoryToDelete).State = EntityState.Deleted;
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    return RedirectToAction("CategoryManaging", "Manager");
                }
                catch (Exception)
                {
                    transaction.Rollback(); //відміна транзакції у випадку помилки
                    throw;
                }
            }
            
        }
        public async Task<IActionResult> CategoryAsync(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }
            var categories = _context.FixCategories.ToList();

            return View(categories);
        }
    }
}
