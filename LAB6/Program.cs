using Lab6.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// Получаем строку подключения
var connectionString = builder.Configuration.GetConnectionString("DBConnection")
    ?? throw new InvalidOperationException("Connection string 'DBConnection' not found.");

// Добавляем DbContext для работы с базой данных
builder.Services.AddDbContext<HairdressingContext>(options =>
    options.UseSqlServer(connectionString));

// Добавляем сервисы для работы с контроллерами и представлениями
builder.Services.AddControllersWithViews();

// Настройка Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); // Включение аннотаций для Swagger
});

var app = builder.Build();

// Конфигурация HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Включаем Swagger middleware
    app.UseSwagger();

    // Включаем Swagger UI middleware
    app.UseSwaggerUI();
}

app.UseAuthorization();

// Маршруты для контроллеров
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PerformedServices}/{action=index}/{id?}");

app.Run();
