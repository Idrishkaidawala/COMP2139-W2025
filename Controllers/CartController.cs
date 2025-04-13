using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInventoryManagement.Data;
using SmartInventoryManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SmartInventoryManagement.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index", "Products");
            }

            var cart = GetCart();
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                cart.Items.Add(new CartItem { Product = product, ProductId = product.Id, Quantity = 1 });
            }
            else
            {
                cartItem.Quantity++;
            }

            SaveCart(cart);
            TempData["Success"] = "Product added to cart.";
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                SaveCart(cart);
                TempData["Success"] = "Product removed from cart.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder([FromForm] string guestName, [FromForm] string guestEmail)
        {
            var cart = GetCart();

            if (!cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(guestName) || string.IsNullOrEmpty(guestEmail))
            {
                TempData["Error"] = "Please provide your name and email.";
                return RedirectToAction("Checkout");
            }

            // Validate products and get fresh prices
            var orderItems = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    TempData["Error"] = $"Product with ID {item.ProductId} is no longer available.";
                    return RedirectToAction("Checkout");
                }

                if (product.QuantityInStock < item.Quantity)
                {
                    TempData["Error"] = $"Not enough stock available for {product.Name}.";
                    return RedirectToAction("Checkout");
                }

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });

                // Update stock
                product.QuantityInStock -= item.Quantity;
            }

            var order = new Order
            {
                GuestName = guestName,
                Email = guestEmail,
                PhoneNumber = "",
                ShippingAddress = "",
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = orderItems
            };

            // Calculate total price
            order.TotalPrice = orderItems.Sum(item => item.TotalPrice);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            TempData["Success"] = $"Order placed successfully. Your order number is {order.Id}.";

            return RedirectToAction("Details", "Orders", new { id = order.Id });
        }

        private Cart GetCart()
        {
            return HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart { Items = new List<CartItem>() };
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }
    }
}
