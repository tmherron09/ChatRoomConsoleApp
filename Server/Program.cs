using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Server().Run();
            Server server = new Server();
            List<Client> clients = server.clients;
            while(true)
            {
                server.Update();
                foreach(Client client in clients)
                {
                    client.Update();
                }
            }



            Console.ReadLine();
        }

        
    }
}
