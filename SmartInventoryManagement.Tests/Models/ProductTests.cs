using Xunit;
using SmartInventoryManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartInventoryManagement.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Product_WithValidData_ShouldBeValid()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.99m,
                QuantityInStock = 100,
                CategoryId = 1
            };

            // Act
            var validationContext = new ValidationContext(product);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void Product_WithEmptyName_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Name = "",
                Description = "Test Description",
                Price = 10.99m,
                QuantityInStock = 100,
                CategoryId = 1
            };

            // Act
            var validationContext = new ValidationContext(product);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Name"));
        }

        [Fact]
        public void Product_WithNegativePrice_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = -10.99m,
                QuantityInStock = 100,
                CategoryId = 1
            };

            // Act
            var validationContext = new ValidationContext(product);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Price"));
        }

        [Fact]
        public void Product_ValidQuantity_ShouldSetQuantity()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.99m,
                QuantityInStock = 100
            };

            // Assert
            Assert.Equal(100, product.QuantityInStock);
        }

        [Fact]
        public void Product_NegativeQuantity_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.99m,
                QuantityInStock = -10,
                LowStockThreshold = 5,
                CategoryId = 1
            };

            // Act
            var validationContext = new ValidationContext(product);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.MemberNames.Contains("QuantityInStock"));
        }

        [Fact]
        public void Product_ZeroQuantity_ShouldBeValid()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.99m,
                QuantityInStock = 0
            };

            // Assert
            Assert.Equal(0, product.QuantityInStock);
        }

        [Fact]
        public void Product_MaxQuantity_ShouldBeValid()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.99m,
                QuantityInStock = int.MaxValue
            };

            // Assert
            Assert.Equal(int.MaxValue, product.QuantityInStock);
        }
    }
} 