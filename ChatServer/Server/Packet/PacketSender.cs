using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class PacketSender
    {
        static PacketSender _instance = new PacketSender();
        public static PacketSender Instance { get { return _instance; } }

        public void SendBroadCastSendPacket(ClientSession session, string messgae)
        {
            BroadCastSend send = new BroadCastSend();
            send.PlayerName = session.PlayerName;
            send.Message = messgae;
            session._Room.BroadCast(send, session);
        }

        public void SendLeavePacket(ClientSession session)
        {
            Leave leave = new Leave();
            leave.PlayerName = session.PlayerName;
            //session.Send(leave.Write());
            session._Room.BroadCastLeave(leave);
        }
    }
}
