using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server : IObserver<Message>
    {
        List<Client> clients;
        public static Client client;
        TcpListener server;
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Message value)
        {
            Respond(value);
        }

        public void Run()
        {
            AcceptClient();
            string message = client.Recieve();
            Respond(message);
        }
        public void MainLoop()
        {
            while (true)
            {
                if (server.Pending())
                {
                    AcceptClient();
                }
            }

        }
        private void AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket, GetUserName(stream));
            clients.Add(client);
        }
        private void Respond(string body)
        {
            foreach (Client client in clients)
            {
                client.Send(body);
            }
        }
        private void Respond(Message message)
        {
            foreach (Client client in clients)
            {
                string outgoingString = message.UserId + message.Body;
                message.sender.Send(outgoingString);
            }
        }
        private string GetUserName(NetworkStream stream)
        {
            if (stream.DataAvailable)
            {
                byte[] userName = new byte[256];
                stream.Read(userName, 0, userName.Length);
                string userNameString = Encoding.ASCII.GetString(userName);
                return userNameString.Replace("\0", string.Empty);
            }
            return "Unknown User";
        }
    }
}
