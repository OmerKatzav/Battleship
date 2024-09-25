using System;

namespace BattleshipServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            BattleshipServer server = new BattleshipServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\owner\source\repos\Battleship\BattleshipServer\Database.mdf;Integrated Security=True");
            server.Start();
            Console.WriteLine("Server started. Press any key to stop.");
            Console.ReadKey();
            server.Stop();
        }
    }
}
