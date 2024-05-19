using OldButGold.Search.Domain.DependencyInjection;
using OldButGold.Search.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSearchDomain()
    .AddSearchStorage(builder.Configuration.GetConnectionString("SearchIndex")!);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();

app.Run();
