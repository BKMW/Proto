using API.Test.Fixtures;
using API6.Controllers;
using API6.Models;
using API6.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test.Systems.Controllers
{
    public class TesUsersController
    {
        [Fact]
        public async Task GetUsers_OnSuccess_ReturnsStatusCode200()
        {
            //Arrange
            var mockUsersService = new Mock<IUsersService>();
            mockUsersService
                .Setup(service => service.GetAllUsers())
                .ReturnsAsync(UsersFixture.Getusers);
            var users = new UsersController(mockUsersService.Object);
            //Act
            var result = (OkObjectResult) await users.GetUsers();
            //Assert
            result.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task GetUsers_OnSuccess_InvokeUsersService()
        {
            //Arrange
            var mockUsersService = new Mock<IUsersService>();
            mockUsersService
                .Setup(service=>service.GetAllUsers())
                .ReturnsAsync(new List<User>());

            var users = new UsersController(mockUsersService.Object);
            //Act
            var result = await users.GetUsers();
            //Assert
            mockUsersService.Verify(
                service => service.GetAllUsers(), 
                Times.Once()
                );
        }

        [Fact]
        public async Task GetUsers_OnSuccess_ReturnsListOfUsers()
        {
            //Arrange
            var mockUsersService = new Mock<IUsersService>();
            mockUsersService
                .Setup(service => service.GetAllUsers())
                .ReturnsAsync(UsersFixture.Getusers);

            var users = new UsersController(mockUsersService.Object);
            //Act
            var result = await users.GetUsers();
            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (ObjectResult)result;
            objectResult.Value.Should().BeOfType<List<User>>();
        }
        [Fact]
        public async Task GetUsers_OnNoUsersFound_Returns404()
        {
            //Arrange
            var mockUsersService = new Mock<IUsersService>();
            mockUsersService
                .Setup(service => service.GetAllUsers())
                .ReturnsAsync(new List<User>());

            var users = new UsersController(mockUsersService.Object);
            //Act
            var result = await users.GetUsers();
            //Assert
            result.Should().BeOfType<NotFoundResult>();
            var objectResult = (NotFoundResult)result;
            objectResult.StatusCode.Should().Be(404);


        }
    }
}
