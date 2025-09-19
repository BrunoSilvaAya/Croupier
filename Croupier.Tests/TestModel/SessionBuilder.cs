using Croupier.Model;

namespace Croupier.Tests.TestModel;

public sealed class SessionBuilder
{
    private int _numberOfDecks = 1;
    private string? _sessionId;

    public SessionBuilder WithNumberOfDecks(int numberOfDecks)
    {
        _numberOfDecks = numberOfDecks;
        return this;
    }

    public SessionBuilder WithSessionId(string sessionId)
    {
        _sessionId = sessionId;
        return this;
    }

    public Session Build()
    {
        var session = new Session(_numberOfDecks);
        
        if (_sessionId != null)
        {
            session.SessionId = _sessionId;
        }
        
        return session;
    }
}