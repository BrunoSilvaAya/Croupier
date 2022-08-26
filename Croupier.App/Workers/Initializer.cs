using Croupier.Model;

namespace Croupier.Services;
public class Initializer : IDisposable
{
    public Initializer()
    {
        _values = new List<string>()
        {
            "ACE",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "JACK",
            "QUEEN",
            "KING"
        };

        _suits = new List<string>()
        {
            "SPADES",
            "HEARTS",
            "CLUBS",
            "DIAMONDS"
        };

    }
    private List<string> _values;
    private List<string> _suits;

    public Deck NewDeck(int numberOfDecks)
    {
        var d = new Deck();

        for (int i = 0; i < numberOfDecks; i++)
        {
            _suits.ForEach(s =>
            {
                _values.ForEach(v =>
                {
                    d.Cards.Push(new Card(v, s));
                });
            });
        }

        return d;
    }

    public void Dispose() { }
}
