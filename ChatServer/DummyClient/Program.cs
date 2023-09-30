using ServerCore;
using System.Net;

namespace DummyClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry IPhost = Dns.GetHostEntry(host);
            IPAddress iPAddress = IPhost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(iPAddress, 7777);

            Connector connector = new Connector();

            Console.WriteLine("Enter Your Name");
            string PlayerName = Console.ReadLine();

            connector.Init(endPoint, () => { return new ServerSession() { PlayerName = PlayerName}; } );

            while (true)
            {
                ;
            }
        }
    }
}