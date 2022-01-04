using API.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Logger;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
//logger
builder.BuildLogger();

var app = builder.Build();
app.Logger.LogInformation("The application started");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();