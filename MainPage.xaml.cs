using AppExample.Models.Logging;


namespace AppExample
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            new LoggerInit();

            InitializeComponent();
            InitializeWebSocketAndUploadVideo();
            InitializeWebSocketAndPlayVideo();
        }
        private async void InitializeWebSocketAndPlayVideo()
        {
            await Task.Delay(1000);

            // Инициализация WebSocket для получения видеопотока

            Uri URI = new Uri("ws://127.0.0.1:8000/video/receive");

            VideoStreamingClient client = new VideoStreamingClient();
            await client.InitializeAndReceiveAsync(URI);

            // Полученные данные можно воспроизвести через MediaElement
        }
        private async void InitializeWebSocketAndUploadVideo()
        {
            // Инициализация WebSocket для отправки видеопотока

            Uri URI = new Uri("ws://127.0.0.1:8000/video/upload");
            string path_to_video = "E:\\Загрузки\\5-seconds-countdown.mp4";

            VideoStreamingClient client = new VideoStreamingClient();
            await client.InitializeAndUploadAsync(URI, path_to_video);

            // Полученные данные можно воспроизвести через MediaElement
        }


    }

}
