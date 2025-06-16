using Croupier.Model;
using Croupier.Workers;

namespace Croupier.Endpoints;

public class DeckEndpoint : IEndpoint    
{
    private ISessionManager _manager;
    public DeckEndpoint(IServiceProvider serviceProvider)
    {
        _manager = serviceProvider.GetService<ISessionManager>() ?? throw new ArgumentNullException(nameof(ISessionManager));
    }
    public void RegisterRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/new-game/{numberOfDecks?}", NewSession);
        app.MapGet("/draw-card/{sessionId}", DrawCard);
        app.MapGet("/see-deck/{sessionId}", SeeDeck);
        app.MapGet("/shuffle-deck/{sessionId}", ShuffleDeck);
    }

    //TODO: add some asynchronicity to the application
    public string NewSession(int? numberOfDecks)
    {
        return _manager.NewSession(numberOfDecks);
    }
    public Card? DrawCard(string sessionId)
    {
        return _manager.DrawCard(sessionId);
    }
    //TODO: make it route param and not querystring
    public Stack<Card>? SeeDeck(string sessionId)
    {
        return _manager.SeeDeck(sessionId);
    }
    public Stack<Card>? ShuffleDeck(string sessionId)
    {        
        return _manager.ShuffleDeck(sessionId);
    }

}
