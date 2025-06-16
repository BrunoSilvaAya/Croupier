using Croupier.Model;

namespace Croupier.Workers;

public class SessionManager : ISessionManager
{
    private readonly List<Session> _sessions;
    public SessionManager()
    {
        _sessions = new();
    }
    // Extracted helper to find a session by ID
    private Session? FindSession(string sessionId) => _sessions.FirstOrDefault(s => s.SessionId == sessionId);

    public string NewSession(int? numberOfDecks)
    {
        _sessions.Add(new Session(numberOfDecks ?? 1));
        return _sessions.Last().SessionId;
    }

    public Session? GetSession(string sessionId)
    {
        return FindSession(sessionId);
    }

    public Card? DrawCard(string sessionId)
    {
        var session = FindSession(sessionId);
        if (session == null)
        {
            return null;
        }
        var cards = session.Cards.Cards;
        if (cards.Count == 0)
        {
            return null;
        }
        return cards.Pop();
    }

    public Stack<Card>? SeeDeck(string sessionId)
    {
        return FindSession(sessionId)?.Cards.Cards;
    }

    public Stack<Card>? ShuffleDeck(string sessionId)
    {
        var deck = FindSession(sessionId)?.Cards;

        if (deck != null)
        {
            deck.Cards = Shuffler.Shuffle(deck.Cards);
            return deck.Cards;
        }
        return null;        
    }
}
