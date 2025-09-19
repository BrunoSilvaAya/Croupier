using Croupier.Model;

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

    public CardBuilder AsAceOfSpades()
    {
        return WithValue("ACE").WithSuit("SPADES");
    }

    public CardBuilder AsKingOfHearts()
    {
        return WithValue("KING").WithSuit("HEARTS");
    }

    public Croupier.Model.Card Build()
    {
        return new Croupier.Model.Card(_value, _suit);
    }
}