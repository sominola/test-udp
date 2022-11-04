using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpSender;

public class UdpSender
{
    private const int ServerPort = 5555;
    private const string ServeIp = "127.0.0.1";

    private UdpClient Sender { get; }

    public UdpSender()
    {
        var rnd = new Random();
        var port = rnd.Next(1, 6000);
        Console.WriteLine(port);
        var ip = new IPEndPoint(IPAddress.Parse(ServeIp),port);
        Sender = new UdpClient(ip);
    }

    public async Task StartListen()
    {
        await Task.Run(() => Receive().ConfigureAwait(false));
    }

    private async Task Receive()
    {
        try
        {
            while (true)
            {
                var data = await Sender.ReceiveAsync();
                var message = Encoding.UTF8.GetString(data.Buffer);
                Console.WriteLine("{0}: {1}",$"{data.RemoteEndPoint.Address} {data.RemoteEndPoint.Port}", message);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Sender.Close();
        }
    }

    public async Task SendMessage()
    {
        try
        {
            while (true)
            {
                var message = $"{Console.ReadLine()}";
                var data = Encoding.UTF8.GetBytes(message);
                await Sender.SendAsync(data, data.Length, ServeIp, ServerPort);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Sender.Close();
        }
    }
}
