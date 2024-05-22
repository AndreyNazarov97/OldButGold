using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OldButGold.Search.API.Grpc;
using OldButGold.Search.ForumConsumer;
using OldButGold.Search.ForumConsumer.Monitoring;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiLogging(builder.Configuration, builder.Environment)
    .AddApiMetrics(builder.Configuration);

builder.Services.AddGrpcClient<SearchEngine.SearchEngineClient>(options =>
    options.Address = new Uri(builder.Configuration.GetConnectionString("SearchEngine")!));



builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection("Kafka").Bind);
builder.Services.AddSingleton(sp => new ConsumerBuilder<byte[], byte[]>(
    sp.GetRequiredService<IOptions<ConsumerConfig>>().Value).Build());

builder.Services.AddHostedService<ForumSearchConsumer>();

var app = builder.Build();


app.Run();
