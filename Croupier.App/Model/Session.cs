using Croupier.Services;

namespace Croupier.Model;
public record Session
{
    public Session(int numberOfDecks)
    {
        Cards = NewDeck(numberOfDecks);
        SessionId = IDService.NewId();
    }
    public string SessionId { get; set; }
    //TODO: there's really no reason for this to be a list. Even with multiple decks,
    //TODO: conceptually it's a single pile of cards
    public Deck Cards { get; set; }

    private Deck NewDeck(int NumberOfDecks)
    {
        using var initializer = new Initializer();
        return initializer.NewDeck(NumberOfDecks);
    }
}
