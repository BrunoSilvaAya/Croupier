using Croupier.Model;
using Croupier.Workers;

namespace Croupier.Endpoints;

public class DeckEndpoint : IEndpoint    
{    private readonly ISessionManager _manager;
    public DeckEndpoint(IServiceProvider serviceProvider)
    {
        _manager = serviceProvider.GetService<ISessionManager>() ?? throw new ArgumentNullException(nameof(serviceProvider), "ISessionManager not registered");
    }public void RegisterRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/new-game/{numberOfDecks:int?}", NewSession);
        app.MapGet("/sessions/{sessionId}/draw-card", DrawCard);
        app.MapGet("/sessions/{sessionId}/see-deck", SeeDeck);
        app.MapPost("/sessions/{sessionId}/shuffle-deck", ShuffleDeck);
    }

    //TODO: add some asynchronicity to the application
    public string NewSession(int? numberOfDecks)
    {
        return _manager.NewSession(numberOfDecks);
    }
    public Card? DrawCard(string sessionId)
    {
        return _manager.DrawCard(sessionId);
    }    public Stack<Card>? SeeDeck(string sessionId)
    {
        return _manager.SeeDeck(sessionId);
    }
    public Stack<Card>? ShuffleDeck(string sessionId)
    {        
        return _manager.ShuffleDeck(sessionId);
    }

}
