using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Room
    {
        static Room _instance = new Room();
        public static Room Instance { get { return _instance; } }

        public List<ClientSession> _sessions = new List<ClientSession>();

        int _sessionId = 0;
        object _lock = new object();

        public void SesssionEnter(ClientSession session)
        {
            lock (_lock)
            {
                session.SessionID = ++_sessionId;
                _sessions.Add(session);
            }
        }

        public void SesssionLeave(ClientSession session)
        {
            lock ( _lock)
            {
                _sessions.Remove(session);
            }
        }

        public void BroadCast(BroadCastSend packet, ClientSession session)
        {
            lock (_lock)
            {
                foreach (ClientSession s in _sessions)
                {
                    if (session.SessionID == s.SessionID)
                        continue;
                    s.Send(packet.Write());
                }
            }
        }
        public void BroadCastLeave(Leave packet)
        {
            lock (_lock)
            {
                foreach (ClientSession s in _sessions)
                {
                    s.Send(packet.Write());
                }
            }
        }
    }
}
