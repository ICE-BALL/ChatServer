using System.Reflection;
using System.Xml;

namespace PacketGenerator
{
    public class Program
    {
        static string _packetFormat = "";

        static void Main(string[] args)
        {
            string PDL_Path = "../PDL.xml";
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };
            using (XmlReader r = XmlReader.Create(PDL_Path, settings))
            {
                _packetFormat += ParseXml(r);
                File.WriteAllText("GenPackets.cs", _packetFormat);
            }
        }

        static string ParseXml(XmlReader r)
        {
            string PacketName = "";
            string Member = "";
            string Read = "";
            string Write = "";

            int depth = 1;
            while (r.Read()) 
            {
                r.MoveToContent();
                if (depth > r.Depth)
                    continue;

                PacketName = r["name"];
                Tuple<string, string, string> t = ParseMembers(r);
                Member += t.Item1;
                Read += t.Item2;
                Write += t.Item3;
            }

            return string.Format(PacketFormat.packetFormat, PacketName, Member, Read, Write);
        }

        static Tuple<string, string, string> ParseMembers(XmlReader r)
        {
            string memberCode = "";
            string readCode = "";
            string writeCode = "";

            while (r.Read())
            {
                if (r.Name == "packet")
                    continue;

                string memberType = r.Name;
                string memberName = r["name"];
                switch (memberType)
                {
                    case "float":
                    case "double":
                    case "short":
                    case "ushort":
                    case "int":
                    case "uing":
                    case "long":
                    case "ulong":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                        readCode += string.Format(PacketFormat.readFormat, memberName, ToMemberType(memberType), memberType);
                        writeCode += string.Format(PacketFormat.writeFormat, memberName, memberType);
                        break;
                    case "string":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                        readCode += string.Format(PacketFormat.readStringFormat, memberName);
                        writeCode += string.Format(PacketFormat.writeStringFormat, memberName);
                        break;
                    case "list":
                        Tuple<string, string, string> t = ParseList(r);
                        memberCode += string.Format(PacketFormat.memberListFormat, FirstCharToUpper(memberName), t.Item1, t.Item2, t.Item3, FirstCharToLower(memberName));
                        readCode += string.Format(PacketFormat.readListFormat, FirstCharToUpper(memberName), FirstCharToLower(memberName));
                        writeCode += string.Format(PacketFormat.writeListFormat, FirstCharToUpper(memberName), FirstCharToLower(memberName));
                        break;
                }
                memberCode += Environment.NewLine;
                memberCode += "\t";
            }
            return new Tuple<string, string, string>(memberCode, readCode, writeCode);
        }

        /// <summary>
        /// 
        /// </summary>
        static Tuple<string, string, string> ParseList(XmlReader r)
        {
            string memberCode = "";
            string readCode = "";
            string writeCode = "";

            Tuple<string, string, string> tuple = ParseMembers(r);
            memberCode += tuple.Item1;
            readCode += tuple.Item3;
            writeCode += tuple.Item3;
            

            return new Tuple<string, string, string>(memberCode, readCode, writeCode);
        }
        /// <summary>
        /// 
        /// </summary>

        static string ToMemberType(string type)
        {
            switch (type)
            {
                case "short":
                    return "ToInt16";
                case "ushort":
                    return "ToUInt16";
                case "int":
                    return "ToInt32";
                case "uint":
                    return "ToUInt32";
                case "long":
                    return "ToInt64";
                case "ulong":
                    return "ToUInt64";
            }

            return null;
        }

        static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            return string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
        }

        static string FirstCharToLower(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            return string.Concat(input[0].ToString().ToLower(), input.AsSpan(1));
        }
    }
}