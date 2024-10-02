using Microsoft.IdentityModel.Tokens;
using MoviesAPI.Data;
using MoviesAPI.Models;
using MoviesAPI.Models.Dtos;
using MoviesAPI.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace MoviesAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private string _secretKey;

        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }
        public User GetUser(int userId)
        {
            return _dbContext.User.FirstOrDefault(u => u.Id == userId) ?? new();
        }

        public ICollection<User> GetUsers()
        {
            return _dbContext.User.OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string userName)
        {
            var user = _dbContext.User.FirstOrDefault(u => u.UserName == userName);
            return user == null;
        }

        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            var encryptPassword = GetMd5(userLoginDto.Password ?? "");
            var user  = _dbContext.User.FirstOrDefault( u => 
                u.UserName.ToLower() == userLoginDto.UserName.ToLower()
                && u.Password == encryptPassword);

            if (user == null)
            {
                return new UserLoginResponseDto()
                {
                    SessionToken = "",
                    User = null
                };
            }

            var tokenManager = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName ?? "".ToString()),
                    new Claim(ClaimTypes.Role, user.Role ?? "")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenManager.CreateToken(tokenDescriptor);

            UserLoginResponseDto userLoginResponseDto = new()
            {
                SessionToken = tokenManager.WriteToken(token),
                User = user
            };

            return userLoginResponseDto;
        }

        public async Task<User> Register(UserRegisterDto userRegisterDto)
        {
            var encryptPassword = GetMd5(userRegisterDto.Password ?? "");

            User user = new User()
            {
                UserName = userRegisterDto.UserName,
                Password = encryptPassword,
                Name = userRegisterDto.Name,
                Role = userRegisterDto.Role
            };

            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();
            user.Password = encryptPassword;

            return user;
        }

        private string GetMd5(string value)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }
    }  
}