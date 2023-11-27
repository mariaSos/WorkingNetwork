using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace Chat
{

    //Попробуйте переработать приложение,
    //добавив подтверждение об отправке сообщений как в сервер, так и в клиент.


    internal class Program
    {
        private const string serverName = "Server";

        public static async Task Server()
        {
            Dictionary<string,IPEndPoint> Users = new Dictionary<string,IPEndPoint>();

            UdpClient udpServer = new UdpClient(12345);
           
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
           
            Console.WriteLine("Ожидаем сообщение от пользователя");

            //Выходим при вводе команды "Exit" на клиенте
            string? exit = null;
            while (true) 
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

                        StringBuilder answer = new StringBuilder("Сообщение получено");

                        if(dataMessage.ToName == "Server")
                        {
                            if(dataMessage.TextMessage == "register")
                            {
                                Users.Add(dataMessage.NickName, new IPEndPoint(iPEndPoint.Address, iPEndPoint.Port));
                                answer.Append($"Пользователь: {dataMessage.NickName} добавлен");
                            }
                            else if (dataMessage.TextMessage == "delete")
                            {
                                Users.Remove(dataMessage.NickName);
                                answer.Append($"Пользователь: {dataMessage.NickName} удален");

                            }
                            else if (dataMessage.TextMessage == "list")
                            {
                                foreach (var item in Users)
                                {
                                    answer.Append($"Пользователь: {item.Key} c IP: {item.Value}\n");
                                }
                            }
                        }
                        else
                        {
                            if (Users.TryGetValue(dataMessage.ToName,out IPEndPoint ipEndPoint))
                            {
                                var mess = new Message()
                                {
                                    NickName = dataMessage.NickName,
                                    DateMessage = DateTime.Now,
                                    TextMessage = answer.ToString()

                                };

                                data = mess.MessageToJson();

                                buffer = Encoding.ASCII.GetBytes(data);

                                udpServer.Send(buffer, buffer.Length, iPEndPoint);
                                answer.Append("Сообщение отправлено клиенту");
                            }
                            else
                            {
                                answer.Append("Такого пользователя не существует");
                            }

                            var replyMessageJson =
                            new Message() 
                            { DateMessage = DateTime.Now, 
                                NickName = serverName,
                                TextMessage = answer.ToString() 
                            }.MessageToJson();

                            byte[] replyBytes = Encoding.ASCII.GetBytes(replyMessageJson);

                            udpServer.Send(replyBytes, replyBytes.Length, iPEndPoint);
                            Console.WriteLine("Ответ отправлен.");
                        }

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

            //4.Мы собираемся сделать наш класс полностью клиент - серверным с возможностью отправки данных сразу нескольким клиентам.
            //Доработаем наш код следующим образом.Представьте что наш сервер умеет работать как медиатор
            //(умеет отправлять сообщения по имени клиента),
            //а также умеет возвращать список всех подключенных к нему клиентов.
            //Для этого доработаем наш класс сообщений добавив поле ToName.
            //5.Доработаем систему команд. Имя пользователя сервера всегда будет Server. Если сервер получает команду (как текст сообщения):
            //register: то он добавляет клиента в свой список.
            //delete: он удаляет клиента из списка
            //если сервер не получает имени получателя то он отправляет сообщение всем клиентам
            //если сервер получает имя получателя то он отправляет сообщение одному конкретному клиенту.
            //Код сервера должен выглядеть примерно следующим образом:



            }

        }

        public static void Client(string name, string ip/*, string message*/)
        {

            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
           
            string ToName = string.Empty;

            string[] message = Console.ReadLine().Split(' ');

            Message mess = new Message()
            { NickName = name,
                DateMessage = DateTime.Now, 
                TextMessage = message[1],
                ToName = message[0]
            };

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
                
              //  while ( exit != "Exit" ) {
              //      exit = Console.ReadLine();
                    
              //  }
                //Если ввели команду "Exit", то передаем ее серверу для окончания
                //if (exit == "Exit")
               // {
                //    var buffer_ex = Encoding.ASCII.GetBytes("Exit");

                //    udpClient.Send(buffer_ex, buffer_ex.Length, iPEndPoint);
              //  }

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
               // for (int i = 0; i < 10; i++)
               // {

                    await Task.Run(() => Client(args[0] , args[1]));
                //}  

            }

        }
    }
}