namespace Croupier.Tests.TestModel;

public sealed class CardBuilder
{
    private string _value = "ACE";
    private string _suit = "SPADES";

    public CardBuilder WithValue(string value)
    {
        _value = value;
        return this;
    }

    public CardBuilder WithSuit(string suit)
    {
        _suit = suit;
        return this;
    }

    public CardBuilder WithAce()
    {
        _value = "ACE";
        return this;
    }

    public CardBuilder WithKing()
    {
        _value = "KING";
        return this;
    }

    public CardBuilder WithQueen()
    {
        _value = "QUEEN";
        return this;
    }

    public CardBuilder WithJack()
    {
        _value = "JACK";
        return this;
    }

    public CardBuilder WithSpades()
    {
        _suit = "SPADES";
        return this;
    }

    public CardBuilder WithHearts()
    {
        _suit = "HEARTS";
        return this;
    }

    public CardBuilder WithClubs()
    {
        _suit = "CLUBS";
        return this;
    }

    public CardBuilder WithDiamonds()
    {
        _suit = "DIAMONDS";
        return this;
    }

    public Card Build()
    {
        var code = (_value[0] != '1' ? _value.Substring(0, 1) : _value.Substring(0, 2)) + _suit.Substring(0, 1);
        return new Card
        {
            code = code,
            value = _value,
            suit = _suit
        };
    }
}