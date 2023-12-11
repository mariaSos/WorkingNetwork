using ChatTestClient.Abstraction;
using ChatTestClient.Models;
using ChatTestClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    public class MockMessageSource : IMessageSource
    {

        private Client client;

        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

        private Queue<MessageUdp> messages = new Queue<MessageUdp>();



        public MockMessageSource()
        {
            messages.Enqueue(new MessageUdp { Command = Command.Register, FromName = "Вася" });
            messages.Enqueue(new MessageUdp { Command = Command.Register, FromName = "Юля" });
            messages.Enqueue(new MessageUdp { Command = Command.Message, FromName = "Юля", ToName = "Вася", Text = "От Юли" });
            messages.Enqueue(new MessageUdp { Command = Command.Message, FromName = "Вася", ToName = "Юля", Text = "От Васи" });
        }


      public MessageUdp Receive(ref IPEndPoint ep)
        {
              ep = endPoint;

            if (messages.Count == 0)
            {
                client.Stop();
                return null;
            }
            return messages.Dequeue();
        }


        public void Send(MessageUdp message, IPEndPoint ep)
        {
             ep = endPoint;


            if (messages.Count == 0)
            {
                client.Stop();


            }
            messages.Dequeue();
    }



        public void AddClient(Client client)
        {
            this.client = client;

        }


    }
}
