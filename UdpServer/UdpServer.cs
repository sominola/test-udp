using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpServer;

public class UdpServer
{
    private HashSet<int> Clients { get; } = new();

    private const int ServerPort = 5555;
    private const string ServerIp = "127.0.0.1";
    private UdpClient Receiver { get; }

    public UdpServer()
    {
        var ip = new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort);
        Receiver = new UdpClient(ip);
    }

    public async Task Start()
    {
        await Task.Run(() => ReceiveMessage().ConfigureAwait(false));
    }


    private async Task ReceiveMessage()
    {
        try
        {
            while (true)
            {
                var data = await Receiver.ReceiveAsync();
                var message = Encoding.UTF8.GetString(data.Buffer);

                var address = data.RemoteEndPoint.Address.ToString();
                var port = data.RemoteEndPoint.Port;

                foreach (var clientPort in Clients.Where(clientPort => clientPort != port))
                {
                    await Receiver.SendAsync(data.Buffer, data.Buffer.Length, address, clientPort);
                }

                if (!Clients.Contains(port))
                    Clients.Add(port);

                Console.WriteLine("{0}: {1}", $"{data.RemoteEndPoint.Address} {data.RemoteEndPoint.Port}", message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Receiver.Close();
        }
    }
}