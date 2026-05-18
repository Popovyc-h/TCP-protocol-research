using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class SimpleSocketClient
{
    static void Main()
    {
        // 1. Створити Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        var clientSocket = new Socket
            (
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
            );

        // 2. Створити IPEndPoint для сервера (IPAddress.Loopback, 5000)
        var endpoint = new IPEndPoint
            (
            IPAddress.Loopback,
            5000
            );

        // 3. З'єднатися з сервером (Connect)
        clientSocket.Connect(endpoint);
        Console.WriteLine($"Підключення до сервера: {clientSocket.RemoteEndPoint}");
        Console.WriteLine("Підключено!");

        // 4. Підготувати повідомлення та конвертувати в байти
        string message = "Привіт, сервер!";
        var sendBuffer = Encoding.UTF8.GetBytes(message);

        // 5. Відправити дані (Send)
        clientSocket.Send(sendBuffer);
        Console.WriteLine($"Відправлено: {message}");

        // 6. Отримати відповідь у буфер (Receive)
        byte[] receiveBuffer = new byte[1024];
        var received = clientSocket.Receive(receiveBuffer);

        // 7. Декодувати відповідь з байтів у рядок
        string receivedStr = Encoding.UTF8.GetString(receiveBuffer, 0, received);
        Console.WriteLine($"Отримано відповідь: {receivedStr}");

        // 8. Закрити з'єднання (Shutdown, Close)
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
        Console.WriteLine("З'єднання закрито");
    }
}
