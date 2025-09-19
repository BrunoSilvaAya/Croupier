using FluentAssertions;
using Xunit;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Endpoints;

public class NewGameEndpointTests : BaseEndpointTest
{
    [Fact]
    public async Task NewGame_WhenCalled_ReturnsStringId()
    {
        // Act
        var result = await Client.GetStringAsync("/new-game");

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeOfType<string>();
    }

    [Fact]
    public async Task NewGame_WhenCalled_ReturnsIdWithCorrectLength()
    {
        // Act
        var id = await Client.GetStringAsync("/new-game");

        // Assert
        id.Length.Should().BeGreaterThan(5).And.BeLessThan(10);
    }
}