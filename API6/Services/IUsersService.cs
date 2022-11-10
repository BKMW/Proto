using API6.Models;

namespace API6.Services
{
    public interface IUsersService
    {
         Task<List<User>> GetAllUsers();
    }
}
