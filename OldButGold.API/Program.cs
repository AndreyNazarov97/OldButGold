using OldButGold.API.DependencyInjection;
using OldButGold.API.Middleware;
using OldButGold.Domain.DependencyIncjection;
using OldButGold.Storage.DependencyIncjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApiLogging(builder.Configuration, builder.Environment);


builder.Services
    .AddForumDomain()
    .AddForumStorage(builder.Configuration.GetConnectionString("Postgres"));
builder.Services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();


public partial class Program { }