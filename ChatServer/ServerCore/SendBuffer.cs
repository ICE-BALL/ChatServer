using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public static class SendBufferHelper
    {
        public static ThreadLocal<SendBuffer> _sendBuffer = new ThreadLocal<SendBuffer>(() => null);

        public static int ChunkSize { get; set; } = 65535;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (_sendBuffer.Value == null)
                _sendBuffer.Value = new SendBuffer(ChunkSize);
            if (_sendBuffer.Value.FreeSize < reserveSize)
                _sendBuffer.Value = new SendBuffer(reserveSize);

            return _sendBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return _sendBuffer.Value.Close(usedSize);
        }
    }

    public class SendBuffer
    {
        byte[] _buffer;
        int _usedSize;

        public SendBuffer(int ChunkSize)
        {
            _buffer = new byte[ChunkSize];
        }

        public int FreeSize { get { return  _buffer.Length - _usedSize; } }

        public ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > FreeSize)
                return null;

            ArraySegment<byte> buff = new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
            return buff;
        }

        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> buff = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize += usedSize;
            return buff;
        }
    }
}
