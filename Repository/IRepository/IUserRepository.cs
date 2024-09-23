using MoviesAPI.Models;
using MoviesAPI.Models.Dtos;

namespace MoviesAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int userId);
        bool IsUniqueUser(string userName);
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
        Task<User> Register(UserRegisterDto userRegisterDto);
    }
}