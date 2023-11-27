using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat
{

    //Попробуйте переработать приложение,
    //добавив подтверждение об отправке сообщений как в сервер, так и в клиент.


    internal class Program
    {
        public static async Task Server()
        {

            UdpClient udpServer = new UdpClient(12345);
           
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
           
            Console.WriteLine("Ожидаем сообщение от пользователя");

            //Выходим при вводе команды "Exit" на клиенте
            string? exit = null;
            while (exit != "Exit") 
            {
                
                


                Task.Run(() =>
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




                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });

                //Добавьте возможность ввести слово Exit в чате клиента,
                //чтобы можно было завершить его работу.
                //В коде сервера добавьте ожидание нажатия клавиши, чтобы также прекратить его работу.

                //Буфер для команды "Exit"

              //  byte[] buffer_ex = udpServer.Receive(ref iPEndPoint);

              //  string data_ex = Encoding.ASCII.GetString(buffer_ex);

              //  exit = data_ex;

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

                Console.WriteLine($"{dataMessage.TextMessage}!");

                string? exit = null;
                
                while ( exit != "Exit" ) {
                    exit = Console.ReadLine();
                    
                }
                //Если ввели команду "Exit", то передаем ее серверу для окончания
                if (exit == "Exit")
                {
                    var buffer_ex = Encoding.ASCII.GetBytes("Exit");

                    udpClient.Send(buffer_ex, buffer_ex.Length, iPEndPoint);
                }

            } catch( Exception ex )
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        static async Task Main(string[] args)
        {
           
            if (args.Length == 0) {
                await Task.Run(()=>  Server());
                
                    
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {

                    await Task.Run(() => Client(args[0] + i, args[1], args[2]));
                }  

            }

        }
    }
}