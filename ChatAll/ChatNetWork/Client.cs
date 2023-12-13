using ChatCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatNetWork
{
    public class Client<T> 
    {
        //T ipServer;
        UdpClient udpClient;

        private readonly string _adress;
        private readonly int _port;
        private readonly string _name;

        IMessageSourceClient<T> messageSource;

        public Client(IMessageSourceClient<T> source, string adress, int port, string name)
        {
            _adress = adress;
            _port = port;
            _name = name;
            messageSource = source;
            udpClient = new UdpClient(_port);
            
            //ipServer = messageSource.CreateNewT(adress, port);
                //new IPEndPoint(IPAddress.Parse(_adress), _port);


        }

        public void Start()
        {
            T ep = messageSource.CreateNewT();
            new Thread(() => { Listener(ep); }).Start();
            Sender(ep);
        }

        public void Listener(T ep)
        {
           
            
            while (true)
            {
                try
                {

                    var message = messageSource.Receive(ref ep);
                    Console.WriteLine("Получено сообщение от: " + message.FromName);
                    Console.WriteLine(message.Text);

                    Confirmation(message, ep);

                }
                catch (Exception e)
                {

                    Console.WriteLine("Возникло исключение: " + e);
                }

            }
        }

        public void Sender(T ep)
        {

            Register(ep);

            while (true)
            {
                try
                {
                    Console.WriteLine("Ожидается ввод сообщения:");
                    var text = Console.ReadLine();
                    Console.WriteLine("Введите имя получателя:");
                    var toName = Console.ReadLine();

                    MessageUdp message = new MessageUdp()
                    {
                        Command = Command.Message,
                        ToName = toName,
                        FromName = _name,
                        Text = text
                    };

                    messageSource.Send(message, ep);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при обработке сообщения : " + e);
                }

            }


        }
        public void Register(T ep)
        {
            MessageUdp message = new MessageUdp()
            {
                Command = Command.Register,
                ToName = null,
                FromName = _name,
                Text = null
            };
            messageSource.Send(message, ep);
        }

        public void Confirmation(MessageUdp mess, T ep)
        {
            MessageUdp message = new MessageUdp()
            {
                Id = mess.Id,
                Command = Command.Confirmation,
                ToName = null,
                FromName = _name,
                Text = null
            };
            messageSource.Send(message, ep);
        }
    }
}
