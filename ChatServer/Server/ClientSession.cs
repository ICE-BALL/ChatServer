using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class ClientSession : PacketSession
    {
        public int SessionID { get; set; }
        public string PlayerName { get; set; }
        public Room _Room { get; set; } = Program._Room;

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Connected to {endPoint}");

            _Room.SesssionEnter(this);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"Disconnected {endPoint}");
            _Room.SesssionLeave(this);
            PacketSender.Instance.SendLeavePacket(this);

        }

        public override void OnRecvPacket(ArraySegment<byte> sendBuff)
        {
            PacketHandler.Instance.SerchPacket(sendBuff, this);
        }

        public override void OnSend(int sendBytes)
        {
            Console.WriteLine($"Sended {sendBytes} bytes");
        }
    }
}
