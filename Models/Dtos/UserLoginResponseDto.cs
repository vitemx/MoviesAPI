namespace MoviesAPI.Models.Dtos
{
    public class UserLoginResponseDto
    {
        public User User { get; set; }
        public string? Role { get; set; }
        public string? SessionToken { get; set; }
    }
}