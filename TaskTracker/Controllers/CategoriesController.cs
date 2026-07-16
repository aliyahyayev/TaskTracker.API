using Microsoft.AspNetCore.Mvc;
using TaskTracker.API.Repositories;
using TaskTracker.Dtos;
using TaskTracker.Entities;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoriesController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var dtos = categories.Select(c => new CategoryDto(c.Id, c.Name));
            return Ok(dtos);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto createDto)
        {
            var category = new Category
            {
                Name = createDto.Name
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();

            var dto = new CategoryDto(category.Id, category.Name);
            return CreatedAtAction(nameof(GetAll), dto);
        }
    }
}