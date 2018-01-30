using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Sockets;


namespace Xamarin2018.SignalRTest
{
    class Program
    {
        private static HubConnection _connection;

        static void Main(string[] args)
        {
            Task.Run(Run).Wait();
        }


        static async Task Run()
        {
            await StartConnectionAsync();
            _connection.On<string>("Color", (color) =>
            {
                Console.WriteLine($"Цвет: {color}");
            });
            Console.ReadLine();
            await DisposeAsync();
        }


        public static async Task StartConnectionAsync()
        {
            _connection =  new HubConnectionBuilder()
                .WithUrl("https://xamarinday2018azure.azurewebsites.net/hub")
                .WithTransport(TransportType.LongPolling)
                .WithConsoleLogger()
                .Build();

            await _connection.StartAsync();
        }

        public static async Task DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}
