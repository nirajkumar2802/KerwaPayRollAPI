using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Version.Controllers;
using Version.EntityModels;
using Version.InfraStructure;
using Xunit;

namespace KerwaUnitTest
{
    public class EmployeeControllerTests
    {

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfEmployees()
        {
            // Arrange
            var mockRepo = new Mock<IKerwaEmployeeRepo>();
            var expectedEmployees = new List<KerwaEmployee>
            {
                new KerwaEmployee { Id = 1, FirstName = "John", LastName = "Doe" },
                new KerwaEmployee { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };
            mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(expectedEmployees);

            var mockLogger = new Mock<ILogger<EmployeeController>>();

            var controller = new EmployeeController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualEmployees = Assert.IsAssignableFrom<List<KerwaEmployee>>(okResult.Value);
            Assert.Equal(expectedEmployees, actualEmployees);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenNoEmployees()
        {
            // Arrange
            var mockRepo = new Mock<IKerwaEmployeeRepo>();
            mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<KerwaEmployee>());

            var mockLogger = new Mock<ILogger<EmployeeController>>();

            var controller = new EmployeeController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsStatusCode500_WhenExceptionThrown()
        {
            // Arrange
            var mockRepo = new Mock<IKerwaEmployeeRepo>();
            mockRepo.Setup(repo => repo.GetAll()).ThrowsAsync(new Exception("Test exception"));

            var mockLogger = new Mock<ILogger<EmployeeController>>();

            var controller = new EmployeeController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
