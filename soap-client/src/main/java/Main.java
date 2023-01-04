
import soap.ws.client.generated.IService1;
import soap.ws.client.generated.Service1;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

/**
 * @author : Zinedine Chelgham
 **/

public class Main {

    public static void main(String[] args) throws Exception{
        //ask the user to choose between the two clients
        System.out.println("Choose a client : ");
        System.out.println("1. Classic Soap Client");
        System.out.println("2. Mq Client");
        Scanner scanner = new Scanner(System.in);
        int choice = scanner.nextInt();
        IClient client = null;
        switch (choice) {
            case 1:
                client = new ClassicSoapClient();

                break;
            case 2:
                client = new MqClient();

                break;
            default:
                System.out.println("Wrong choice");
                System.exit(0);
        }

        //print you have successfully connected to the client choice
        System.out.println("You have successfully connected to the client " + choice);

        //ask user to enter two address
        BufferedReader reader = new BufferedReader(
                new InputStreamReader(System.in));
        System.out.println("Enter the first address: ");
        String address1 = reader.readLine();
        System.out.println("Enter the second address: ");
        String address2 = reader.readLine();


        //call the function to get informations about itinerary1
        client.showRoute(address1, address2, choice != 1);
    }
}
