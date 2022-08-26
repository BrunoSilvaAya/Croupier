using Croupier.Model;

namespace Croupier.Workers;

public static class Shuffler
{
    private static Random random = new Random();
    public static Stack<Card> Shuffle (Stack<Card> cards)
    {
        var list = cards.ToList();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }

        var shuffledCards = new Stack<Card>();
        list.ForEach(c => shuffledCards.Push(c));
        return shuffledCards;
    }
}
