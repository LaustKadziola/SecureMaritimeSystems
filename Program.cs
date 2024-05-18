
using MMSAgent;
using MMSEdgeRouter;

class Program
{

    public static void Main(string[] args)
    {

        if (args[0] == "edge")
        {
            Console.WriteLine("running Edgerouter");
            RunEdgeRouter er = new RunEdgeRouter();
        }
        else
        {
            Console.WriteLine("running agent");
            Agent a = new Agent();

        }

    }

}