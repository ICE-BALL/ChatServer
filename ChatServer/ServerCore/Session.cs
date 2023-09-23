using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public abstract class PacketSession : Session
    {
        public static readonly ushort HeaderSize = 2;

        public sealed override int OnRecv(ArraySegment<byte> sendBuff)
        {
            int processLen = 0;
            while (true) 
            {
                if (sendBuff.Count < HeaderSize)
                    break;
                ushort size = BitConverter.ToUInt16(sendBuff.Array, sendBuff.Offset);
                if (size > sendBuff.Count || size <= 0)
                    break;

                ArraySegment<byte> buffer = new ArraySegment<byte>(sendBuff.Array, sendBuff.Offset, size);
                processLen += size;
                OnRecvPacket(buffer);

                sendBuff = new ArraySegment<byte>(sendBuff.Array, sendBuff.Offset + size, size);
            }

            return processLen;
        }

        public abstract void OnRecvPacket(ArraySegment<byte> sendBuff);
    }

    public abstract class Session
    {
        Socket _socket;
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();

        RecvBuffer _recvBuffer = new RecvBuffer(4096);

        int _disconnected = 0;
        object _lock = new object();
        List<ArraySegment<byte>> _sendList = new List<ArraySegment<byte>>();
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();

        public void Start(Socket socket)
        {
            _socket = socket;

            _recvArgs.Completed += OnRecvCompleted;
            _sendArgs.Completed += OnSendCompleted;

            RegisterRecv();
        }

        public void Send(ArraySegment<byte> buffer)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(buffer);

                if (_sendList.Count == 0)
                    RegisterSend();
            }
        }

        void RegisterSend()
        {
            while (_sendQueue.Count > 0)
            {
                _sendList.Add(_sendQueue.Dequeue());
            }
            _sendArgs.BufferList = _sendList;

            bool pending = _socket.SendAsync(_sendArgs);
            if (pending == false)
                OnSendCompleted(null, _sendArgs);
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock ( _lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    _sendList.Clear();
                    args.BufferList = null;

                    OnSend(args.BytesTransferred);

                    if (_sendQueue.Count > 0)
                    {
                        RegisterSend();
                    }
                }
                else
                {
                    Console.WriteLine($"Send Error : {args.BytesTransferred}, {args.SocketError}");
                    Disconnect();
                }
            }
        }

        void RegisterRecv()
        {
            _recvBuffer.Clean();

            ArraySegment<byte> readBuffer = _recvBuffer.WriteSegment;
            _recvArgs.SetBuffer(new ArraySegment<byte>(readBuffer.Array, readBuffer.Offset, readBuffer.Count));

            bool pending = _socket.ReceiveAsync(_recvArgs);
            if (pending == false)
                OnRecvCompleted(null, _recvArgs);
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                {
                    Disconnect();
                    return;
                }
                int processLen = 0;
                processLen = OnRecv(_recvBuffer.ReadSegment);
                if (processLen < 0 && _recvBuffer.FreeSize < processLen)
                {
                    Disconnect();
                    return;
                }

                if (_recvBuffer.OnRead(processLen) == false)
                {
                    Disconnect();
                    return;
                }

                RegisterRecv();
            }
            else
            {
                Console.WriteLine($"Recv Error : {args.BytesTransferred}, {args.SocketError}");
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;

            OnDisconnected(_socket.RemoteEndPoint);
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnDisconnected(EndPoint endPoint);
        public abstract int OnRecv(ArraySegment<byte> sendBuff);
        public abstract void OnSend(int sendBytes);
    }
}
