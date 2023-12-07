using Moq;

using ModelExTests;
using ModelsEx;
using ModelsEx.Abstraction;
using Azure.Messaging;
using ModelsEx.Models;
using ModelsEx.Services;

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
        public void ServerTest()
        {


            var mock = new MockMessageSource();

            var server = new Server(mock);

            mock.AddServer(server);

            server.Work();

            using(MessageContext ctx = new MessageContext())
            {
                Assert.IsTrue(ctx.Users.Count() == 2, "������������ �� �������");

                var user1 = ctx.Users.FirstOrDefault(x => x.Name == "����");
                var user2 = ctx.Users.FirstOrDefault(x => x.Name == "���");

                Assert.IsNotNull(user1, "������������ �� �������");
                Assert.IsNotNull(user2, "������������ �� �������");

                Assert.IsTrue(user1.FromMessages.Count == 1);
                Assert.IsTrue(user2.FromMessages.Count == 1);

                Assert.IsTrue(user1.ToMessage.Count == 1);
                Assert.IsTrue(user2.ToMessage.Count == 1);

                var msg1 = ctx.Messages.FirstOrDefault(x => x.FromUser == user1 && x.ToUser == user2);
                var msg2 = ctx.Messages.FirstOrDefault(x => x.FromUser == user2 && x.ToUser == user1);

                Assert.AreEqual("�� ���", msg2.Text);
                Assert.AreEqual("�� ����", msg1.Text);

            }

        }


    }
}