using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public enum PacketID
{
    Enter = 1,
	Leave = 2,
	BroadCastSend = 3,
	
}

public interface IPacket
{
	public ushort Protocol { get; }
	public void Read(ArraySegment<byte> sendBuff);
	public ArraySegment<byte> Write();
}


public class Enter
{
    public string PlayerName;
	

    public ushort Protocol { get { return (ushort)PacketID.Enter; } }

    public void Read(ArraySegment<byte> sendBuff)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
		ushort PlayerNameLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
		count += sizeof(ushort);
		this.PlayerName = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, PlayerNameLen);
		count += PlayerNameLen;
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        
		ushort PlayerNameLen = (ushort)Encoding.Unicode.GetByteCount(PlayerName);
		Array.Copy(BitConverter.GetBytes(PlayerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(Encoding.Unicode.GetBytes(PlayerName), 0, segment.Array, segment.Offset + count, PlayerNameLen);
		count += PlayerNameLen;
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);
        return sendBuff;
    }
}

public class Leave
{
    public string PlayerName;
	

    public ushort Protocol { get { return (ushort)PacketID.Leave; } }

    public void Read(ArraySegment<byte> sendBuff)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
		ushort PlayerNameLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
		count += sizeof(ushort);
		this.PlayerName = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, PlayerNameLen);
		count += PlayerNameLen;
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        
		ushort PlayerNameLen = (ushort)Encoding.Unicode.GetByteCount(PlayerName);
		Array.Copy(BitConverter.GetBytes(PlayerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(Encoding.Unicode.GetBytes(PlayerName), 0, segment.Array, segment.Offset + count, PlayerNameLen);
		count += PlayerNameLen;
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);
        return sendBuff;
    }
}

public class BroadCastSend
{
    public string PlayerName;
	public string Message;
	

    public ushort Protocol { get { return (ushort)PacketID.BroadCastSend; } }

    public void Read(ArraySegment<byte> sendBuff)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
		ushort PlayerNameLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
		count += sizeof(ushort);
		this.PlayerName = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, PlayerNameLen);
		count += PlayerNameLen;
		
		ushort MessageLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
		count += sizeof(ushort);
		this.Message = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, MessageLen);
		count += MessageLen;
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        
		ushort PlayerNameLen = (ushort)Encoding.Unicode.GetByteCount(PlayerName);
		Array.Copy(BitConverter.GetBytes(PlayerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(Encoding.Unicode.GetBytes(PlayerName), 0, segment.Array, segment.Offset + count, PlayerNameLen);
		count += PlayerNameLen;
		
		ushort MessageLen = (ushort)Encoding.Unicode.GetByteCount(Message);
		Array.Copy(BitConverter.GetBytes(MessageLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(Encoding.Unicode.GetBytes(Message), 0, segment.Array, segment.Offset + count, MessageLen);
		count += MessageLen;
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);
        return sendBuff;
    }
}

