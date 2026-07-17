using TaskTracker.API.Repositories;
using TaskTracker.Dtos;
using TaskTracker.Entities;

namespace TaskTracker.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<TodoTask> _taskRepository;
        private readonly IRepository<Category> _categoryRepository;

        public TaskService(IRepository<TodoTask> taskRepository, IRepository<Category> categoryRepository)
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<TodoTaskDto>> GetAllTasksAsync(TaskQueryParams queryParams)
        {
            // filtr (eger varsa)
            System.Linq.Expressions.Expression<Func<TodoTask, bool>>? filter = null;
            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                filter = t => t.Title.Contains(queryParams.Search, StringComparison.OrdinalIgnoreCase) ||
                             t.Description.Contains(queryParams.Search, StringComparison.OrdinalIgnoreCase);
            }

            // sorting
            Func<IQueryable<TodoTask>, IQueryable<TodoTask>> orderByExpression = query =>
            {
                var isDesc = queryParams.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                return queryParams.SortBy?.ToLower() switch
                {
                    "title" => isDesc ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
                    "duedate" => isDesc ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
                    _ => isDesc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt) // Standart olaraq yaranma tarixinə görə
                };
            };

            // Repository-dən paged datanın çəkilməsi
            var tasks = await _taskRepository.GetAllPagedAsync(
                filter: filter,
                includeProperties: "Category",
                orderBy: orderByExpression,
                pageNumber: queryParams.PageNumber,
                pageSize: queryParams.PageSize
            );

            // DTO-ya çevrilmə
            return tasks.Select(t => new TodoTaskDto(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.CreatedAt,
                t.DueDate,
                t.CategoryId,
                t.Category?.Name ?? "Kateqoriyasız"
            ));
        }
        public async Task<TodoTaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetAsync(t => t.Id == id, includeProperties: "Category");
            if (task == null) return null;

            return new TodoTaskDto(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.CreatedAt,
                task.DueDate,
                task.CategoryId,
                task.Category?.Name ?? "Kateqoriyasız"
            );
        }

        public async Task<TodoTaskDto> CreateTaskAsync(CreateTodoTaskDto createTaskDto)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == createTaskDto.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Göstərilən CategoryId ({createTaskDto.CategoryId}) tapılmadı.");
            }

            var task = new TodoTask
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                CategoryId = createTaskDto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveAsync();

            return new TodoTaskDto(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.CreatedAt,
                task.DueDate,
                task.CategoryId,
                category.Name
            );
        }

        public async Task<bool> UpdateTaskAsync(int id, UpdateTodoTaskDto updateTaskDto)
        {
            var task = await _taskRepository.GetAsync(t => t.Id == id);
            if (task == null) return false;

            var category = await _categoryRepository.GetAsync(c => c.Id == updateTaskDto.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Göstərilən CategoryId ({updateTaskDto.CategoryId}) tapılmadı.");
            }

            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.IsCompleted = updateTaskDto.IsCompleted;
            task.DueDate = updateTaskDto.DueDate;
            task.CategoryId = updateTaskDto.CategoryId;

            _taskRepository.Update(task);
            await _taskRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetAsync(t => t.Id == id);
            if (task == null) return false;

            _taskRepository.Delete(task);
            await _taskRepository.SaveAsync();
            return true;
        }
    }
}