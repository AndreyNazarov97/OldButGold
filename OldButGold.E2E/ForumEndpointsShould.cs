using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using OldButGold.Forums.API.Models;
using System.Net.Http.Json;
using OldButGold.Forums.API;

namespace OldButGold.Forums.E2E
{
    public class ForumEndpointsShould(ForumApiApplicationFactory factory) : IClassFixture<ForumApiApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> factory = factory;

        //[Fact]
        public async Task CreateNewForum()
        {
            const string forumTitle = "284bbc97-47f6-4adb-b99b-20916fa0a6e0";

            using var httpClient = factory.CreateClient();
            using var getInitialForumsResponse = await httpClient.GetAsync("forums");
            var initialForums = await getInitialForumsResponse.Content.ReadFromJsonAsync<Forum[]>();
            initialForums
                .Should().NotBeNull().And
                .Subject.As<Forum[]>().Should().NotContain(f => f.Title.Equals(forumTitle));

            using var response = await httpClient.PostAsync("forums",
                JsonContent.Create(new { title = forumTitle }));


            response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
            var forum = await response.Content.ReadFromJsonAsync<Forum>();
            forum
                .Should().NotBeNull().And
                .Subject.As<Forum>().Title.Should().Be(forumTitle);
            forum?.Id.Should().NotBeEmpty();


            using var getForumsResponse = await httpClient.GetAsync("forums");
            var forums = await getForumsResponse.Content.ReadFromJsonAsync<Forum[]>();
            forums
                .Should().NotBeNull().And
                .Subject.As<Forum[]>().Should().Contain(f => f.Title.Equals(forumTitle));
        }


    }
}
