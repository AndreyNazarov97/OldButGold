using Confluent.Kafka;
using OldButGold.Forums.Domain.DependencyInjection;
using OldButGold.Forums.API;
using OldButGold.Forums.API.Authentication;
using OldButGold.Forums.API.Middleware;
using OldButGold.Forums.API.Monitoring;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Storage.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApiLogging(builder.Configuration, builder.Environment)
    .AddApiMetrics(builder.Configuration);

builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication").Bind);
builder.Services.AddScoped<IAuthTokenStorage, AuthTokenStorage>();


builder.Services
    .AddForumDomain()
    .AddForumStorage(builder.Configuration.GetConnectionString("Postgres")!);
builder.Services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton(new ConsumerBuilder<byte[], byte[]>(new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    EnableAutoCommit = false,
    EnablePartitionEof = true,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    GroupId = "obg.experiment"
}).Build() );

builder.Services.AddHostedService<KafkaConsumer>();
builder.Services.Configure<HostOptions>(options =>
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapPrometheusScrapingEndpoint();

app
    .UseMiddleware<ErrorHandlingMiddleware>()
    .UseMiddleware<AuthenticationMiddleware>();

app.Run();


public partial class Program { }