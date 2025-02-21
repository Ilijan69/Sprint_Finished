using Categories2024.Controllers;
using Categories2024.Data;
using Categories2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Categories2024.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<ILogger<HomeController>> _mockLogger;
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            // Create in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb" + System.Guid.NewGuid().ToString())
                .Options;

            // Create actual DbContext with in-memory database
            _context = new ApplicationDbContext(options);

            // Seed test data
            _context.Categories.AddRange(new List<Category>
            {
                new Category { Id = 1, Name = "Fishing Rods", CategoryOrder = 1 },
                new Category { Id = 2, Name = "Baits", CategoryOrder = 2 }
            });
            _context.SaveChanges();

            // Mock logger
            _mockLogger = new Mock<ILogger<HomeController>>();

            // Create controller with real context and mock logger
            _controller = new HomeController(_mockLogger.Object, _context);
        }

        [Test]
        public async Task Index_ReturnsViewWithCategories()
        {
            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            var model = result.Model as IEnumerable<Category>;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count(), Is.EqualTo(2));
            Assert.That(model.First().Name, Is.EqualTo("Fishing Rods"));
        }

        [Test]
        public void Create_Get_ReturnsView()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Create_Post_ValidCategory_RedirectsToIndex()
        {
            // Arrange
            var newCategory = new Category { Id = 3, Name = "Fishing Reels", CategoryOrder = 3 };

            // Act
            var result = _controller.Create(newCategory) as RedirectToActionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("Index"));

            // Verify the category was actually added to the database
            var addedCategory = _context.Categories.Find(3);
            Assert.That(addedCategory, Is.Not.Null);
            Assert.That(addedCategory.Name, Is.EqualTo("Fishing Reels"));
        }

        [Test]
        public void Create_Post_NullCategory_RedirectsToIndex()
        {
            // Act
            var result = _controller.Create(null) as RedirectToActionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _controller.Dispose();
        }
    }
}

