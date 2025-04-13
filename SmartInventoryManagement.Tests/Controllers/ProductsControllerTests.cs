using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartInventoryManagement.Controllers;
using SmartInventoryManagement.Data;
using SmartInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartInventoryManagement.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductsController _controller;
        private readonly ILogger<ProductsController> _logger;

        public ProductsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<ProductsController>();
            _controller = new ProductsController(_context, _logger);

            // Seed test data
            _context.Categories.Add(new Category { Id = 1, Name = "Test Category" });
            _context.SaveChanges();

            _context.Products.AddRange(
                new Product { Id = 1, Name = "Product 1", Price = 10.99m, QuantityInStock = 100, CategoryId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 20.99m, QuantityInStock = 50, CategoryId = 1 }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task Index_ReturnsViewWithProducts()
        {
            // Act
            var result = await _controller.Index("", null, null, null, null, "") as ViewResult;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_WithValidId_ReturnsViewWithProduct()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Product>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_WithValidProduct_RedirectsToIndex()
        {
            // Arrange
            var product = new Product
            {
                Name = "New Product",
                Description = "Test Description",
                Price = 15.99m,
                QuantityInStock = 75,
                CategoryId = 1
            };

            // Act
            var result = await _controller.Create(product);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(ProductsController.Index), redirectResult.ActionName);
            Assert.Equal(3, _context.Products.Count());
        }

        [Fact]
        public async Task Create_WithInvalidProduct_ReturnsView()
        {
            // Arrange
            var product = new Product
            {
                Name = "", // Invalid: empty name
                Price = -10.99m, // Invalid: negative price
                QuantityInStock = -1, // Invalid: negative quantity
                CategoryId = 1
            };

            // Manually add validation errors to ModelState
            _controller.ModelState.AddModelError("Name", "Name is required");
            _controller.ModelState.AddModelError("Price", "Price cannot be negative");
            _controller.ModelState.AddModelError("QuantityInStock", "Quantity cannot be negative");

            // Act
            var result = await _controller.Create(product);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(viewResult.ViewData.ModelState.IsValid);
            Assert.Equal(3, viewResult.ViewData.ModelState.ErrorCount);
        }

        [Fact]
        public async Task Edit_WithValidId_ReturnsViewWithProduct()
        {
            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Product>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Edit_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Edit(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsViewWithProduct()
        {
            // Act
            var result = await _controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Product>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_WithValidId_RemovesProduct()
        {
            // Act
            await _controller.DeleteConfirmed(1);

            // Assert
            Assert.Null(await _context.Products.FindAsync(1));
            Assert.Equal(1, _context.Products.Count());
        }
    }
} 