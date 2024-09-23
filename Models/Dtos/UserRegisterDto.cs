using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models.Dtos
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "This field is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}