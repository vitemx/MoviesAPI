using AutoMapper;
using MoviesAPI.Models;
using MoviesAPI.Models.Dtos;

namespace MoviesAPI.MoviesMappers
{
    public class MoviesMapper : Profile
    {
        public MoviesMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Movie, CreateMovieDto>().ReverseMap();
        }
    }
}