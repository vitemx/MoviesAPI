using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.Models;

namespace MoviesAPI.Repository.IRepository
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();
        ICollection<Movie> GetMoviesFromCategory(int categoryId);
        IEnumerable<Movie> SearchMovie(string name);
        Movie GetMovie(int movieId);
        bool MovieExits(int movieId);
        bool MovieExits(string movieName);
        bool CreateMovie(Movie movie);
        bool UpdateMovie(Movie movie);
        bool DeleteMovie(Movie movie);
        bool Save();
    }
}