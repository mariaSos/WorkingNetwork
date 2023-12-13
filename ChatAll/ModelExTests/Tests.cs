using Moq;

using ModelExTests;
using ModelsEx;
using ModelsEx.Abstraction;
using Azure.Messaging;
using ModelsEx.Models;
using ModelsEx.Services;
using System.Net;

namespace ModelExTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            using (MessageContext ctx = new MessageContext())
            {
                ctx.Messages.RemoveRange(ctx.Messages);
                ctx.Users.RemoveRange(ctx.Users);

                ctx.SaveChanges();
            }


        }

        [TearDown]
        public void TearDown()
        {
            using (MessageContext ctx = new MessageContext())
            {
                ctx.Messages.RemoveRange(ctx.Messages);
                ctx.Users.RemoveRange(ctx.Users);

                ctx.SaveChanges();
            }


        }


        [Test]
        public async Task ServerTest()
        {

            var mock = new MockMessageSource();
             var server = new Server(mock);
           
            //var client1 = new Client(mock,"127.0.0.1",5522,"Юля");
            //var client2 = new Client(mock, "127.0.0.1", 5533, "Вася");


            mock.AddServer(server);
           // mock.AddClient(client1);
           // mock.AddClient(client2);
            //  client.Start();
           //  Task.Run(() =>
           //   {
           // mock.TestCtx("2");
           // client1.Start();
          //  client2.Start();
            server.Work();
            // mock.TestCtx("2");
            //  });
            // client.Stop();



           // mock.TestCtx("3");
          //  await Task.Run(() =>
          //  {
                using (MessageContext ctx = new MessageContext())
                {
                     Assert.IsTrue(ctx.Users.Count() == 2, "Пользователи не созданы");

                    var user1 = ctx.Users.FirstOrDefault(x => x.Name == "Вася");
                    var user2 = ctx.Users.FirstOrDefault(x => x.Name == "Юля");

                    Assert.IsNotNull(user1, "Пользователь не создан");
                    Assert.IsNotNull(user2, "Пользователь не создан");

                    Assert.IsTrue(user1.FromMessages.Count == 1);
                    Assert.IsTrue(user2.FromMessages.Count == 1);

                    Assert.IsTrue(user1.ToMessage.Count == 1);
                    Assert.IsTrue(user2.ToMessage.Count == 1);

                    var msg1 = ctx.Messages.FirstOrDefault(x => x.FromUser == user1 && x.ToUser == user2);
                    var msg2 = ctx.Messages.FirstOrDefault(x => x.FromUser == user2 && x.ToUser == user1);

                    Assert.AreEqual("От Юли", msg2.Text);
                    Assert.AreEqual("От Васи", msg1.Text);

                   
                }
          //  });
            







            // mock.TestCtx();

        }


    }
}