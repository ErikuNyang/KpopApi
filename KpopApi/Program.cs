using Microsoft.EntityFrameworkCore;
using KpopApi.Models;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options.AddPolicy("AllowAnything", policy => policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework stuff
builder.Services.AddDbContext<KpopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnything");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());  //Allow CORS
