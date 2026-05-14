using ASPPenguinWings.Data;
using ASPPenguinWings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPPenguinWings.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Customer> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<Customer> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            string lognatUser = _userManager.GetUserId(User);

            if (User.IsInRole("Admin"))
            {
                var applicationDbContext = _context.Orders
                    .Include(o => o.Customers)
                    .Include(o => o.Products)
                    .Where(o => o.IsCompleted);

                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Orders
                    .Include(o => o.Customers)
                    .Include(o => o.Products)
                    .Where(o => o.CustomerId == lognatUser && !o.IsCompleted);

                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }
        public async Task<IActionResult> CreateById(int productId)
        {
            string userId = _userManager.GetUserId(User);

            var product = await _context.Products.FindAsync(productId);

            if (product == null || product.Quantity <= 0)
            {
                return RedirectToAction("Index");
            }

            var existingOrder = _context.Orders
                .FirstOrDefault(o => o.ProductId == productId
                                  && o.CustomerId == userId
                                  && !o.IsCompleted);

            if (existingOrder != null)
            {
                existingOrder.Quantity++;
            }
            else
            {
                Order order = new Order
                {
                    DateOn = DateTime.Now,
                    CustomerId = userId,
                    ProductId = productId,
                    Quantity = 1
                };

                _context.Orders.Add(order);
            }

            product.Quantity--;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId")] Order order)
        {
            order.DateOn = DateTime.Now;
            order.CustomerId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,CustomerId,Quantity,DateOn")] Order order)
        {
            order.DateOn = DateTime.Now;

            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
       
        public async Task<IActionResult> Increase(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                var product = await _context.Products.FindAsync(order.ProductId);

                if (product != null && product.Quantity > 0)
                {
                    order.Quantity++;

                    product.Quantity--;

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Decrease(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                var product = await _context.Products.FindAsync(order.ProductId);

                order.Quantity--;

                if (product != null)
                {
                    product.Quantity++;
                }

                if (order.Quantity <= 0)
                {
                    _context.Orders.Remove(order);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Checkout()
        {
            string userId = _userManager.GetUserId(User);

            var orders = _context.Orders
                .Where(o => o.CustomerId == userId && !o.IsCompleted);

            foreach (var order in orders)
            {
                order.IsCompleted = true;
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Поръчката беше успешно завършена!";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Complete(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyOrders()
        {
            string userId = _userManager.GetUserId(User);

            var orders = _context.Orders
                .Include(o => o.Products)
                .Where(o => o.CustomerId == userId)
                .ToList();

            return View(orders);
        }
    }
}
