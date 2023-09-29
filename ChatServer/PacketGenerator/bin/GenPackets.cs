
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public enum PacketID
{
    PlayerInfo = 1,
	PlayerInfoRec = 2,
	PlayerSkillInfoRec = 3,
	
}


public class PlayerInfo
{
    public int PlayerId;
	public string PlayerName;
	public int Error;
	
	public class Skill
	{
	    public int Id;
		public string name;
		
	
	    public void Read(ArraySegment<byte> s, ref ushort count)
	    {
	        
			this.Id = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
			count += sizeof(int);
			
			ushort nameLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
			count += sizeof(ushort);
			this.name = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, nameLen);
			count += nameLen;
			
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
		
		this.Error = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
		count += sizeof(int);
		
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
		
		Array.Copy(BitConverter.GetBytes(Error), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		
		Array.Copy(BitConverter.GetBytes(skills.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Skill skill in skills)
		    skill.Write(segment, ref count);
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);
        return sendBuff;
    }
}

public class PlayerInfoRec
{
    public int PlayerId;
	public string PlayerName;
	
	public class Skill
	{
	    public int Id;
		public string name;
		
		public class SkillInfo
		{
		    public int Attack;
			public string Heal;
			
		
		    public void Read(ArraySegment<byte> s, ref ushort count)
		    {
		        
				this.Attack = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
				count += sizeof(int);
				
				ushort HealLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
				count += sizeof(ushort);
				this.Heal = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, HealLen);
				count += HealLen;
				
		    }
		
