using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 9392;
            string ip = "localhost";
            TcpClient client = new TcpClient(ip, port);
            Console.WriteLine("Is the client connected? "+ (client.Connected ? "yes": "no"));

            bool connected = client.Client.Connected;
            int loops = 0;
            while (connected)
            {
                try
                {
                    client.GetStream().Write(new byte[1], 0, 1);
                }
                catch (IOException)
                {
                    connected = false;
                }

                string msg = Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes(msg);

                client.GetStream().Write(buffer, 0, msg.Length);
            }
            client.Close();

            Console.WriteLine("Disconnecting. ");
            Console.Read();
        }
    }
}
