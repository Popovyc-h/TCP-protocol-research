using System.Net;
using System.Net.Sockets;
using System.Text;

class SimpleSocketServer
{
    static void Main()
    {
        // 1. Створити Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        var serverSocket = new Socket
            (
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
            );

        // 2. Створити IPEndPoint (IPAddress.Any, 5000)
        var endpoint = new IPEndPoint
            (
            IPAddress.Any,
            5000
            );

        // 3. Прив'язати сокет до endpoint (Bind)
        serverSocket.Bind(endpoint);

        // 4. Почати слухати з чергою 10 (Listen)
        serverSocket.Listen(10);
        Console.WriteLine("Сервер запущено на порту 5000...");

        Console.WriteLine("Очікування клієнта...");

        // 5. Прийняти клієнта (Accept) - блокуючий виклик
        using var clientSocket = serverSocket.Accept();
        Console.WriteLine($"Клієнт підключився: {clientSocket.RemoteEndPoint}");
        
        // 6. Отримати дані в буфер (Receive)
        var buffer = new byte[1024];
        int bytesReceived = clientSocket.Receive(buffer);

        // 7. Декодувати дані з байтів у рядок
        string received = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

        Console.WriteLine($"Отримано ({bytesReceived} байт): {received}");

        // 8. Відправити відповідь (Send)
        string message = "Hello!";
        byte[] responseBytes = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(responseBytes);

        // 9. Закрити з'єднання (Shutdown, Close)
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
        Console.WriteLine("З'єднання закрито");
    }
}
