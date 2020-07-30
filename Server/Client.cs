using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client : IObservable<Message>
    {
        NetworkStream stream;
        TcpClient client;
        public string UserId;
        IDisposable unsubscriber;

        private List<IObserver<Message>> observers;

        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            UserId = "495933b6-1762-47a1-b655-483510072e73";
            observers = new List<IObserver<Message>>();
        }
        public Client(NetworkStream Stream, TcpClient Client, string userName)
        {
            stream = Stream;
            client = Client;
            UserId = userName;
            observers = new List<IObserver<Message>>();
        }
        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public string Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
            recievedMessageString = recievedMessageString.Replace("\0", string.Empty);
            Console.WriteLine(recievedMessageString);
            return recievedMessageString;
        }
        public void Update()
        {
            if(stream.DataAvailable)
            {
                Message message = new Message(this, Recieve());
                foreach(IObserver<Message> observer in observers)
                {
                    observer.OnNext(message);
                }
            }
        }
        public IDisposable Subscribe(IObserver<Message> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber(observers, observer);
        }
    }
}
