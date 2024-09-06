using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Models.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        [MaxLength(100, ErrorMessage = "Maxlength is 100 characters")]
        public string? Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}