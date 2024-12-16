using Lab6.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// �������� ������ �����������
var connectionString = builder.Configuration.GetConnectionString("DBConnection")
    ?? throw new InvalidOperationException("Connection string 'DBConnection' not found.");

// ��������� DbContext ��� ������ � ����� ������
builder.Services.AddDbContext<HairdressingContext>(options =>
    options.UseSqlServer(connectionString));

// ��������� ������� ��� ������ � ������������� � ���������������
builder.Services.AddControllersWithViews();

// ��������� Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); // ��������� ��������� ��� Swagger
});

var app = builder.Build();

// ������������ HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // �������� Swagger middleware
    app.UseSwagger();

    // �������� Swagger UI middleware
    app.UseSwaggerUI();
}

app.UseAuthorization();

// �������� ��� ������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PerformedServices}/{action=index}/{id?}");

app.Run();
