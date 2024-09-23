using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "This field is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? Password { get; set; }
    }
}