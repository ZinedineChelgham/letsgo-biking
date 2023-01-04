import soap.ws.client.generated.IService1;

public interface IClient {

    void showRoute(String start, String end,boolean isMq);

    void close();

}
