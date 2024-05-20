using OldButGold.Search.API.Controllers;
using OldButGold.Search.API.Monitoring;
using OldButGold.Search.Domain.DependencyInjection;
using OldButGold.Search.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiLogging(builder.Configuration, builder.Environment)
    .AddApiMetrics(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSearchDomain()
    .AddSearchStorage(builder.Configuration.GetConnectionString("SearchIndex")!);

builder.Services.AddGrpcReflection().AddGrpc();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();

app.MapGrpcService<SearchEngineGrpcService>();
app.MapGrpcReflectionService();

app.Run();
