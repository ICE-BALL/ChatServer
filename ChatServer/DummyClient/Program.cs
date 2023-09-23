using ServerCore;
using System.Net;

namespace DummyClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAddress IpAddr = IPAddress.Parse("192.168.251.230");
            IPEndPoint endPoint = new IPEndPoint(IpAddr, 7777);

            Connector connector = new Connector();

            connector.Init(endPoint, () => { return new ServerSession(); });

            while (true)
            {
                ;
            }
        }
    }
}