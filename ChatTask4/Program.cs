using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatTask4
{
    internal class Program
    {
        private const string serverName = "Server";

        // 1. Мы собираемся сделать наш класс полностью клиент-серверным с возможностью
        // отправки данных сразу нескольким клиентам.
        // Доработаем наш код следующим образом. 
        // Представьте что наш сервер умеет работать как медиатор (умеет отправлять 
        // сообщения по имени клиента), а также умеет возвращать список всех подключенных 
        // к нему клиентов.
        // Для этого доработаем наш класс сообщений добавив поле ToName.

        // 2. Доработаем систему команд.
        // Имя пользователя сервера всегда будет Server.
        // Если сервер получает команду (как текст сообщения):
        // register : то он добавляет клиента в свой список.
        // delete: он удаляет клиента из списка
        // если сервер не получает имени получателя то он отправляет сообщение всем клиентам
        // если сервер получает имя получателя то он отправляет сообщение одному конкретному клиенту.


        public static void Server()
        {
            Dictionary<string, IPEndPoint> UsersList = new Dictionary<string, IPEndPoint>();
            UdpClient udpServer = new UdpClient(12345);
            IPEndPoint localRemouteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Ожидаем сообщение от пользователя:");

            while (true)
            {
                Task.Run(() =>
                {
                    try
                    {
                        byte[] buffer = udpServer.Receive(ref localRemouteEndPoint);
                        string data = Encoding.ASCII.GetString(buffer);
                        var message = Message.MessageFromJson(data);
                        Console.WriteLine($"Получено сообщение от {message.NickName}, для {message.ToName}" +
                        $" время получения {message.DateMessage}, ");
                        Console.WriteLine(message.TextMessage);

                        StringBuilder answer = new StringBuilder("Сообщение получено");

                        if (message.ToName == serverName)
                        {
                            if (message.TextMessage == "register")
                            {
                                UsersList.Add(message.NickName, new IPEndPoint(localRemouteEndPoint.Address, localRemouteEndPoint.Port));
                                answer.Append($"\nПользователь {message.NickName} добавлен.");
                            }
                            if (message.TextMessage == "delete")
                            {
                                UsersList.Remove(message.NickName);
                                answer.Append($"\nПользователь {message.NickName} удалён.");
                            }
                            if (message.TextMessage == "list")
                            {
                                foreach (var i in UsersList)
                                {
                                    answer.Append($"\nПользователь: {i.Key}, IP: {i.Value}");
                                }
                            }
                        }
                        else
                        {
                            if (UsersList.TryGetValue(message.ToName, out IPEndPoint? ep))
                            {
                                var answerMessage = new Message()
                                {
                                    DateMessage = DateTime.Now,
                                    NickName = message.ToName,
                                    TextMessage = answer.ToString(),
                                    ToName = message.ToName
                                };

                                var answerData = answerMessage.MessageToJson();
                                byte[] bytes = Encoding.ASCII.GetBytes(answerData);
                                udpServer.Send(bytes, bytes.Length, ep);
                                answer.Append($"\nСообщение переслано {message.ToName}.");
                            }
                            else
                            {
                                answer.Append("\nТакого пользователя не существует!");
                            }
                        }

                        var replyMessageJson = new Message()
                        {
                            DateMessage = DateTime.Now,
                            NickName = serverName,
                            TextMessage = answer.ToString()
                        }.MessageToJson();

                        byte[] replyBytes = Encoding.ASCII.GetBytes(replyMessageJson);

                        udpServer.Send(replyBytes, replyBytes.Length, localRemouteEndPoint);
                        Console.WriteLine("Ответ отправлен.");

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
            }
        }

        public static void Client(string name, string ip)
        {

            UdpClient udpClient = new UdpClient();
            IPEndPoint localRemouteEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);

            while (true)
            {
                string[] message = Console.ReadLine().Split(' ');

                var mess = new Message()
                {
                    DateMessage = DateTime.Now,
                    NickName = name,
                    TextMessage = message[1],
                    ToName = message[0]
                };

                Console.WriteLine($"{mess.ToName}:");



                try
                {
                    var data = mess.MessageToJson();
                    byte[] bytes = Encoding.ASCII.GetBytes(data);
                    udpClient.Send(bytes, bytes.Length, localRemouteEndPoint);


                    Console.WriteLine("Сообщение отправлено.");


                    byte[] buffer = udpClient.Receive(ref localRemouteEndPoint);
                    data = Encoding.ASCII.GetString(buffer);
                    var messageReception = Message.MessageFromJson(data);
                    Console.WriteLine($"Получено сообщение от {messageReception.NickName}," +
                    $" время получения {messageReception.DateMessage}, ");
                    Console.WriteLine(messageReception.TextMessage);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
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
                Client(args[0], args[1]);
            }

        }
    }
}