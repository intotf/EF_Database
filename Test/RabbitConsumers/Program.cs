using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQServer;
using KafkaServer;

namespace RabbitConsumers
{
    class Program
    {
        static void Main(string[] args)
        {
            var MqServer = new MqFactory();
            var i = 0;
            while (i < 100)
            {
                var data = MqServer.Get("测试主题", false);
                if (data == null)
                {
                    Console.WriteLine(i);
                    break;
                }
                else
                {
                    i++;
                    MqServer.Ack(data.Tag);
                }
            }
            Console.WriteLine("RabbitMQ 本次总共处理：{0}条记录", i);

            var kafka = new KafkaMq();
            //kafka.Send();
            kafka.Get();

            Console.ReadKey();

        }
    }
}
