using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInventoryManagement.Data;
using SmartInventoryManagement.Models;

namespace SmartInventoryManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            ViewData["Title"] = "Welcome";
            return View("Index");
        }

        // Get dashboard data
        var totalProducts = await _context.Products.CountAsync();
        var totalCategories = await _context.Categories.CountAsync();
        var lowStockProducts = await _context.Products
            .Where(p => p.QuantityInStock <= p.LowStockThreshold)
            .CountAsync();
        var recentOrders = await _context.Orders
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .ToListAsync();

        ViewBag.TotalProducts = totalProducts;
        ViewBag.TotalCategories = totalCategories;
        ViewBag.LowStockProducts = lowStockProducts;
        ViewBag.RecentOrders = recentOrders;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}