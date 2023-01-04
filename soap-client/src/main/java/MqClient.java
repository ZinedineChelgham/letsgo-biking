import org.apache.activemq.ActiveMQConnectionFactory;
import soap.ws.client.generated.ArrayOfstring;
import soap.ws.client.generated.IService1;
import soap.ws.client.generated.Service1;

import javax.jms.*;
import java.util.List;


public class MqClient implements IClient, MessageListener {

    private static Connection connection;
    private static Session session;
    private static MessageProducer producer;
    private static final String QUEUE_NAME = "LBQ";
    private static final String URL = "tcp://localhost:61616";
    private final IService1 routingServ;

    public MqClient() {
        Service1 service = new Service1();
        routingServ = service.getBasicHttpBindingIService1();

        try {
            // Create a ConnectionFactory
            ActiveMQConnectionFactory connectionFactory = new ActiveMQConnectionFactory(URL);

            // Create a Connection
            connection = connectionFactory.createConnection();
            connection.start();

            // Create a Session
            session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);

            // Create the destination (Topic or Queue)
            Destination destination = session.createQueue(QUEUE_NAME);

            // Create a MessageProducer from the Session to the Topic or Queue
            producer = session.createProducer(destination);
            producer.setDeliveryMode(DeliveryMode.NON_PERSISTENT);

            // Set the consumer
            MessageConsumer consumer = session.createConsumer(destination, "messageType = 'response'");
            consumer.setMessageListener(this);

        } catch (JMSException e) {
            e.printStackTrace();
        }
    }


public void showRoute(String start, String end,boolean isMq) {
    //call the function to get informations about itinerary
    //array of string
    ArrayOfstring instructions = routingServ.getRoute(start, end,isMq);
    List<String> list = instructions.getString();
    //print the instructions and color each line that contains "*" in red and  "bike" in blue and any other line in bold white
    for (String instruction : list) {
        if (instruction.contains("*")) {
            System.out.println("\033[31m" + instruction + "\033[0m");
        } else if (instruction.contains("bike")) {
            System.out.println("\033[34m" + instruction + "\033[0m");
        } else {
            System.out.println("\033[1m" + instruction + "\033[0m");
        }
    }




}
    @Override
    public void onMessage(Message message) {
        try {
            System.out.println("instruction Received: " + ((TextMessage) message).getText());
        } catch (JMSException e) {
            e.printStackTrace();

        }
    }

    @Override
    public void close() {
        //close the connection
        try {
            connection.close();
        } catch (JMSException e) {
            e.printStackTrace();
        }
    }
}