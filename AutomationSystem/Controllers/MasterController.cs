using AutomationSystem.Areas.Identity.Data;
using AutomationSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomationSystem.Controllers
{
    [Authorize(Roles ="Master")]
    public class MasterController : Controller
    {
        private readonly AutomationSystemContext _context;
        private readonly UserManager<AutomationSystemUser> _userManager;
        public MasterController(AutomationSystemContext context, UserManager<AutomationSystemUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }
            var masterData = _context.MasterDatas.FirstOrDefault(x => x.Id == currentUser.Id);
            ViewBag.isNewOrdersChecked = masterData.isNewOrdersChecked;
            return View();
            
        }
        public async Task<IActionResult> FreeWorks() 
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }
            var masterData = _context.MasterDatas.FirstOrDefault(x => x.Id == currentUser.Id);
            masterData.isNewOrdersChecked = true;
            _context.SaveChanges();
            var avaibleOrders = _context.Orders.Where(o => o.MasterId == null && (!o.Status)).ToList();
            foreach (var order in avaibleOrders)
            {
                order.AutomationSystemUser = _context.AutomationSystemUsers.FirstOrDefault(u => u.Id == order.ClientId);
                order.Technik = _context.Techniks.FirstOrDefault(t => t.Id == order.TechnikId);
                order.FixCategory = _context.FixCategories.FirstOrDefault(f => f.Id == order.FixCategoryId);
            }
            return View(avaibleOrders);
            
        }
        public IActionResult IndexWithNewOrders(List<Order> avaibleOrders)
        {
            return View(avaibleOrders);
        }
        public async Task<IActionResult> Works()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }
            var masterData=_context.MasterDatas.FirstOrDefault(x => x.Id == currentUser.Id);
            masterData.isNewOrdersChecked = true;
            _context.SaveChanges();
            var masterOrders = _context.Orders
                                    .Where(o => o.MasterId == currentUser.Id)
                                    .ToList();
            foreach (var order in masterOrders)
            {
                order.AutomationSystemUser = _context.AutomationSystemUsers.FirstOrDefault(u => u.Id == order.ClientId);
                order.Technik = _context.Techniks.FirstOrDefault(t => t.Id == order.TechnikId);
                order.FixCategory = _context.FixCategories.FirstOrDefault(f => f.Id == order.FixCategoryId);
            }
            return View(masterOrders);
        }

        public async Task<IActionResult> TakeRepair(int orderId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }
            var countWorks=_context.Orders.Count(o => o.MasterId == currentUser.Id);
            if (countWorks<3)
            {
                var orderToUpdate = _context.Orders.FirstOrDefault(o => o.Id == orderId);
                orderToUpdate.MasterId = currentUser.Id;
                _context.SaveChanges();
                return RedirectToAction("Works");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CompleteWork(int orderId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }

            var orderToUpdate = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            orderToUpdate.Status = true;
            _context.SaveChanges();

            return RedirectToAction("Works");
        }

    }
}
