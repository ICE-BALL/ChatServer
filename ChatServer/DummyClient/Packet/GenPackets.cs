using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public enum PacketID
{
    SendHello,
}

public class SendHello
{
    public int Id;
    public string name;

    public class Skill
    {
        public int S_Id;
        public string S_name;

        public void Read(ArraySegment<byte> s, ref ushort count)
        {
            this.S_Id = BitConverter.ToInt32(s.Array, s.Offset + count);
            count += sizeof(int);
            ushort nameLen = BitConverter.ToUInt16(s.Array, s.Offset + count);
            count += sizeof(ushort);
            this.S_name = Encoding.Unicode.GetString(s.Array, s.Offset + count, nameLen);
            count += nameLen;
        }

        public void Write(ArraySegment<byte> s, ref ushort count)
        {
            Array.Copy(BitConverter.GetBytes(S_Id), 0, s.Array, s.Offset + count, sizeof(int));
            count += sizeof(int);
            ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(S_name);
            Array.Copy(BitConverter.GetBytes(nameLen), 0, s.Array, s.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(Encoding.Unicode.GetBytes(S_name), 0, s.Array, s.Offset + count, nameLen);
            count += nameLen;
        }
    }
    public List<Skill> skills = new List<Skill>();

    public ushort Protocol { get { return (ushort)PacketID.SendHello; } }

    public void Read(ArraySegment<byte> sendBuff)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.Id = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
        count += sizeof(int);
        ushort nameLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
        count += sizeof(ushort);
        this.name = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, nameLen);
        count += nameLen;
        ushort skillLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
        count += sizeof(ushort);
        for (int i = 0; i < skillLen; i++) 
        {
            Skill skill = new Skill();
            skill.Read(sendBuff, ref count);
            this.skills.Add(skill);
        }
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Id), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(name);
        Array.Copy(BitConverter.GetBytes(nameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(Encoding.Unicode.GetBytes(name), 0, segment.Array, segment.Offset + count, nameLen);
        count += nameLen;
        Array.Copy(BitConverter.GetBytes(skills.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        foreach (Skill skill in skills)
            skill.Write(segment, ref count);

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);
        return sendBuff;
    }
}