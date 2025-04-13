using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartInventoryManagement.Data;
using SmartInventoryManagement.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SmartInventoryManagement.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            // Get all products to populate the dropdown
            var products = await _context.Products
                .Select(p => new { id = p.Id, name = p.Name, price = p.Price })
                .ToListAsync();
            
            ViewBag.Products = products;
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuestName,Email,PhoneNumber,ShippingAddress,OrderItems")] Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    order.OrderDate = DateTime.UtcNow;
                    order.Status = "Pending";
                    
                    _context.Add(order);
                    await _context.SaveChangesAsync();

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, orderId = order.Id });
                    }

                    return RedirectToAction(nameof(Details), new { id = order.Id });
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = new[] { "An error occurred while creating the order. Please try again." } });
                }

                ModelState.AddModelError("", "An error occurred while creating the order. Please try again.");
                return View(order);
            }
        }

        // GET: Orders/Track
        public IActionResult Track()
        {
            return View();
        }

        // POST: Orders/Track
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Track(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Please enter your email address");
                return View();
            }

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.Email == email)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            if (!orders.Any())
            {
                ModelState.AddModelError("", "No orders found for this email address");
                return View();
            }

            return View("TrackResults", orders);
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

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GuestName,Email,PhoneNumber,ShippingAddress,OrderDate,Status")] Order order)
        {
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

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
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
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Search
        public async Task<IActionResult> Search(string searchString)
        {
            var orders = from o in _context.Orders
                         select o;

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.GuestName.Contains(searchString));
            }

            return View(await orders.ToListAsync());
        }

        // GET: Orders/Cart
        public IActionResult Cart()
        {
            return View();
        }
    }
}
