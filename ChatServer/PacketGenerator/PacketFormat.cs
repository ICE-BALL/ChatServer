using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    internal class PacketFormat
    {
        // {0} packet enum
        // {1} packets
        public static string fileFormat =
@"
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public enum PacketID
{{
    {0}
}}

{1}
";

        // {0} packet Name
        // {1} packet Number
        public static string packetIdFormat = 
@"{0} = {1},
";

        // {0} packet Name
        // {1} member
        // {2} Read
        // {3} Write
        public static string packetFormat =
@"
public class {0}
{{
    {1}

    public ushort Protocol {{ get {{ return (ushort)PacketID.{0}; }} }}

    public void Read(ArraySegment<byte> sendBuff)
    {{
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        {2}
    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        {3}

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);
        return sendBuff;
    }}
}}
";

        // {0} member Type
        // {1} member Name
        public static string memberFormat = 
@"public {0} {1};";


        // {0} List Name
        // {1} Member
        // {2} Read
        // {3} Write
        // {4} lowercase list Name
        public static string memberListFormat =
@"
public class {0}
{{
    {1}

    public void Read(ArraySegment<byte> s, ref ushort count)
    {{
        {2}
    }}

    public void Write(ArraySegment<byte> s, ref ushort count)
    {{
        {3}
    }}
}}
public List<{0}> {4}s = new List<{0}>();
";

        // {0} member Name
        // {1} To~
        // {2} memberType
        public static string readFormat =
@"
this.{0} = BitConverter.{1}(sendBuff.Array, sendBuff.Offset + count);
count += sizeof({2});
";

        // {0} string name
        public static string readStringFormat =
@"
ushort {0}Len = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
count += sizeof(ushort);
this.{0} = Encoding.Unicode.GetString(sendBuff.Array, sendBuff.Offset + count, {0}Len);
count += {0}Len;
";

        // {0} list Name
        // {1} lowercase list Name
        public static string readListFormat =
@"
ushort {1}Len = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset + count);
count += sizeof(ushort);
for (int i = 0; i < {1}Len; i++) 
{{
    {0} {1} = new {0}();
    {1}.Read(sendBuff, ref count);
    this.{1}s.Add({1});
}}
";

        // {0} member Name
        // {1} member Type
        public static string writeFormat =
@"
Array.Copy(BitConverter.GetBytes({0}), 0, segment.Array, segment.Offset + count, sizeof({1}));
count += sizeof({1});
";

        // {0} string name
        public static string writeStringFormat =
@"
ushort {0}Len = (ushort)Encoding.Unicode.GetByteCount({0});
Array.Copy(BitConverter.GetBytes({0}Len), 0, segment.Array, segment.Offset + count, sizeof(ushort));
count += sizeof(ushort);
Array.Copy(Encoding.Unicode.GetBytes({0}), 0, segment.Array, segment.Offset + count, {0}Len);
count += {0}Len;
";

        // {0} list Name
        // {1} lowercase list Name
        public static string writeListFormat =
@"
Array.Copy(BitConverter.GetBytes({1}s.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
count += sizeof(ushort);
foreach ({0} {1} in {1}s)
    {1}.Write(segment, ref count);
";
    }
}
