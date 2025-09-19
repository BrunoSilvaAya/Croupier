using Croupier.Model;
using Croupier.Services;

namespace Croupier.Tests.TestModel;

public sealed class DeckBuilder
{
    private int _deckCount = 1;
    private bool _shuffled;
    private int? _deckId;

    public DeckBuilder WithDeckCount(int count) 
    { 
        _deckCount = count; 
        return this; 
    }

    public DeckBuilder Shuffled() 
    { 
        _shuffled = true; 
        return this; 
    }

    public DeckBuilder WithDeckId(int deckId)
    {
        _deckId = deckId;
        return this;
    }

    public Deck Build()
    {
        using var initializer = new Initializer();
        var deck = initializer.NewDeck(_deckCount);
        
        if (_deckId.HasValue)
        {
            deck.DeckId = _deckId.Value;
        }
        
        if (_shuffled)
        {
            deck.Cards = Workers.Shuffler.Shuffle(deck.Cards);
        }
        
        return deck;
    }
}