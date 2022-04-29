using System;
using SocketIOSharp.Server;

namespace auto_battler_server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SocketIOServer server = new SocketIOServer(new SocketIOServerOption(3000));
            server.Start();
            
            Console.WriteLine("Server started on port 3000");

            server.OnConnection(socket =>
            {
                socket.On("joinRoom", (tokens =>
                {
                    var username = tokens[0].ToString();
                    var roomName = tokens[1].ToString();
                    
                    socket.Server.
                }))
            });
        }
    }
}