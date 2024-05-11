using OldButGold.API.Authentication;
using OldButGold.API.Middleware;
using OldButGold.API.Monitoring;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.DependencyIncjection;
using OldButGold.Storage.DependencyIncjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApiLogging(builder.Configuration, builder.Environment);
builder.Services.AddApiMetrics();
builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication").Bind);
builder.Services.AddScoped<IAuthTokenStorage, AuthTokenStorage>();


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
app.MapPrometheusScrapingEndpoint();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();


public partial class Program { }