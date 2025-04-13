using Xunit;
using SmartInventoryManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartInventoryManagement.Tests.Models
{
    public class OrderTests
    {
        [Fact]
        public void Order_WithValidData_ShouldBeValid()
        {
            // Arrange
            var order = new Order
            {
                GuestName = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                ShippingAddress = "123 Main St",
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalPrice = 100.00m,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = 1,
                        Quantity = 2,
                        UnitPrice = 50.00m
                    }
                }
            };

            // Act
            var validationContext = new ValidationContext(order);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void Order_WithEmptyGuestName_ShouldBeInvalid()
        {
            // Arrange
            var order = new Order
            {
                GuestName = "",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                ShippingAddress = "123 Main St",
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalPrice = 100.00m
            };

            // Act
            var validationContext = new ValidationContext(order);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v => v.MemberNames.Contains("GuestName"));
        }

        [Fact]
        public void Order_WithInvalidEmail_ShouldBeInvalid()
        {
            // Arrange
            var order = new Order
            {
                GuestName = "John Doe",
                Email = "invalid-email",
                PhoneNumber = "1234567890",
                ShippingAddress = "123 Main St",
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalPrice = 100.00m
            };

            // Act
            var validationContext = new ValidationContext(order);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Email"));
        }

        [Fact]
        public void Order_WithNegativeTotalPrice_ShouldBeInvalid()
        {
            // Arrange
            var order = new Order
            {
                GuestName = "Test Guest",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                ShippingAddress = "123 Test St",
                TotalPrice = -10.99m,
                Status = "Pending"
            };

            // Act
            var validationContext = new ValidationContext(order);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.MemberNames.Contains("TotalPrice"));
        }

        [Fact]
        public void Order_WithEmptyOrderItems_ShouldBeValid()
        {
            // Arrange
            var order = new Order
            {
                GuestName = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                ShippingAddress = "123 Main St",
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalPrice = 0.00m,
                OrderItems = new List<OrderItem>()
            };

            // Act
            var validationContext = new ValidationContext(order);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }
    }
} 