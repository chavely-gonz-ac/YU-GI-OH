using YuGiOh.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// 1. Add Services
// =====================================================

// Add Controllers + System.Text.Json options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (for React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins("http://localhost:3000") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddLogging();

// Infrastructure (Persistence + Identity)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// =====================================================
// 2. Configure Middleware
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use HTTPS redirection (recommended)
app.UseHttpsRedirection();

// Use CORS for the React frontend
app.UseCors("AllowReactApp");

// Authentication / Authorization
app.UseAuthentication(); // <— Needed if you use Identity
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// =====================================================
// 3. (Optional) Seed Database
// =====================================================
// using var scope = app.Services.CreateScope();
// var context = scope.ServiceProvider.GetRequiredService<YuGiOhDbContext>();
// await DatabaseSeeder.SeedYuGiOhArchetypesAsync(context);

app.Run();
