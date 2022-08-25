using Microsoft.EntityFrameworkCore;
using SQLFilter.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


string connstring = builder.Configuration.GetConnectionString("SqlWordsDatabase");
builder.Services.AddDbContext<SqlWordsContext>(options => options.UseSqlServer(connstring));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SensitiveWords API",
        Description = "An ASP.NET Core Web API to “bloop” out sensitive words of a company’s choice. This also provides basic CRUD operations for the sensitive words.",            
    });
});

var app = builder.Build();

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

