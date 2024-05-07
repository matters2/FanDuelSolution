using FanDuelSolution.Application;
using FanDuelSolution.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FanDuelSolution-Matters2 API", Version = "v1" });
});

var app = builder.Build();

if (configuration["EnableDebugTools"] == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FanDuelSolution-Matters2 API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();