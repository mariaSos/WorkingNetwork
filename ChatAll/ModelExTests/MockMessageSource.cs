using ModelsEx.Abstraction;
using ModelsEx.Models;
using ModelsEx.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModelExTests
{
    public class MockMessageSource :IMessageSource
    {
        private Server server;

        //private Client client;

        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

        private Queue<MessageUdp> messages = new Queue<MessageUdp>();

       // private IPEndPoint ipServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

        // UdpClient udpClient = new UdpClient();
        //IPEndPoint ipServer;
       // UdpClient udpClient;

        public MockMessageSource() 
        {
            messages.Enqueue(new MessageUdp { Command = Command.Register, FromName = "Вася" });
            messages.Enqueue(new MessageUdp { Command = Command.Register, FromName = "Юля" });
            messages.Enqueue(new MessageUdp { Command = Command.Message, FromName = "Юля", ToName = "Вася", Text = "От Юли" });
            messages.Enqueue(new MessageUdp { Command = Command.Message, FromName = "Вася", ToName = "Юля", Text = "От Васи" });
        }

       // public void Send(MessageUdp message, IPEndPoint ep)
        //{
            //throw new NotImplementedException();
        //}




        


        public MessageUdp Receive(ref IPEndPoint ep)
        {
            ep = endPoint;
           
            if (messages.Count == 0)
            {
                //client.Stop();
                server.Stop();
                return null;
           }
            return messages.Dequeue();
        }


        public void Send(MessageUdp message, IPEndPoint ep)
        {
           // ep = endPoint;

            
         /*   if (messages.Count == 0)
             {
            client.Stop();

            

              }
            messages.Dequeue();
            */

           // byte[] forwardBytes = Encoding.ASCII.GetBytes(message.MessageToJson());

            // udpClient.Send(forwardBytes, forwardBytes.Length, ep);
        }



      /*  public void AddClient(Client client)
        {
           this.client = client;

        }*/


        public void AddServer(Server serv) 
        { 
            server = serv;

        }

        //Test
        public void TestCtx(string testText)
        {
            using (MessageContext ctx = new MessageContext())
            {
                ctx.Add(new User() {Name = testText }); ;
                ctx.SaveChanges();
            }
        }


    }
}
