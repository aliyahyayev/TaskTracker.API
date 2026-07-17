using Microsoft.EntityFrameworkCore;
using TaskTracker.API.Data;
using TaskTracker.API.Repositories;
using TaskTracker.Exceptions;
using TaskTracker.Services;

var builder = WebApplication.CreateBuilder(args);


// Repository qeydiyyatı (Generic Repository üçün scoped istifadə olunur)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Service qeydiyyatı
builder.Services.AddScoped<ITaskService, TaskService>();
// DbContext-i PostgreSQL ilə qeydiyyatdan keçiririk
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
var app = builder.Build();
// Qlobal xəta idarəetmə middleware-i (Hər şeydən əvvəl dayanmalıdır)
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
