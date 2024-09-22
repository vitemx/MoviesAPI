using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAPI.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string? ImageUrl { get; set; }
        public enum ClasificationType
        {
            AA,
            A,
            B,
            B15,
            C,
            D
        }
        public ClasificationType Clasification { get; set; }
        public DateTime CreationDate { get; set; }
        public int CategoryId { get; set; }
    }
}
