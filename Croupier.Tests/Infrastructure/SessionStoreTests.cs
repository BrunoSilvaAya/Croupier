using FluentAssertions;
using Xunit;
using Croupier.Workers;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Infrastructure;

public class SessionStoreTests
{
    private readonly SessionManager _sessionManager;

    public SessionStoreTests()
    {
        _sessionManager = new SessionManager();
    }

    [Fact]
    public void NewSession_WhenCalled_ReturnsValidSessionId()
    {
        // Act
        var sessionId = _sessionManager.NewSession(1);

        // Assert
        sessionId.Should().NotBeNullOrEmpty();
        sessionId.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetSession_WithValidSessionId_ReturnsCorrectSession()
    {
        // Arrange
        var sessionId = _sessionManager.NewSession(1);

        // Act
        var session = _sessionManager.GetSession(sessionId);

        // Assert
        session.Should().NotBeNull();
        session!.SessionId.Should().Be(sessionId);
        session.Cards.Cards.Should().HaveCount(52);
    }

    [Fact]
    public void GetSession_WithInvalidSessionId_ReturnsNull()
    {
        // Act
        var session = _sessionManager.GetSession("invalid-session-id");

        // Assert
        session.Should().BeNull();
    }

    [Fact]
    public void DrawCard_WithValidSession_ReturnsCardAndUpdatesCount()
    {
        // Arrange
        var sessionId = _sessionManager.NewSession(1);
        var originalCount = _sessionManager.SeeDeck(sessionId)?.Count ?? 0;

        // Act
        var drawnCard = _sessionManager.DrawCard(sessionId);
        var newCount = _sessionManager.SeeDeck(sessionId)?.Count ?? 0;

        // Assert
        drawnCard.Should().NotBeNull();
        newCount.Should().Be(originalCount - 1);
    }

    [Fact]
    public void ShuffleDeck_WithValidSession_MaintainsCardCount()
    {
        // Arrange
        var sessionId = _sessionManager.NewSession(2);
        var originalDeck = _sessionManager.SeeDeck(sessionId)?.ToList();

        // Act
        var shuffledDeck = _sessionManager.ShuffleDeck(sessionId)?.ToList();

        // Assert
        originalDeck.Should().NotBeNull();
        shuffledDeck.Should().NotBeNull();
        shuffledDeck.Should().HaveCount(originalDeck!.Count);
        shuffledDeck.Should().NotEqual(originalDeck);
    }
}