using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    public class PacketHandler
    {
        static PacketHandler instance = new PacketHandler();
        public static PacketHandler Instance { get { return instance; } }

        public void SerchPacket(ArraySegment<byte> sendBuff)
        {
            ushort count = 0;
            count += sizeof(ushort);
            ushort Id = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
            switch (Id)
            {
                case (ushort)PacketID.Enter:
                    Enter enter = new Enter();
                    enter.Read(sendBuff);
                    Enter(enter);
                    break;
                case (ushort)PacketID.Leave:
                    Leave leave = new Leave();
                    leave.Read(sendBuff);
                    Leave(leave);
                    break;
                case (ushort)PacketID.BroadCastSend:
                    BroadCastSend broadCastSend = new BroadCastSend();
                    broadCastSend.Read(sendBuff);
                    BroadCastSend(broadCastSend);
                    break;
            }
        }

        public void Enter(Enter packet)
        {

        }

        public void Leave(Leave packet)
        {
            Console.WriteLine($"{packet.PlayerName} Leaved");
        }

        public void BroadCastSend(BroadCastSend packet)
        {
            Console.WriteLine($"From : {packet.PlayerName}, Message : {packet.Message}");
        }
    }
}
