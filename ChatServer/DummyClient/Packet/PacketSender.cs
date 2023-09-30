using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    internal class PacketSender
    {
        static PacketSender _instance = new PacketSender();
        public static PacketSender Instance { get { return _instance; } }

        public void SendEnterPacket(ServerSession session)
        {
            Enter enter = new Enter();
            enter.PlayerName = session.PlayerName;
            session.Send(enter.Write());

            SendBroadCastSendPacket(session);
        }

        public void SendLeavePacket(ServerSession session)
        {
            Leave leave = new Leave();
            leave.PlayerName = session.PlayerName;
            session.Send(leave.Write());

            session.Disconnect();
        }

        public void SendBroadCastSendPacket(ServerSession session)
        {
            Console.WriteLine("Write Message");
            BroadCastSend send = new BroadCastSend();
            send.PlayerName = session.PlayerName;
            send.Message = Console.ReadLine();
            session.Send(send.Write());

            SendBroadCastSendPacket(session);
        }
    }
}
