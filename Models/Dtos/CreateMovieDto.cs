namespace MoviesAPI.Models.Dtos
{
    public class CreateMovieDto
    {public string? Name { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string? ImageUrl { get; set; }
        public enum CreateClasificationType
        {
            AA,
            A,
            B,
            B15,
            C,
            D
        }
        public CreateClasificationType Clasification { get; set; }
        public int CategoryId { get; set; }
    }
}