using ChatApp.Client;

class Program
{
    static void Main()
    {
        ClientModel model = new ClientModel();
        ClientView view = new ClientView();
        ClientController controller = new ClientController(model, view);

        controller.StartClient("localhost", 8888);
    }
}
