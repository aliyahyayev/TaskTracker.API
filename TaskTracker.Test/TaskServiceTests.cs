using Moq;
using System.Linq.Expressions;
using TaskTracker.API.Repositories;
using TaskTracker.Dtos;
using TaskTracker.Entities;
using TaskTracker.Services;
using Xunit;

namespace TaskTracker.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<IRepository<TodoTask>> _mockTaskRepo;
        private readonly Mock<IRepository<Category>> _mockCategoryRepo;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            // Asılılıqları mock-layırıq (imitasiya edirik)
            _mockTaskRepo = new Mock<IRepository<TodoTask>>();
            _mockCategoryRepo = new Mock<IRepository<Category>>();

            // Mock obyektləri real servisimizə daxil edirik (Dependency Injection-ı simulyasiya edirik)
            _taskService = new TaskService(_mockTaskRepo.Object, _mockCategoryRepo.Object);
        }

        [Fact]
        public async Task GetTaskByIdAsync_WhenTaskExists_ReturnsTodoTaskDto()
        {
            // Arrange (Hazırlıq mərhələsi): Bazada var mış kimi davranacağımız saxta datanı hazırlayırıq
            int taskId = 1;
            var fakeTask = new TodoTask
            {
                Id = taskId,
                Title = "Test Tapşırığı",
                Description = "Açıqlama",
                CategoryId = 1,
                Category = new Category { Id = 1, Name = "İş" }
            };

            // burda ne cagirilsa mock obj qayidir
            _mockTaskRepo.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<TodoTask, bool>>>(),
                It.IsAny<string>()
            )).ReturnsAsync(fakeTask);

            // Act (İcra etmə mərhələsi): Real servisimizi çağırırıq
            var result = await _taskService.GetTaskByIdAsync(taskId);

            // serice mapping check
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal("Test Tapşırığı", result.Title);
            Assert.Equal("İş", result.CategoryName);
        }

        [Fact]
        public async Task GetTaskByIdAsync_WhenTaskDoesNotExist_ReturnsNull()
        {
            // only return null when the task does not exist
            _mockTaskRepo.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<TodoTask, bool>>>(),
                It.IsAny<string>()
            )).ReturnsAsync((TodoTask?)null);

            // Act
            var result = await _taskService.GetTaskByIdAsync(999);

            // Assert
            Assert.Null(result);
        }
    }
}