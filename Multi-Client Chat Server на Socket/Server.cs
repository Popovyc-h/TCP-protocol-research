using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class Program
{
    private static List<ClientInfo> clientInfos = new();

    private static void HandleClient(Socket client)
    {
        Console.WriteLine($"Клієнт підключився: {client.RemoteEndPoint}");
        
        var buffer = new byte[1024];
        int bytesReceived = client.Receive(buffer);

        string name = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

        Console.WriteLine($"Користувач {name} приєднався до чату");

        var newClient = new ClientInfo
        {
            Socket = client,
            RemoteEndPoint = (IPEndPoint)client.RemoteEndPoint,
            Username = name
        };

        clientInfos.Add(newClient);
        Console.WriteLine($"Активних клієнтів: {clientInfos.Count}");

        Broadcast($"[{name} приєднався до чату]");

        try
        {
            while (true)
            {
                int receive = client.Receive(buffer);
                string str = Encoding.UTF8.GetString(buffer, 0, receive);

                Console.WriteLine($"[{name}]: {str}");

                Broadcast($"[{name}]: {str}");
            }
        }
        catch(Exception ex) 
        {
            Console.WriteLine($"Клієнт {newClient.Username} відключився");
            clientInfos.Remove(newClient);
        }
    }

    private static void Broadcast(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);

        foreach (var cl in clientInfos)
            cl.Socket.Send(messageBytes);
    }


    static void Main(string[] args)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var endpoint = new IPEndPoint(IPAddress.Any, 8888);

        socket.Bind(endpoint);
        socket.Listen();

        Console.WriteLine($"Chat Server запущено на {endpoint.Address}:{endpoint.Port}");
        Console.WriteLine("Очікування клієнтів...");

        while (true)
        {
            var client = socket.Accept();

            var thread = new Thread(() => HandleClient(client));
            thread.Start();
        }
    }
}
