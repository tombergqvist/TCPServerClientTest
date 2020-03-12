using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServerTest
{
    public class ClientHandler
    {
        private static List<Thread> threads = new List<Thread>();
        private static List<TcpClient> clients = new List<TcpClient>();
        private TcpClient client;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
            lock (clients)
            {
                clients.Add(client);
            }
        }

        public void Start()
        {
            var t = new Thread(DoStuff);
            lock (threads)
            {
                threads.Add(t);
            }
            t.Start(client);
        }

        public void DoStuff(object c)
        {
            TcpClient client = (TcpClient)c;
            Console.WriteLine(client.Client.RemoteEndPoint + " connected. ");
            bool connected = true;

            while (connected)
            {
                try
                {
                    byte[] buffer = new byte[1000];
                    //client.GetStream().Flush();
                    client.GetStream().Read(buffer, 0, 1000);

                    string msg = Encoding.ASCII.GetString(buffer);
                    msg = msg.Trim('\0');
                    if(msg.Length != 0)
                        Console.WriteLine(client.Client.RemoteEndPoint + " says " + msg);
                }
                catch(IOException)
                {
                    connected = false;
                }
            }
            Console.WriteLine("Closing client " + client.Client.RemoteEndPoint);
            client.Close();
            clients.Remove(client);
            //threads.Remove();
        }
    }
}
