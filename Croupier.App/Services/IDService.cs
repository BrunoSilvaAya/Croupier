namespace Croupier.Services;
public static class IDService
{
    private static Random _random = new Random();

    public static string NewId()
    {
        return _random.Next(int.MaxValue).ToString("x");
    }
}