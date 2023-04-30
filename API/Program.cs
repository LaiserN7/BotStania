using API.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Logger;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers().AddNewtonsoftJson();
//logger
builder.BuildLogger();

var app = builder.Build();
app.Logger.LogInformation("The application started");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();


app.UseAuthorization();

app.MapGet("/", () => "Hello from BOT!");

app.MapControllers();

app.Run();