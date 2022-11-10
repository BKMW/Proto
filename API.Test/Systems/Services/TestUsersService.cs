using API6.Services;

namespace API.Test.Systems.Services
{
    public class TestUsersService
    {
        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesHttpGetRequest()
        {
            //Arrange
            var service = new UsersService();
            //Act
            await service.GetAllUsers();
            //Assert
            //Verify HTTP request is made!
        }
    }
}
