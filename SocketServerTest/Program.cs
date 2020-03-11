using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace TCPServerTest
{
    class Program
    {
        private static List<ClientHandler> ClientSockets = new List<ClientHandler>();

        static void Main(string[] args)
        {
            int portNum = 9392;
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener listener = new TcpListener(ip, portNum);

            listener.Start();
            Console.WriteLine("Server is running listening for connections. ");


            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                var handler = new ClientHandler(client);

                lock (ClientSockets)
                {
                    Console.WriteLine("Connecting client "+ client.Client.RemoteEndPoint);
                    ClientSockets.Add(handler);
                    handler.Start();
                }
            }
        }
    }
}