using Microsoft.EntityFrameworkCore;
using KpopApi.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using KpopApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options.AddPolicy("AllowAnything", policy => policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
c.SwaggerDoc("v1", new OpenApiInfo
{
    Contact = new OpenApiContact
    {
        Email = "bumjun.cho.314@csun.edu",
        Name = "Bum Jun Cho",
        Url = new Uri("https://www.linkedin.com/in/bum-jun-cho-866319145")
    },
    Description = "APIs for Kpop Lists",
    Title = "Kpop List APIs",
    Version = "V1"
});
OpenApiSecurityScheme jwtSecurityScheme = new()
{
    Scheme = "bearer",
    BearerFormat = "JWT",
    Name = "JWT Authentication",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Description = "Please enter only JWT token",
    Reference = new OpenApiReference
    {
        Id = JwtBearerDefaults.AuthenticationScheme,
        Type = ReferenceType.SecurityScheme
    }
};
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});               // this enabled Swagger

// Entity Framework stuff
builder.Services.AddDbContext<KpopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<KpopUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;        // isn't it too short?
})
    .AddEntityFrameworkStores<KpopContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        RequireExpirationTime = true,
        ValidateIssuer = true,                  // you can omit the issue, audience, lifetime.
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"]!))
    };
});

builder.Services.AddScoped<JwtHandler>();       // Generation of a Token

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnything");
app.UseHttpsRedirection();

app.UseAuthentication();            // add authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());  //Allow CORS
