namespace UdpServer;

public static class Program
{
    private static async Task Main()
    {
        var server = new UdpServer();
        await server.Start();
        Console.WriteLine("Server up");
        Console.ReadLine();
    }
}