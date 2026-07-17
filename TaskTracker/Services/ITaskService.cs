using TaskTracker.Dtos;

namespace TaskTracker.Services
{
    public interface ITaskService
    {
        //task asinxron yerine yetirmesi ucundu
        Task<IEnumerable<TodoTaskDto>> GetAllTasksAsync(TaskQueryParams queryParams); 
        Task<TodoTaskDto?> GetTaskByIdAsync(int id);
        Task<TodoTaskDto> CreateTaskAsync(CreateTodoTaskDto createTaskDto);
        Task<bool> UpdateTaskAsync(int id, UpdateTodoTaskDto updateTaskDto);
        Task<bool> DeleteTaskAsync(int id);
    }
}