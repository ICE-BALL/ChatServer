
public class PlayerInfo
{
    public int PlayerId;
	public string PlayerName;
	
public class Skill
{
    public int Id;
	public string name;
	
public class 
{
    
	

    public void Read(ArraySegment<byte> s, ref ushort count)
    {
        
    }

    public void Write(ArraySegment<byte> s, ref ushort count)
    {
        
    }
}
public List<> s = new List<>();

	

    public void Read(ArraySegment<byte> s, ref ushort count)
    {
        
Array.Copy(BitConverter.GetBytes(Id), 0, segment.Array, segment.Offset + count, sizeof(int));
    count += sizeof(int);

ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(name);
    Array.Copy(BitConverter.GetBytes(nameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
    count += sizeof(ushort);
    Array.Copy(Encoding.Unicode.GetBytes(name), 0, segment.Array, segment.Offset + count, nameLen);
    count += nameLen;

Array.Copy(BitConverter.GetBytes(s.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
    count += sizeof(ushort);
    foreach (  in s)
        .Write(segment, ref count);

    }

    public void Write(ArraySegment<byte> s, ref ushort count)
    {
        
Array.Copy(BitConverter.GetBytes(Id), 0, segment.Array, segment.Offset + count, sizeof(int));
    count += sizeof(int);

ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(name);
    Array.Copy(BitConverter.GetBytes(nameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
    count += sizeof(ushort);
    Array.Copy(Encoding.Unicode.GetBytes(name), 0, segment.Array, segment.Offset + count, nameLen);
    count += nameLen;

Array.Copy(BitConverter.GetBytes(s.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
    count += sizeof(ushort);
    foreach (  in s)
        .Write(segment, ref count);

    }
}
public List<Skill> skills = new List<Skill>();

	

    public ushort Protocol { get { return (ushort)PacketID.PlayerInfo; } }

    public void Read(ArraySegment<byte> sendBuff)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
this.PlayerId = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
    count += sizeof(int);

ushort PlayerNameLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
    count += sizeof(ushort);
    this.PlayerName = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, PlayerNameLen);
    count += PlayerNameLen;

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

        
Array.Copy(BitConverter.GetBytes(PlayerId), 0, segment.Array, segment.Offset + count, sizeof(int));
    count += sizeof(int);

ushort PlayerNameLen = (ushort)Encoding.Unicode.GetByteCount(PlayerName);
    Array.Copy(BitConverter.GetBytes(PlayerNameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
    count += sizeof(ushort);
    Array.Copy(Encoding.Unicode.GetBytes(PlayerName), 0, segment.Array, segment.Offset + count, PlayerNameLen);
    count += PlayerNameLen;

Array.Copy(BitConverter.GetBytes(skills.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
    count += sizeof(ushort);
    foreach (Skill skill in skills)
        skill.Write(segment, ref count);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);
        return sendBuff;
    }
}
