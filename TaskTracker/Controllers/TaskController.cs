using Microsoft.AspNetCore.Mvc;
using TaskTracker.Dtos;
using TaskTracker.Services;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/tasks?search=kod&sortBy=title&sortOrder=desc&pageNumber=1&pageSize=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoTaskDto>>> GetAll([FromQuery] TaskQueryParams queryParams)
        {
            var tasks = await _taskService.GetAllTasksAsync(queryParams);
            return Ok(tasks);
        }

        // GET: api/tasks/5 (ID-yə görə tək task gətirmək)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoTaskDto>> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound(new { Message = $"ID-si {id} olan tapşırıq tapılmadı." }); 
            }
            return Ok(task); // 200 OK
        }

        // POST: api/tasks (Yeni task yaratmaq)
        [HttpPost]
        public async Task<ActionResult<TodoTaskDto>> Create([FromBody] CreateTodoTaskDto createTaskDto)
        {
            // Input Validation avtomatik olaraq [ApiController] tərəfindən idarə olunur (400 Bad Request)
            try
            {
                var createdTask = await _taskService.CreateTaskAsync(createTaskDto);

                // Yaradılan resursun linki ilə birlikdə geri qaytarırıq (201 Created)
                return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { Message = ex.Message }); // Əgər göndərilən CategoryId səhvdirsə
            }
        }

        // PUT: api/tasks/5 (Taskı yeniləmək)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoTaskDto updateTaskDto)
        {
            try
            {
                var updated = await _taskService.UpdateTaskAsync(id, updateTaskDto);
                if (!updated)
                {
                    return NotFound(new { Message = $"ID-si {id} olan tapşırıq tapılmadı." }); // 404 Not Found
                }
                return NoContent(); // 204 No Content (Yenilənmə uğurludur, geriyə data qaytarmağa ehtiyac yoxdur)
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/tasks/5 (Taskı silmək)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id); //true qayidir silse
            if (!deleted)
            {
                return NotFound(new { Message = $"ID-si {id} olan tapşırıq tapılmadı." });
            }
            return NoContent(); 
        }
    }
}