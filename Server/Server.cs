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
        public List<Client> clients;
        TcpListener server;
        List<IDisposable> unsubscribers;


        public Server()
        {
            clients = new List<Client>();
            unsubscribers = new List<IDisposable>();
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }

        public void Subscribe(IObservable<Message> newClient)
        {
            if (newClient != null)
            {
                unsubscribers.Add(newClient.Subscribe(this));
            }
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
            string outgoingMessage = $"{value.UserId}: {value.Body}";
            Respond(outgoingMessage);
        }

        //public void Run()
        //{
        //    AcceptClient();
        //    string message = client.Recieve();
        //    Respond(message);
        //}
        public void Update()
        {
            if (server.Pending())
            {
                AcceptClient();
            }
        }
        private void AcceptClient()
        {
            Client client;
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket);
            clients.Add(client);
            client.UserId = GetUserName(stream);
            Subscribe(client);
            AlertNewUser(client);
            
        }
        private void AlertNewUser(Client client)
        {
            string outgoingString = $"Welcome *{client.UserId}* to the server!";
            Console.WriteLine(outgoingString);
            Respond(outgoingString);
        }
        // Message from Server
        private void Respond(string body)
        {
            foreach (Client client in clients)
            {
                client.Send(body);
            }
        }
        // Message from one user to all.
        private void Respond(Message message)
        {
            foreach (Client client in clients)
            {
                string outgoingString = message.UserId + message.Body;
                message.sender.Send(outgoingString);
            }
        }
        // Immediately catch username upon connection if available.
        private string GetUserName(NetworkStream stream)
        {
            byte[] userName = new byte[256];
            stream.Read(userName, 0, userName.Length);
            string userNameString = Encoding.ASCII.GetString(userName);
            userNameString = userNameString.Replace("\0", string.Empty);
            if (userNameString != null)
            {
                return userNameString;
            }
            return "Unknown User";
        }
    }
}
