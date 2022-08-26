namespace Croupier.Model;

public record Deck
{
    public Deck(int? deckId)
    {
        DeckId = deckId ?? 1;
        Cards = new();
    }
    public Deck() : this((int?)null) { }
    public int DeckId { get; set; }
    public Stack<Card> Cards { get; set; }

}