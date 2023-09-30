using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace PacketGenerator
{
    public class Program
    {
        static string _fileFormat = "";
        static string _packetFormat = "";

        static string _packetEnums = "";
        static int _packetNum = 0;

        static void Main(string[] args)
        {
            string PDL_Path;
            if (args.Length >= 1)
                PDL_Path = args[0];
            else
                 PDL_Path = "../PDL.xml";
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };
            

            using (XmlReader r = XmlReader.Create(PDL_Path, settings))
            {
                r.MoveToContent();

                while (r.Read() && r.NodeType != XmlNodeType.EndElement)
                {
                    ParsePacket(r);
                }
            }
            _fileFormat += string.Format(PacketFormat.fileFormat, _packetEnums, _packetFormat);

            File.WriteAllText("GenPackets.cs", _fileFormat);
        }

        static void ParsePacket(XmlReader r)
        {
            string PacketName = "";
            string Member = "";
            string Read = "";
            string Write = "";

            PacketName = r["name"];
            Console.WriteLine(PacketName);
            _packetNum = ++_packetNum;
            _packetEnums += string.Format(PacketFormat.packetIdFormat, PacketName, _packetNum);
            Tuple<string, string, string> t = ParseMembers(r);
            Member += t.Item1;
            Read += t.Item2;
            Write += t.Item3;

            _packetEnums += '\t';
            _packetFormat += string.Format(PacketFormat.packetFormat, PacketName, Member, Read, Write);
            return;
        }

        static Tuple<string, string, string> ParseMembers(XmlReader r)
        {
            string memberCode = "";
            string readCode = "";
            string writeCode = "";

            int depth = r.Depth + 1;
            while (r.Read())
            {
                if (r.Depth != depth)
                    break;

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
                        memberCode += t.Item1;
                        readCode += t.Item2;
                        writeCode += t.Item3;
                        break;
                }
                memberCode += Environment.NewLine;

            }
            memberCode = memberCode.Replace("\n", "\n\t");
            readCode = readCode.Replace("\n", "\n\t\t");
            writeCode = writeCode.Replace("\n", "\n\t\t");
            return new Tuple<string, string, string>(memberCode, readCode, writeCode);
        }

        /// <summary>
        /// 
        /// </summary>
        static Tuple<string, string, string> ParseList(XmlReader r)
        {
            string memberName = r["name"];

            string memberCode = "";
            string readCode = "";
            string writeCode = "";

            Tuple<string, string, string> t = ParseMembers(r);
            memberCode += string.Format(PacketFormat.memberListFormat, FirstCharToUpper(memberName), t.Item1, t.Item2, t.Item3, FirstCharToLower(memberName));
            readCode += string.Format(PacketFormat.readListFormat, FirstCharToUpper(memberName), FirstCharToLower(memberName));
            writeCode += string.Format(PacketFormat.writeListFormat, FirstCharToUpper(memberName), FirstCharToLower(memberName));

            return new Tuple<string, string, string>(memberCode, readCode, writeCode);
        }
        /// <summary>
        /// 
        /// </summary>

        static string ToMemberType(string type)
        {
            switch (type)
            {
                case "bool":
                    return "ToBoolean";
                case "short":
                    return "ToInt16";
                case "ushort":
                    return "ToUInt16";
                case "int":
                    return "ToInt32";
                case "long":
                    return "ToInt64";
                case "float":
                    return "ToSingle";
                case "double":
                    return "ToDouble";
                default:
                    return "";
            }
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