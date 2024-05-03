using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Domain.UseCases.GetForums;
using OldButGold.Storage;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Postgres");


builder.Services.AddScoped<IGetForumsUseCase, GetForumsUseCase>();
builder.Services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<ForumDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();




app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

//app.Services.GetRequiredService<ForumDbContext>().Database.Migrate();

app.Run();
