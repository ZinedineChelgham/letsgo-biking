
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using RoutingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace routing_server
{
    internal class MqController
    {

       
        private Service1 service1;
        private ISession session;
        private IMessageProducer producer;
        private IMessageConsumer consumer;



        public MqController(string url, Service1 service1)
        {

            this.service1 = service1;
            IConnection connection = new ConnectionFactory(url).CreateConnection();
            connection.Start();
            
            session = connection.CreateSession();
            IDestination destination = session.GetQueue("LBQ");
            
            producer = session.CreateProducer(destination);
            producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

            consumer = session.CreateConsumer(destination, "messageType = 'request'");

           // consumer.Listener += new MessageListener(OnMessageListener);
           

        }

        public void sendMessageToQueue(List<string> route)
        {

            foreach (string s in route)
            {
                ITextMessage message = session.CreateTextMessage(s);
                message.Properties.SetString("messageType", "response");
                producer.Send(message);
            }

        }
    }
}
