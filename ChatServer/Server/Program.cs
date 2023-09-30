using ServerCore;
using System.Net;

namespace Server
{
    internal class Program
    {
        public static Room _Room = new Room();
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry IPhost = Dns.GetHostEntry(host);
            IPAddress iPAddress = IPhost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(iPAddress, 7777);

            _listener.Init(endPoint, () => { return new ClientSession(); });

            Console.WriteLine("Listening ...");
            while (true)
            {
                ;
            }
        }
    }
}