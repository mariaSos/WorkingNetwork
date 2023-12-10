using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatCancel
{


    //Добавьте возможность ввести слово Exit в чате клиента, чтобы можно было завершить его работу.
    //В коде сервера добавьте ожидание нажатия клавиши, чтобы также прекратить его работу.


    internal class Program
    {

        //Токен для остановки сервера
        static private CancellationTokenSource cts = new CancellationTokenSource();

        static private CancellationToken ct = cts.Token;

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


        public static async Task Server(string name)
        {

            UdpClient udpServer = new UdpClient(12345);

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Ожидаем сообщение от пользователя");

            string? messText = null;


            while (!ct.IsCancellationRequested)
            {
                await Task.Run(() =>
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
                            NickName = name,
                            DateMessage = DateTime.Now,
                            TextMessage = answer

                        };


                        if (messText == "Exit")
                        {
                            //Токен для остановки сервера

                            cts.Cancel();

                        }

                        //Отправляем сообщение
                        DataSend(mess, udpServer, iPEndPoint);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });

            }

        }

        public static void Client(string name, string ip)
        {

            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            string message = string.Empty;

            Message mess = new Message()
            {
                NickName = name,
                DateMessage = DateTime.Now,
                TextMessage = message
            };



            while (message != "Exit")
            {

                message = Console.ReadLine() ?? "Пустое сообщение";

                mess.TextMessage = message ?? "Пустое сообщение";

                mess.DateMessage = DateTime.Now;

                try
                {

                    //Отправляем сообщение
                    DataSend(mess, udpClient, iPEndPoint);

                    Console.WriteLine("Сообщение отправлено");

                    var buffer = udpClient.Receive(ref iPEndPoint);

                    //Получаем сообщение
                    var dataMessage = DataRecieve(udpClient, iPEndPoint, buffer);

                    string? messText = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }
        static async Task Main(string[] args)
        {

            if (args.Length == 0)
            {
                await Task.Run(() =>  Server("Сервер"));
            }
            else
            {

                Client(args[0], args[1]);

            }

        }
    }
}