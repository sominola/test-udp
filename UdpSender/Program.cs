namespace UdpSender;


internal static class Program
{
    private static async Task Main()
    {
        var sender = new UdpSender();
        await sender.StartListen();
        await sender.SendMessage();
    }
}