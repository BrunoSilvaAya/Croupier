using Croupier.Model;

namespace Croupier.Workers;

public class SessionManager : ISessionManager
{
    private readonly List<Session> _sessions;
    public SessionManager()
    {
        _sessions = new();
    }    public string NewSession(int? numberOfDecks)
    {
        _sessions.Add(new Session(numberOfDecks ?? 1));
        return _sessions[_sessions.Count - 1].SessionId;
    }
    public Session? GetSession(string sessionId)
    {
        return _sessions.FirstOrDefault(s => s.SessionId == sessionId);
    }    public Card? DrawCard(string sessionId)
    {
        var session = _sessions.FirstOrDefault(s => s.SessionId == sessionId);
        if (session?.Cards.Cards.Count > 0)
        {
            return session.Cards.Cards.Pop();
        }
        return null;
    }
    public Stack<Card>? SeeDeck(string sessionId)
    {
        return _sessions.FirstOrDefault(s => s.SessionId == sessionId)?
                .Cards.Cards;
    }
    public Stack<Card>? ShuffleDeck(string sessionId)
    {
        var deck = _sessions.FirstOrDefault(s => s.SessionId == sessionId)?
                    .Cards;

        if (deck != null)
        {
            deck.Cards = Shuffler.Shuffle(deck.Cards);
            return deck.Cards;
        }
        return null;        
    }
}
