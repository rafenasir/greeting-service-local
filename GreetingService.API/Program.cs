using GreetingService.Core;
using GreetingService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGreetingRepository, FileGreetingRepository>(c =>
{
    var config = c.GetService<IConfiguration>();
    return new FileGreetingRepository(config["FileRepositoryFilePath"]);
});

builder.Services.AddScoped<IUserService, HardCodedUserService>();

var app = builder.Build();

var config = app.Configuration;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
