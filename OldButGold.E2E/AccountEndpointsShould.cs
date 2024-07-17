using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OldButGold.Forums.API.Models;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Storage;
using System.Net.Http.Json;
using System.Text.Json;
using OldButGold.Forums.API.Models.Topics;

namespace OldButGold.Forums.E2E
{
    public class AccountEndpointsShould(
        ForumApiApplicationFactory factory) : IClassFixture<ForumApiApplicationFactory>
    {
        [Fact]
        public async Task SignInAfterSignOn()
        {
            using var httpClient = factory.CreateClient();

            using var signOnResponse = await httpClient.PostAsync(
                "account", JsonContent.Create(new { login = "Test", password = "qwerty" }));
            signOnResponse.IsSuccessStatusCode.Should().BeTrue();
            var createdUser = await signOnResponse.Content.ReadFromJsonAsync<User>();

            using var signInResponse = await httpClient.PostAsync(
                "account/signin", JsonContent.Create(new { login = "Test", password = "qwerty" }));
            signInResponse.IsSuccessStatusCode.Should().BeTrue();

            var signedInUser = await signInResponse.Content.ReadFromJsonAsync<User>();
            signedInUser!.UserId.Should().Be(createdUser!.UserId);

            var createForumResponse = await httpClient.PostAsync(
                    "forums", JsonContent.Create(new { title = "Test title" }));
            createForumResponse.IsSuccessStatusCode.Should().BeTrue();

            var createdForum = (await createForumResponse.Content.ReadFromJsonAsync<API.Models.Forum>())!;

            const string testTitle = "New Topic";
            var createTopicResponse = await httpClient.PostAsync(
                $"forums/{createdForum.Id}/topics",
                JsonContent.Create(new { title = testTitle }));
            createTopicResponse.IsSuccessStatusCode.Should().BeTrue();

            await using var scope = factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
            var domainEvents = await dbContext.DomainEvents.ToArrayAsync();
            domainEvents.Should().HaveCount(1);
            var topic = JsonSerializer.Deserialize<Topic>(domainEvents[0].ContentBlob);
            topic!.Title.Should().Be(testTitle);
        }
    }
}
