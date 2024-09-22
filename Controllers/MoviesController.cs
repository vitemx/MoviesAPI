using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Models.Dtos;
using MoviesAPI.Repository.IRepository;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepository movieRepository, IMapper mapper)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMovies()
        {
            var moviesList = _movieRepository.GetMovies();
            var moviesListDto = new List<MovieDto>();

            foreach (var lista in moviesList)
            {
                moviesListDto.Add(_mapper.Map<MovieDto>(lista));
            }

            return Ok(moviesListDto);
        }

        [HttpGet("{movieId:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int movieId)
        {
            var movieItem = _movieRepository.GetMovie(movieId);

            if (movieItem == null)
            {
                return NotFound();
            }

            var movieItemDto = _mapper.Map<MovieDto>(movieItem);
            return Ok(movieItemDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AddMovie([FromBody] CreateMovieDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null)
            {
                return BadRequest(ModelState);
            }

            if (_movieRepository.MovieExits(request.Name ?? ""))
            {
                ModelState.AddModelError("", $"The movie if exits");
                return StatusCode(404, ModelState);
            }

            var movie = _mapper.Map<Movie>(request);

            if (!_movieRepository.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"Error save the movie {movie.Name}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetMovie", new { movieId = movie.Id }, movie);
        }

        [HttpPatch("{movieId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdateMovie(int movieId, [FromBody] MovieDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null || movieId != request.Id)
            {
                return BadRequest(ModelState);
            }

            var movieExits = _movieRepository.GetMovie(movieId);
            if (movieExits == null)
            {
                return NotFound($"The movie no found");
            }

            var movie = _mapper.Map<Movie>(request);

            if (!_movieRepository.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Error update the movie {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{movieId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteMovie(int movieId)
        {
            if (!_movieRepository.MovieExits(movieId))
            {
                return NotFound($"category not exits");
            }

            var movie = _movieRepository.GetMovie(movieId);

            if (!_movieRepository.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"Error delete the category {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("GetMovieWithCategory/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovieWithCategory(int categoryId)
        {
            var listaPeliculas = _movieRepository.GetMoviesFromCategory(categoryId);
            
            if (!listaPeliculas.Any())
            {
                return NotFound();
            }

            var itemPelicula = new List<MovieDto>();
            foreach (var movie in listaPeliculas)
            {
                itemPelicula.Add(_mapper.Map<MovieDto>(movie));
            }

            return Ok(itemPelicula);
        }

        [HttpGet("SearchMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SearchMovie(string name)
        {
            try
            {
                var result = _movieRepository.SearchMovie(name);
                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fatal Error");
            }
        }
    }
}