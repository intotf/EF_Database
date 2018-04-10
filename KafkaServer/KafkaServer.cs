using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaServer
{
    public class KafkaMq
    {
        private readonly KafkaOptions options;


        private readonly BrokerRouter router;

        private readonly Producer producer;

        public KafkaMq()
        {
            this.options = new KafkaOptions(new Uri("http://localhost:9092"));
            this.router = new BrokerRouter(options);
            this.producer = new Producer(router);
        }

        public void Send()
        {
            for (var i = 1; i <= 10; i++)
            {
                producer.SendMessageAsync("TestHarness", new[] { new Message("hello world_" + i.ToString()) }).Wait();
            }
        }

        public void Get()
        {
            var consumer = new Consumer(new ConsumerOptions("TestHarness", router));
            var dataList = new List<KafMessage>();
            var topic = consumer.GetTopic("TestHarness");
            //Consume returns a blocking IEnumerable (ie: never ending stream)

            consumer.SetOffsetPosition(new OffsetPosition() { Offset = 50, PartitionId = 0 });
            foreach (var message in consumer.Consume())
            {
                var model = new KafMessage
                {
                    PartitionId = message.Meta.PartitionId,
                    Body = message.Value,
                    Offset = message.Meta.Offset
                };
                dataList.Add(model);

                Console.WriteLine(model.ToString());
            }
            Console.WriteLine("完成.");
            var ss = "";
        }

        public void Del()
        {
            var topic = producer.GetTopic("TestHarness");
        }
    }

    public class KafMessage
    {
        public int PartitionId { get; set; }

        public long Offset { get; set; }

        public byte[] Body { get; set; }

        public override string ToString()
        {
            return string.Format("Response: P{0},O{1} : {2}",
                     this.PartitionId, this.Offset, Encoding.UTF8.GetString(this.Body));
        }
    }

}
