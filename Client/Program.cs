using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        // Same Computer: "127.0.0.1"
        // LAN: 192.168.1.3
        // Codeanywhere: ??

        static void Main(string[] args)
        {
            Console.WriteLine("Please input your username:");
            string userName = UI.GetInput();
            Client client = new Client("127.0.0.1", 9999, userName);
            client.Send(userName);
            while (true)
            {
                client.Recieve();
                client.Send();
                
            }
            Console.ReadLine();
        }
    }
}
