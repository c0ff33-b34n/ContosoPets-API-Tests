using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using ContosoPets.Api.Data;
using System;
using ContosoPets.Api.Models;
using ContosoPets.Api.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ContosoPets.Tests
{
    public class ProductsControllerTests
    {
        [Fact]
        public void GetAll_ReturnsTypeOfActionResultContainingListOfProducts_WhenCalled()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var productsController = new ProductsController(dbContext);

            // Act
            var products = productsController.GetAll();

            // Assert
            Assert.IsType<ActionResult<List<Product>>>(products);
        }

        [Fact]
        public void GetAll_ReturnsActionResultContainingListOfAllProducts_WhenCalled()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var productsController = new ProductsController(dbContext);

            // Act
            var products = productsController.GetAll();

            // Assert
            Assert.Equal(products.Value.Count(), 5);
        }

        [Fact]
        public void GetById_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var productsController = new ProductsController(dbContext);

            // Act
            var result = productsController.GetById(6);

            // Assert
            var actionResult = Assert.IsType<Task<ActionResult<Product>>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result.Result);
        }

        [Fact]
        public void GetById_ReturnsProduct_ForValidId()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var productsController = new ProductsController(dbContext);

            // Act
            var result = productsController.GetById(3);

            // Assert
            var actionResult = Assert.IsType<Task<ActionResult<Product>>>(result);
            Assert.IsType<Product>(actionResult.Result.Value);
            Assert.Equal(actionResult.Result.Value.Id, 3);
        }

        [Fact]
        public void Create_ReturnsNewlyCreatedProduct()
        {
            // Arrange
            string productName = "5 metre lead";
            decimal price = 4.99M;

            var dbContext = GetDatabaseContext();
            var productsController = new ProductsController(dbContext);

            Product newProduct = new(6, productName, price);

            // Act
            var result = productsController.Create(newProduct);

            // Assert
            var actionResult = Assert.IsType<Task<IActionResult>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Product>(createdAtActionResult.Value);
            Assert.Equal(6, returnValue.Id);
            Assert.Equal(productName, returnValue.Name);
            Assert.Equal(price, returnValue.Price);
        }

        [Fact]
        public void Update_ReturnsBadRequest_ForProductAndIdMismatch()
        {
            // Arrange
            int i = 4;
            string productName = $"Product{i}";
            decimal price = 4.99M;

            var dbContext = GetDatabaseContext();
            var productsController = new ProductsController(dbContext);

            Product updatedProduct = new(i, productName, price);

            // Act
            var result = productsController.Update(3, updatedProduct);

            // Assert
            var actionResult = Assert.IsType<Task<IActionResult>>(result);
            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public void Update_ReturnsNoContent_ForSuccessfulUpdate()
        {
            // Arrange
            int i = 4;
            decimal price = 7.99M;

            var dbContext = GetDatabaseContext();
            var productsController = new ProductsController(dbContext);

            var product = productsController.GetById(i).Result.Value;

            Product updatedProduct = product with {Price = price};

            // Act
            var result = productsController.Update(i, updatedProduct);

            // Assert
            var actionResult = Assert.IsType<Task<IActionResult>>(result);
            Assert.IsType<NoContentResult>(actionResult.Result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var productsController = new ProductsController(dbContext);

            // Act
            var result = productsController.Delete(7);

            // Assert
            var actionResult = Assert.IsType<Task<IActionResult>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public void Delete_ReturnsNoContent_ForDeletingProductWithValidId()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var productsController = new ProductsController(dbContext);

            // Act
            var result = productsController.Delete(3);

            // Assert
            var actionResult = Assert.IsType<Task<IActionResult>>(result);
            Assert.IsType<NoContentResult>(actionResult.Result);
        }

        private ContosoPetsContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ContosoPetsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ContosoPetsContext(options);
            databaseContext.Database.EnsureCreated();

            if (databaseContext.Products.Count() <= 0)
            {
                for (int i = 1; i <= 5; i++)
                {
                    databaseContext.Products.Add(
                        new Product(
                            i,
                            $"Product{i}",
                            0.99M + i
                        ));
                    databaseContext.SaveChanges();
                }
            }
            return databaseContext;
        }
    }
}