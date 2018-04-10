using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQServer;
using System.Text;
using KafkaServer;

namespace RabbitMQTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var factory = new MqFactory();
            for (var i = 0; i <= 1000; i++)
            {
                factory.Publish("测试主题", Encoding.UTF8.GetBytes("MqContent_" + i.ToString()));
            }
        }

        [TestMethod]
        public void TestKafka()
        {
            var opt = new KafkaMq();
            opt.Send();
            opt.Get();
        }
    }
}
