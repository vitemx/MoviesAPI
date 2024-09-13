using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.Models.Dtos;
using MoviesAPI.Repository.IRepository;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryRepository categories, IMapper mapper)
        {
            _categoryRepo = categories;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            var listaCategorias = _categoryRepo.GetCategories();
            var listaCategoriasDto = new List<CategoryDto>();

            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoryDto>(lista));
            }

            return Ok(listaCategoriasDto);
        }


        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoryId)
        {
            var categoryItem = _categoryRepo.GetCategory(categoryId);

            if (categoryItem == null)
            {
                return NotFound();
            }

            var itemCategoryDto = _mapper.Map<CategoryDto>(categoryItem);
            return Ok(itemCategoryDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AddCategory([FromBody] CreateCategoryDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request ==  null)
            {
                return BadRequest(ModelState);
            }

            if (_categoryRepo.CategoryExits(request.Name))
            {
                ModelState.AddModelError("", $"The category if exits");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(request);

            if (!_categoryRepo.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Error save the category {category.Name}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);
        }

        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdateCategory(int categoryId,  [FromBody] CategoryDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null || categoryId != request.Id)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(request);

            if (!_categoryRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Error update the category {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{categoryId:int}", Name = "UpdatePutCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePutCategory(int categoryId, [FromBody] CategoryDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null || categoryId != request.Id)
            {
                return BadRequest(ModelState);
            }

            var exitsCategory = _categoryRepo.GetCategory(categoryId);
            if (exitsCategory == null)
            {
                return NotFound($"category {request.Name} not exits");
            }

            var category = _mapper.Map<Category>(request);

            if (!_categoryRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Error update the category {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepo.CategoryExits(categoryId))
            {
                return NotFound($"category not exits");
            }

            var category = _categoryRepo.GetCategory(categoryId);

            if (!_categoryRepo.DelteCategory(category))
            {
                ModelState.AddModelError("", $"Error delete the category {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}