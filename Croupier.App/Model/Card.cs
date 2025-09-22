namespace Croupier.Model;

public record Card
{
    public Card(string value, string suit)
    {
        Value = value;
        Suit = suit;
        Code = (value[0] != '1' ? value.Substring(0, 1) : value.Substring(0, 2))
            + suit.Substring(0, 1);
    }
    public string Code { get; set; }
    public string Value { get; set; }
    public string Suit { get; set; }
}
