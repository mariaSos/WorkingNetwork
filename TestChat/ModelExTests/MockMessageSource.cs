using ModelsEx.Abstraction;
using ModelsEx.Models;
using ModelsEx.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModelExTests
{
    public class MockMessageSource :IMessageSource
    {
        private Server server;

        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

        private Queue<MessageUdp> messages = new Queue<MessageUdp>();



        public MockMessageSource() 
        {
            messages.Enqueue(new MessageUdp { Command = Command.Register, FromName = "Вася" });
            messages.Enqueue(new MessageUdp { Command = Command.Register, FromName = "Юля" });
            messages.Enqueue(new MessageUdp { Command = Command.Message, FromName = "Юля", ToName = "Вася", Text = "От Юли" });
            messages.Enqueue(new MessageUdp { Command = Command.Message, FromName = "Вася", ToName = "Юля", Text = "От Васи" });
        }

        public void Send(MessageUdp message, IPEndPoint ep)
        {
            throw new NotImplementedException();
        }

        public MessageUdp Receive(ref IPEndPoint ep)
        {
            ep = endPoint;

            if(messages.Count == 0)
            {
                return null;
            }
            return messages.Dequeue();
        }

        public void AddServer(Server serv) 
        { 
            server = serv;

        }


    }
}
