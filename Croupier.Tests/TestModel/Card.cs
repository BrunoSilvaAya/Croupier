namespace Croupier.Tests;
public record Card
{
    public string? code { get; set; }
    public string? value { get; set; }
    public string? suit { get; set; }
}