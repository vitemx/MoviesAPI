using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Models;
using MoviesAPI.Repository.IRepository;

namespace MoviesAPI.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MovieRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CreateMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;
            _dbContext.Movie.Add(movie);
            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _dbContext.Remove(movie);
            return Save();
        }

        public Movie GetMovie(int movieId)
        {
            return _dbContext.Movie.Where(m => m.Id == movieId).FirstOrDefault() ??  new();
        }

        public ICollection<Movie> GetMovies()
        {
            return _dbContext.Movie.ToList();
        }

        public ICollection<Movie> GetMoviesFromCategory(int categoryId)
        {
            return _dbContext.Movie.Where(m => m.CategoryId == categoryId).ToList();
        }

        public bool MovieExits(int movieId)
        {
            return _dbContext.Movie.Any(m => m.Id == movieId);
        }

        public bool MovieExits(string movieName)
        {
            return _dbContext.Movie.Any(m => m.Name == movieName);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        public IEnumerable<Movie> SearchMovie(string name)
        {
            return _dbContext.Movie.Where(m => m.Name.Contains(name) || m.Description.Contains(name)).ToList();
        }

        public bool UpdateMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;

            var movieExits = _dbContext.Movie.Find(movie.Id);
            if (movieExits != null) 
            {
                _dbContext.Entry(movieExits).CurrentValues.SetValues(movie);
            }
            else
            {
                _dbContext.Update(movie);
            }

            return Save();
        }
    }
}
