import soap.ws.client.generated.ArrayOfstring;
import soap.ws.client.generated.IService1;
import soap.ws.client.generated.Service1;

import java.util.ArrayList;
import java.util.List;


public class ClassicSoapClient implements IClient {
    private final IService1 routingServ;

    //constructor
    public ClassicSoapClient() {
        Service1 service = new Service1();
        routingServ = service.getBasicHttpBindingIService1();
    }

    public void showRoute(String start, String end,boolean isMq) {
        //call the function to get informations about itinerary
        //array of string
        ArrayOfstring instructions = routingServ.getRoute(start, end, isMq);
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
    public void close() {

    }
}

