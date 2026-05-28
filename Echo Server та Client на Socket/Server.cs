using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EchoChat;

internal class Program
{
    static void Main(string[] args)
    {
        var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var serverEndpoint = new IPEndPoint(IPAddress.Any, 7777);
        var buffer = new byte[1024];

        int messageCount = 0;
        int byteCount = 0;

        serverSocket.Bind(serverEndpoint);
        serverSocket.Listen(1);

        Console.WriteLine($"Echo Server запущено на {serverEndpoint.Address}:{serverEndpoint.Port}");
        Console.WriteLine("Очікування клієнта...");

        using var clientSocket = serverSocket.Accept();

        Console.WriteLine($"Клієнт підключився: {clientSocket.RemoteEndPoint}");

        while (true)
        {
            int bufferReceived = clientSocket.Receive(buffer);

            if (bufferReceived == 0)
            {
                Console.WriteLine("Клієнт відключився");
                break;
            }

            string message = Encoding.UTF8.GetString(buffer, 0, bufferReceived);

            if (message.Trim() == "exit")
            {
                Console.WriteLine("Клієнт відключився");
                break;
            }
            
            messageCount++;
            byteCount += bufferReceived;

            Console.WriteLine($"Отримано ({bufferReceived} байт): {message}");

            var messageByte = Encoding.UTF8.GetBytes(message);

            clientSocket.Send(messageByte);

            Console.WriteLine($"Відправлено echo: {message}");
        }
        
        Console.WriteLine($"Статистика: {messageCount} повідомлення, {byteCount} байт");

        try
        {
            clientSocket.Shutdown(SocketShutdown.Both);
        }
        catch { }
        clientSocket.Close();
    }
}
