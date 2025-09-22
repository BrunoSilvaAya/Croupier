namespace Croupier.Tests.TestModel;

public sealed class DeckBuilder
{
    private int _deckCount = 1;
    private bool _shuffled;

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

    public List<Card> Build()
    {
        // Create standard deck structure matching the API response
        var values = new[] { "ACE", "2", "3", "4", "5", "6", "7", "8", "9", "10", "JACK", "QUEEN", "KING" };
        var suits = new[] { "SPADES", "HEARTS", "CLUBS", "DIAMONDS" };

        var cards = new List<Card>();

        for (int deck = 0; deck < _deckCount; deck++)
        {
            foreach (var suit in suits)
            {
                foreach (var value in values)
                {
                    var code = (value[0] != '1' ? value.Substring(0, 1) : value.Substring(0, 2)) + suit.Substring(0, 1);
                    cards.Add(new Card { code = code, value = value, suit = suit });
                }
            }
        }

        if (_shuffled)
        {
            var random = new Random(42); // Fixed seed for deterministic tests
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }
        }

        return cards;
    }
}