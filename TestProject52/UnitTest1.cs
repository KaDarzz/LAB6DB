using Lab6.Controllers;
using Lab6.Data;
using Lab6.Models;
using LAB6.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestProject52
{
    public class BaseControllerTest
    {
        protected readonly Mock<DbSet<PerformedService>> _mockPerformedServiceSet;
        protected readonly Mock<DbSet<Client>> _mockClientSet;
        protected readonly Mock<HairdressingContext> _mockContext;
        protected PerformedServicesAPIController _performedServicesController;
        protected ClientsAPIController _clientsController;

        public BaseControllerTest()
        {
            _mockPerformedServiceSet = new Mock<DbSet<PerformedService>>();
            _mockClientSet = new Mock<DbSet<Client>>();
            _mockContext = new Mock<HairdressingContext>();

            _mockContext.Setup(m => m.PerformedServices).Returns(_mockPerformedServiceSet.Object);
            _mockContext.Setup(m => m.Clients).Returns(_mockClientSet.Object);

            _performedServicesController = new PerformedServicesAPIController(_mockContext.Object);
            _clientsController = new ClientsAPIController(_mockContext.Object);
        }


        [Fact]
        public async Task PutPerformedService_ReturnsBadRequest_ForMismatchedIds()
        {
            // Arrange
            var service = new PerformedService { Id = 1 };

            // Act
            var result = await _performedServicesController.PutPerformedService(2, service);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PostPerformedService_CreatesNewService()
        {
            // Arrange
            var newService = new PerformedService();

            // Act
            var result = await _performedServicesController.PostPerformedService(newService);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetPerformedService", createdAtActionResult.ActionName);
            Assert.Equal(newService, createdAtActionResult.Value);
        }



        [Fact]
        public async Task GetClient_ReturnsNotFound_WhenClientDoesNotExist()
        {
            // Arrange
            int nonExistentId = 1;
            _mockClientSet.Setup(m => m.FindAsync(nonExistentId)).ReturnsAsync((Client)null);

            // Act
            var result = await _clientsController.GetClient(nonExistentId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}