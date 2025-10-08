using ChatApp.Server;

class Program
{
    static void Main()
    {
        ServerModel model = new ServerModel();
        ServerView view = new ServerView();
        ServerController controller = new ServerController(model, view);

        controller.StartServer(8888);
    }
}
