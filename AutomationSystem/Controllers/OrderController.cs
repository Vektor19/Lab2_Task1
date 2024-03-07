using AutomationSystem.Areas.Identity.Data;
using AutomationSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using AutomationSystem.Observers;

namespace AutomationSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly AutomationSystemContext _context;
        private readonly UserManager<AutomationSystemUser> _userManager;
        private List<IOrderObserver> observers = new List<IOrderObserver>();
        public OrderController(AutomationSystemContext context, UserManager<AutomationSystemUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            foreach (var master in _context.MasterDatas)
            {
                observers.Add(master);
            }
        }
        
        public IActionResult PlaceOrder(int FixCategoryId)
        {

            ViewBag.FixCategoryId = FixCategoryId;
            return View(new Technik());
        }

        [Authorize(Roles = "Manager")]
        public IActionResult UpdateOrder(int orderId)
        {

            ViewBag.orderId = orderId;
            Order tempOrder = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            ViewBag.FixCategories = _context.FixCategories.ToList();
            return View(tempOrder);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Technik technik, int FixCategoryId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }
            var order = new Order
            {
                Technik = technik,
                FixCategoryId = FixCategoryId,
                data = DateTime.Now,
                Status = false,
                ClientId = currentUser.Id,
                Price = 0,
                MasterId = null
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Orders.Add(order);
                        _context.SaveChanges();
                    }
                    transaction.Commit(); //функція транзакції
                    return RedirectToAction("Order");
                }
                catch (Exception)
                {
                    transaction.Rollback(); //відміна транзакції у випадку помилки
                    throw;
                }
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Order order, int orderId)
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

                    var orderToUpdate = _context.Orders.FirstOrDefault(o => o.Id == orderId);
                    if (orderToUpdate != null)
                    {
                        orderToUpdate.Status = order.Status;
                        orderToUpdate.FixCategoryId = order.FixCategoryId;
                        orderToUpdate.Price = order.Price;
                        orderToUpdate.MasterId = order.MasterId;
                        _context.Entry(orderToUpdate).State = EntityState.Modified;
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    return RedirectToAction("OrderManaging", "Manager");
                }
                catch (Exception)
                {
                    transaction.Rollback(); //відміна транзакції у випадку помилки
                    throw;
                }
            }
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }

            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            order.AutomationSystemUser = _context.AutomationSystemUsers.FirstOrDefault(u => u.Id == order.ClientId);
            order.Technik = _context.Techniks.FirstOrDefault(t => t.Id == order.TechnikId);
            order.FixCategory = _context.FixCategories.FirstOrDefault(f => f.Id == order.FixCategoryId);
            return View(order);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }
            var orderToDelete = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            var technikToDelete = _context.Techniks.FirstOrDefault(t => t.Id == orderToDelete.TechnikId);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (orderToDelete != null)
                    {
                        if (technikToDelete != null)
                        {

                            _context.Entry(technikToDelete).State = EntityState.Deleted;
                        }
                        //_context.Techniks.Remove(technikToDelete);
                        _context.Entry(orderToDelete).State = EntityState.Deleted;
                        //_context.Orders.Remove(orderToDelete);
                        _context.SaveChanges();
                        transaction.Commit();
                        
                    }
                    return RedirectToAction("OrderManaging", "Manager");
                }
                catch(Exception ex) 
                {
                    transaction.Rollback();
                    throw;
                }
            }       
        }

        [Authorize]
        public async Task<IActionResult> OrderAsync(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Error");
            }

            var userOrders = _context.Orders
                                    .Where(o => o.ClientId == currentUser.Id)
                                    .ToList();
            foreach (var order in userOrders)
            {
                order.AutomationSystemUser = _context.AutomationSystemUsers.FirstOrDefault(u => u.Id == order.ClientId);
                order.Technik = _context.Techniks.FirstOrDefault(t => t.Id == order.TechnikId);
                order.FixCategory = _context.FixCategories.FirstOrDefault(f => f.Id == order.FixCategoryId);
            }
            Notify();
            return View(userOrders);
        }
        public void Add(IOrderObserver observer)
        {
            observers.Add(observer);
        }

        public void Remove(IOrderObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update();
                _context.SaveChanges();
            }
        }
    }
}
