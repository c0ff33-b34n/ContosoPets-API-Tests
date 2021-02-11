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