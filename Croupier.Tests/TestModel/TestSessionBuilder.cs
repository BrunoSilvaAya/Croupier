using Microsoft.AspNetCore.Mvc.Testing;

namespace Croupier.Tests.TestModel;

public sealed class TestSessionBuilder
{
    private WebApplicationFactory<Program>? _app;
    private HttpClient? _client;
    private int _numberOfDecks = 1;

    public TestSessionBuilder WithNumberOfDecks(int numberOfDecks)
    {
        _numberOfDecks = numberOfDecks;
        return this;
    }

    public TestSessionBuilder WithHttpClient(HttpClient client)
    {
        _client = client;
        return this;
    }

    public TestSessionBuilder WithWebApp(WebApplicationFactory<Program> app)
    {
        _app = app;
        _client = app.CreateClient();
        return this;
    }

    public async Task<string> BuildAsync()
    {
        if (_client == null)
        {
            if (_app == null)
            {
                _app = new WebApplicationFactory<Program>();
            }
            _client = _app.CreateClient();
        }

        var endpoint = _numberOfDecks == 1
            ? "/new-game"
            : $"/new-game?numberOfDecks={_numberOfDecks}";

        return await _client.GetStringAsync(endpoint);
    }
}