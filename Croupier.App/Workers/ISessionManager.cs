using Croupier.Model;

namespace Croupier.Workers;

public interface ISessionManager
{
    public string NewSession(int? numberOfDecks);
    public Card? DrawCard(string sessionId);
    public Stack<Card>? SeeDeck(string sessionId);
    public Stack<Card>? ShuffleDeck(string sessionId);
}
