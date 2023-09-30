using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    internal class ServerSession : PacketSession
    {
        public string PlayerName { get; set; }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Connected to {endPoint}");

            PacketSender.Instance.SendEnterPacket(this);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"Disconnected {endPoint}");

            PacketSender.Instance.SendLeavePacket(this);
        }

        public override void OnRecvPacket(ArraySegment<byte> sendBuff)
        {
            PacketHandler.Instance.SerchPacket(sendBuff);
        }

        public override void OnSend(int sendBytes)
        {
            Console.WriteLine($"Sended {sendBytes} bytes");
        }
    }
}
