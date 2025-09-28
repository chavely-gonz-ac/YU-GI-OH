using YuGiOh.Application;
using YuGiOh.Infrastructure;
// using YuGiOh.Infrastructure.Persistence;
// using YuGiOh.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();

// CORS (para React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins("http://localhost:3000") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

// using var scope = app.Services.CreateScope();
// var context = scope.ServiceProvider.GetRequiredService<YuGiOhDbContext>();

// await DatabaseSeeder.SeedYuGiOhArchetypesAsync(context);

app.Run();
