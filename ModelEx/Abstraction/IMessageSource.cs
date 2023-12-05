using ModelsEx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEx.Abstraction
{
    interface IMessageSource
    {
        void Send(MessageUdp message, IPEndPoint ep);
        MessageUdp Receive(ref IPEndPoint ep);
    }
}
