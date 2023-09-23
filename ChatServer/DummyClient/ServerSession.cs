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
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Connected to {endPoint}");

            SendHello hello = new SendHello();
            hello.Id = 2;
            hello.name = "Client";
            hello.skills.Add(new SendHello.Skill() { S_Id = 5, S_name = "Dark" });
            hello.skills.Add(new SendHello.Skill() { S_Id = 6, S_name = "Light" });
            hello.skills.Add(new SendHello.Skill() { S_Id = 7, S_name = "Dragon" });
            hello.skills.Add(new SendHello.Skill() { S_Id = 8, S_name = "Ghost" });

            Send(hello.Write());
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"Disconnected {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> sendBuff)
        {
            ushort count = 0;
            count += sizeof(ushort);
            ushort Id = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
            switch ((PacketID)Id)
            {
                case PacketID.SendHello:
                    SendHello s = new SendHello();
                    s.Read(sendBuff);
                    Console.WriteLine($"ID : {s.Id}, Name : {s.name}");
                    foreach (SendHello.Skill skill in s.skills)
                    {
                        Console.WriteLine($"S_ID : {skill.S_Id}, S_Name : {skill.S_name}");
                    }
                    break;
            }
        }

        public override void OnSend(int sendBytes)
        {
            Console.WriteLine($"Sended {sendBytes} bytes");
        }
    }
}
