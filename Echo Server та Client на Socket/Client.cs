using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

internal class Client
{
    static void Main(string[] args)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var endpoint = new IPEndPoint(IPAddress.Loopback, 7777);

        var receiveBuffer = new byte[1024];

        socket.Connect(endpoint);

        Console.WriteLine($"Підключено до сервера: {endpoint.Address}:{endpoint.Port}");
        Console.WriteLine($"Локальний endpoint: {socket.LocalEndPoint}");
        
        while (true)
        {
            Console.WriteLine("Введіть повідомлення (exit для виходу): ");
            string message = Console.ReadLine();

            var sendBuffer = Encoding.UTF8.GetBytes(message);

            socket.Send(sendBuffer);

            if (message == "exit")
                break;

            var received = socket.Receive(receiveBuffer);

            var receivedStr = Encoding.UTF8.GetString(receiveBuffer, 0, received);

            Console.WriteLine($"Echo: {receivedStr}");
        }

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}
