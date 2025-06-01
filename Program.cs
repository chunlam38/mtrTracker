using MtrTracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Register MtrService
builder.Services.AddTransient<MtrService>();

// Register HTTP client + your service
builder.Services.AddHttpClient<IMtrService, MtrService>();
builder.Services.AddControllers();

// Add CORS policy to allow requests from your React frontend (localhost:3000)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000")  // Allow your frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Enable CORS middleware to apply the policy
app.UseCors("AllowLocalhost");

app.MapControllers();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<IMtrService>();

await app.RunAsync();