using ChatTestClient.Models;
using System.Net;


namespace ChatTestClient.Abstraction
{
    public interface IMessageSource
    {
        void Send(MessageUdp message, IPEndPoint ep);
        MessageUdp Receive(ref IPEndPoint ep);
    }
}
