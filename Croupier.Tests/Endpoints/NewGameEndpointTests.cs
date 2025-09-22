using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Endpoints;

public class NewGameEndpointTests
{
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _client;

    public NewGameEndpointTests()
    {
        _app = new WebApplicationFactory<Program>();
        _client = _app.CreateClient();
    }

    [Fact]
    public async Task NewGame_WhenCalled_ReturnsStringId()
    {
        var result = await _client.GetStringAsync("/new-game");

        result.Should().BeOfType<string>();
    }

    [Fact]
    public async Task NewGame_WhenCalled_ReturnsIdWithValidLength()
    {
        var sessionId = await _client.GetStringAsync("/new-game");

        sessionId.Length.Should().BeGreaterThan(5).And.BeLessThan(10);
    }

    [Fact]
    public async Task NewGame_WithMultipleDecks_CreatesValidSession()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .WithNumberOfDecks(3)
            .BuildAsync();

        sessionId.Should().NotBeNullOrEmpty();
        sessionId.Length.Should().BeGreaterThan(5).And.BeLessThan(10);
    }
}