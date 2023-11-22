using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat
{

    //Попробуйте переработать приложение,
    //добавив подтверждение об отправке сообщений как в сервер, так и в клиент.


    internal class Program
    {
        public static void Server()
        {

            UdpClient udpServer = new UdpClient(12345);
           
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
           
            Console.WriteLine("Ожидаем сообщение от пользователя");
           
           

            while(true)
            {
                try
                {
                    byte[] buffer = udpServer.Receive(ref iPEndPoint);

                    string data = Encoding.ASCII.GetString(buffer);

                    var dataMessage = Message.MessageFromJson(data);

                    Console.WriteLine($" Получено сообщение от {dataMessage.NickName} + время сообщения {dataMessage.DateMessage} ");

                    Console.WriteLine($"{dataMessage.TextMessage}");
                    
                    string answer = "Сообщение получено";

                    var mess = new Message()
                    {
                        NickName = dataMessage.NickName,
                        DateMessage = DateTime.Now,
                        TextMessage = answer


                    };

                    data = mess.MessageToJson();

                    buffer = Encoding.ASCII.GetBytes(data);

                    udpServer.Send(buffer, buffer.Length, iPEndPoint);



                } catch ( Exception ex ) 
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }

        public static void Client(string name, string ip, string message)
        {

            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
           
            Message mess = new Message()
            { NickName = name,
                DateMessage = DateTime.Now, 
                TextMessage = message};

            try
            {
                var data = mess.MessageToJson();

                byte[] buffer = Encoding.ASCII.GetBytes(data);

                udpClient.Send(buffer, buffer.Length, iPEndPoint);

                Console.WriteLine("Сообщение отправлено");

                buffer = udpClient.Receive(ref iPEndPoint);

                data = Encoding.ASCII.GetString(buffer);

                var dataMessage = Message.MessageFromJson(data);

                Console.WriteLine($" Получено сообщение от {dataMessage.NickName} + время сообщения {dataMessage.DateMessage} ");

                Console.WriteLine($"{dataMessage.TextMessage}");


            } catch( Exception ex )
            {
                Console.WriteLine(ex.Message);
            }

            
        }

        static void Main(string[] args)
        {
           
            if (args.Length == 0) {
                Server();
            }
            else
            {
                
                    new Thread(() => Client(args[0], args[1], args[2]))
                    .Start();

            }

        }
    }
}