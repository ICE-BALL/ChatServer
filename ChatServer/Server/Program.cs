using ServerCore;
using System.Net;

namespace Server
{
    internal class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            IPAddress IpAddr = IPAddress.Parse("192.168.251.230");
            IPEndPoint endPoint = new IPEndPoint(IpAddr, 7777);

            _listener.Init(endPoint, () => { return new ClientSession(); });

            Console.WriteLine("Listening ...");
            while (true)
            {
                ;
            }
        }
    }
}