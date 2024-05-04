using Microsoft.EntityFrameworkCore;
using OldButGold.Domain;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Domain.UseCases.GetForums;
using OldButGold.Storage;
using OldButGold.Storage.Storages;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Postgres");


builder.Services.AddScoped<IGetForumsUseCase, GetForumsUseCase>();
builder.Services.AddScoped<IGetForumsStorage, GetForumStorage>();
builder.Services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();
builder.Services.AddScoped<ICreateTopicStorage, CreateTopicStorage>();
builder.Services.AddScoped<IGuidFactory, GuidFactory>();
builder.Services.AddScoped<IMomentProvider, MomentProvider>();
builder.Services.AddScoped<IIntentionResolver, TopicIntentionResolver>();
builder.Services.AddScoped<IIntentionManager, IntentionManager>();
builder.Services.AddScoped<IIdentityProvider, IdentityProvider>();


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
