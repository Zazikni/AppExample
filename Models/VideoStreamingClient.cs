
using Serilog;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

class VideoStreamingClient
{
    private ClientWebSocket uploadSocket;
    private ClientWebSocket receiveSocket;
    private const int BufferSize = 4096; // Размер буфера для отправки видео
    public async Task DisconnectAsync(ClientWebSocket _webSocket)
    {
        if (_webSocket != null && _webSocket.State == WebSocketState.Open)
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }
    }

    public async Task InitializeAndUploadAsync(Uri uploadUri, string filePath)
    {
        uploadSocket = new ClientWebSocket();
        await uploadSocket.ConnectAsync(uploadUri, CancellationToken.None);


        // Отправка видеопотока на сервер
        using (var videoStream = File.OpenRead(filePath))
        {
            byte[] buffer = new byte[BufferSize];
            int bytesRead;

            while ((bytesRead = await videoStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                bool endOfMessage = (videoStream.Position == videoStream.Length);
                await uploadSocket.SendAsync(new ArraySegment<byte>(buffer, 0, bytesRead), WebSocketMessageType.Binary, endOfMessage, CancellationToken.None);
            }
        }
        await DisconnectAsync(uploadSocket);
        // Здесь должен быть код для чтения файла видео и отправки его содержимого
    }




public async Task InitializeAndReceiveAsync(Uri receiveUri)
    {
        receiveSocket = new ClientWebSocket();
        await receiveSocket.ConnectAsync(receiveUri, CancellationToken.None);
        string new_path = "E:\\Загрузки\\5-seconds-countdown_answer.mp4";

        // Получение видеопотока от сервера
        // Здесь должен быть код для получения видеопотока и его воспроизведение
        byte[] buffer = new byte[4096];
        ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
        WebSocketReceiveResult result;
        using (var fileStream = new FileStream(new_path, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            while(true) 
            {
                result = await receiveSocket.ReceiveAsync(segment, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await receiveSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Acknowledge Close frame",
                        CancellationToken.None);
                    break;
                }
                else if (result.MessageType == WebSocketMessageType.Binary)
                {
                    fileStream.Write(segment.Array, segment.Offset, result.Count);

                }
            } 

        }



        

    }
}
