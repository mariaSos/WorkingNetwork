using ChatCommon;
using ChatNetWork;
using System.Net;
//using System.Net;

namespace ChatApp;

class Program
{

   
    
    static void Main(string[] args)
    {
        UdpMessageSource source = new UdpMessageSource();

        Server<EndPoint> server = new Server<EndPoint>(source);



        // serv.Work();

        Console.WriteLine("Hello, World!");
    }
}
