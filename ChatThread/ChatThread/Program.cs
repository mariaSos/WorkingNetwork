using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat
{

    //Добавьте возможность ввести слово Exit в чате клиента, чтобы можно было завершить его работу.
    //В коде сервера добавьте ожидание нажатия клавиши, чтобы также прекратить его работу.


    internal class Program
    {
        //Отправляем сообщение
        public static void DataSend(Message mess, UdpClient udpClient, IPEndPoint iPEndPoint)
        {
            var data = mess.MessageToJson();

            byte[] buffer = Encoding.ASCII.GetBytes(data);

            udpClient.Send(buffer, buffer.Length, iPEndPoint);

            
        }

        //Получаем сообщение
        public static Message DataRecieve(UdpClient udpClient, IPEndPoint iPEndPoint, byte[] buffer)
        {

            var data = Encoding.ASCII.GetString(buffer);

            var dataMessage = Message.MessageFromJson(data);

            Console.WriteLine($" Получено сообщение от {dataMessage.NickName} + время сообщения {dataMessage.DateMessage} ");

            Console.WriteLine($"{dataMessage.TextMessage}");
           
            return dataMessage;
        }


        public static void Server()
        {

            UdpClient udpServer = new UdpClient(12345);

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Ожидаем сообщение от пользователя");

            string? messText = null;


            while (messText != "Exit")
            {
                try
                {
                   
                   byte[] buffer = udpServer.Receive(ref iPEndPoint);

                    //Получаем сообщение
                    var dataMessage = DataRecieve(udpServer, iPEndPoint, buffer);

                    messText = dataMessage.TextMessage;

                    string answer = "Сообщение получено";

                    var mess = new Message()
                    {
                        NickName = dataMessage.NickName,
                        DateMessage = DateTime.Now,
                        TextMessage = answer

                    };



                    //Отправляем сообщение
                    DataSend(mess, udpServer, iPEndPoint);

                }
                catch (Exception ex)
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
            {
                NickName = name,
                DateMessage = DateTime.Now,
                TextMessage = message
            };


            try
            {
                
                //Отправляем сообщение
                DataSend(mess, udpClient, iPEndPoint);

                Console.WriteLine("Сообщение отправлено");

                var buffer = udpClient.Receive(ref iPEndPoint);

                //Получаем сообщение
                var dataMessage = DataRecieve(udpClient, iPEndPoint, buffer);

                string? messText = null;
                
                //Ждем команды Exit
                while (messText != "Exit")
                {

                    messText = Console.ReadLine();
                    mess.TextMessage = messText ?? "Пустое сообщение";
                    mess.DateMessage = DateTime.Now;

                    //Отправляем сообщение
                    DataSend(mess, udpClient, iPEndPoint);

                    //Получаем сообщение
                    dataMessage = DataRecieve(udpClient, iPEndPoint, buffer);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Server();
            }
            else
            {
                //  for (int i = 0; i < 10; i++)
                //  {

                new Thread(() => Client(args[0], args[1], args[2]))
                .Start();
                // }

            }

        }
    }
}