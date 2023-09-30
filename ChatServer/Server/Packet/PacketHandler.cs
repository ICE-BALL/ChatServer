using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class PacketHandler
    {
        static PacketHandler instance = new PacketHandler();
        public static PacketHandler Instance { get { return instance; } }

        public void SerchPacket(ArraySegment<byte> sendBuff, ClientSession session)
        {
            ushort count = 0;
            count += sizeof(ushort);
            ushort Id = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
            switch (Id)
            {
                case (ushort)PacketID.Enter:
                    Enter enter = new Enter();
                    enter.Read(sendBuff);
                    Enter(enter, session);
                    break;
                case (ushort)PacketID.Leave:
                    Leave leave = new Leave();
                    leave.Read(sendBuff);
                    Leave(leave, session);
                    break;
                case (ushort)PacketID.BroadCastSend:
                    BroadCastSend broadCastSend = new BroadCastSend();
                    broadCastSend.Read(sendBuff);
                    BroadCastSend(broadCastSend, session);
                    break;
            }
        }

        public void Enter(Enter packet, ClientSession session)
        {
            session.PlayerName = packet.PlayerName;
            PacketSender.Instance.SendBroadCastSendPacket(session, $"{session.PlayerName} entered");
        }

        public void Leave(Leave packet, ClientSession session)
        {
            session.PlayerName = packet.PlayerName;
            PacketSender.Instance.SendBroadCastSendPacket(session, $"{session.PlayerName} Leaved");
        }

        public void BroadCastSend(BroadCastSend packet, ClientSession session)
        {
            session._Room.BroadCast(packet, session);
        }
    }
}