		    public void Write(ArraySegment<byte> s, ref ushort count)
		    {
		        
				Array.Copy(BitConverter.GetBytes(Attack), 0, segment.Array, segment.Offset + count, sizeof(int));
				count += sizeof(int);
				
				ushort HealLen = (ushort)Encoding.Unicode.GetByteCount(Heal);
				Array.Copy(BitConverter.GetBytes(HealLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
				count += sizeof(ushort);
				Array.Copy(Encoding.Unicode.GetBytes(Heal), 0, segment.Array, segment.Offset + count, HealLen);
				count += HealLen;
				
		    }
		}
		public List<SkillInfo> skillInfos = new List<SkillInfo>();
		
		
	
	    public void Read(ArraySegment<byte> s, ref ushort count)
	    {
	        
			this.Id = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
			count += sizeof(int);
			
			ushort nameLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
			count += sizeof(ushort);
			this.name = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, nameLen);
			count += nameLen;
			
			ushort skillInfoLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
			count += sizeof(ushort);
			for (int i = 0; i < skillInfoLen; i++) 
			{
			    SkillInfo skillInfo = new SkillInfo();
			    skillInfo.Read(sendBuff, ref count);
			    this.skillInfos.Add(skillInfo);
			}
			
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
			
			Array.Copy(BitConverter.GetBytes(skillInfos.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
			count += sizeof(ushort);
			foreach (SkillInfo skillInfo in skillInfos)
			    skillInfo.Write(segment, ref count);
			
	    }
	}
	public List<Skill> skills = new List<Skill>();
	
	

    public ushort Protocol { get { return (ushort)PacketID.PlayerInfoRec; } }

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

public class PlayerSkillInfoRec
{
    public int PlayerSkill;
	public string PlayerNameSkill;
	
	public class SkillId
	{
	    public int Id;
		public string name;
		
		public class SkillInfo
		{
		    public int Attack;
			public string Heal;
			
			public class SkillEnerge
			{
			    public int Damage;
				public float Duration;
				
			
			    public void Read(ArraySegment<byte> s, ref ushort count)
			    {
			        
					this.Damage = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
					count += sizeof(int);
					
					this.Duration = BitConverter.(sendBuff.Array, sendBuff.Offset + count);
					count += sizeof(float);
					
			    }
			
			    public void Write(ArraySegment<byte> s, ref ushort count)
			    {
			        
					Array.Copy(BitConverter.GetBytes(Damage), 0, segment.Array, segment.Offset + count, sizeof(int));
					count += sizeof(int);
					
					Array.Copy(BitConverter.GetBytes(Duration), 0, segment.Array, segment.Offset + count, sizeof(float));
					count += sizeof(float);
					
			    }
			}
			public List<SkillEnerge> skillEnerges = new List<SkillEnerge>();
			
			
		
		    public void Read(ArraySegment<byte> s, ref ushort count)
		    {
		        
				this.Attack = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
				count += sizeof(int);
				
				ushort HealLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
				count += sizeof(ushort);
				this.Heal = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, HealLen);
				count += HealLen;
				
				ushort skillEnergeLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
				count += sizeof(ushort);
				for (int i = 0; i < skillEnergeLen; i++) 
				{
				    SkillEnerge skillEnerge = new SkillEnerge();
				    skillEnerge.Read(sendBuff, ref count);
				    this.skillEnerges.Add(skillEnerge);
				}
				
		    }
		
		    public void Write(ArraySegment<byte> s, ref ushort count)
		    {
		        
				Array.Copy(BitConverter.GetBytes(Attack), 0, segment.Array, segment.Offset + count, sizeof(int));
				count += sizeof(int);
				
				ushort HealLen = (ushort)Encoding.Unicode.GetByteCount(Heal);
				Array.Copy(BitConverter.GetBytes(HealLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
				count += sizeof(ushort);
				Array.Copy(Encoding.Unicode.GetBytes(Heal), 0, segment.Array, segment.Offset + count, HealLen);
				count += HealLen;
				
				Array.Copy(BitConverter.GetBytes(skillEnerges.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
				count += sizeof(ushort);
				foreach (SkillEnerge skillEnerge in skillEnerges)
				    skillEnerge.Write(segment, ref count);
				
		    }
		}
		public List<SkillInfo> skillInfos = new List<SkillInfo>();
		
		
	
	    public void Read(ArraySegment<byte> s, ref ushort count)
	    {
	        
			this.Id = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
			count += sizeof(int);
			
			ushort nameLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
			count += sizeof(ushort);
			this.name = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, nameLen);
			count += nameLen;
			
			ushort skillInfoLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
			count += sizeof(ushort);
			for (int i = 0; i < skillInfoLen; i++) 
			{
			    SkillInfo skillInfo = new SkillInfo();
			    skillInfo.Read(sendBuff, ref count);
			    this.skillInfos.Add(skillInfo);
			}
			
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
			
			Array.Copy(BitConverter.GetBytes(skillInfos.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
			count += sizeof(ushort);
			foreach (SkillInfo skillInfo in skillInfos)
			    skillInfo.Write(segment, ref count);
			
	    }
	}
	public List<SkillId> skillIds = new List<SkillId>();
	
	

    public ushort Protocol { get { return (ushort)PacketID.PlayerSkillInfoRec; } }

    public void Read(ArraySegment<byte> sendBuff)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
		this.PlayerSkill = BitConverter.ToInt32(sendBuff.Array, sendBuff.Offset + count);
		count += sizeof(int);
		
		ushort PlayerNameSkillLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
		count += sizeof(ushort);
		this.PlayerNameSkill = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, PlayerNameSkillLen);
		count += PlayerNameSkillLen;
		
		ushort skillIdLen = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < skillIdLen; i++) 
		{
		    SkillId skillId = new SkillId();
		    skillId.Read(sendBuff, ref count);
		    this.skillIds.Add(skillId);
		}
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        
		Array.Copy(BitConverter.GetBytes(PlayerSkill), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		
		ushort PlayerNameSkillLen = (ushort)Encoding.Unicode.GetByteCount(PlayerNameSkill);
		Array.Copy(BitConverter.GetBytes(PlayerNameSkillLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(Encoding.Unicode.GetBytes(PlayerNameSkill), 0, segment.Array, segment.Offset + count, PlayerNameSkillLen);
		count += PlayerNameSkillLen;
		
		Array.Copy(BitConverter.GetBytes(skillIds.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (SkillId skillId in skillIds)
		    skillId.Write(segment, ref count);
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);
        return sendBuff;
    }
}

