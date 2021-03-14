using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.Languages;

namespace moe.yo3explorer.sharpBluRay
{
    static class Extensions
    {
        [DebuggerStepThrough]
        public static string ReadFixedLengthString(this Stream s, int length)
        {
            byte[] buffer = new byte[length];
            int actuallyRead;
            if ((actuallyRead = s.Read(buffer, 0, length)) != length)
                throw new EndOfStreamException(String.Format("Expected to read {0} bytes, got {1}",length,actuallyRead));
            return Encoding.ASCII.GetString(buffer);
        }

        [DebuggerStepThrough]
        public static int ReadInt32BE(this Stream s)
        {
            byte[] bufferLE = new byte[4];
            if (s.Read(bufferLE, 0, 4) != 4)
                throw new EndOfStreamException("Failed to read 4 bytes!");
            byte[] bufferBE = new byte[4] {bufferLE[3], bufferLE[2], bufferLE[1], bufferLE[0]};
            return BitConverter.ToInt32(bufferBE, 0);
        }
        
        [DebuggerStepThrough]
        public static uint ReadUInt32BE(this Stream s)
        {
            byte[] bufferLE = new byte[4];
            if (s.Read(bufferLE, 0, 4) != 4)
                throw new EndOfStreamException("Failed to read 4 bytes!");
            byte[] bufferBE = new byte[4] { bufferLE[3], bufferLE[2], bufferLE[1], bufferLE[0] };
            return BitConverter.ToUInt32(bufferBE, 0);
        }
        
        [DebuggerStepThrough]
        public static ulong ReadUInt64BE(this Stream s)
        {
            byte[] bufferLE = new byte[8];
            if (s.Read(bufferLE, 0, 8) != 8)
                throw new EndOfStreamException("Failed to read 8 bytes!");
            byte[] bufferBE = new byte[8] { bufferLE[7], bufferLE[6], bufferLE[5], bufferLE[4], bufferLE[3], bufferLE[2], bufferLE[1], bufferLE[0] };
            return BitConverter.ToUInt64(bufferBE, 0);
        }

        [DebuggerStepThrough]
        public static void ReadFully(this Stream s, byte[] fillMe)
        {
            if (s.Read(fillMe, 0, fillMe.Length) != fillMe.Length)
                throw new EndOfStreamException(String.Format("Failed to read {0} bytes.", fillMe.Length));
        }

        [DebuggerStepThrough]
        public static byte[] ReadFixedLengthByteArray(this Stream s, int length)
        {
            byte[] result = new byte[length];
            int actualLength;
            if ((actualLength = s.Read(result,0,length)) != length)
                throw new EndOfStreamException(String.Format("Expected to read {0} bytes, got {1}", length, actualLength));
            return result;
        }

        [DebuggerStepThrough]
        public static byte ReadInt8(this Stream s)
        {
            int readByte = s.ReadByte();
            if (readByte == -1)
                throw new EndOfStreamException("Failed to read 1 byte");
            return (byte) readByte;
        }

        [DebuggerStepThrough]
        public static ushort ReadUInt16BE(this Stream s)
        {
            byte[] bufferLE = new byte[2];
            if (s.Read(bufferLE, 0, 2) != 2)
                throw new EndOfStreamException("Failed to read 2 bytes!");
            byte[] bufferBE = new byte[2] { bufferLE[1], bufferLE[0] };
            return BitConverter.ToUInt16(bufferBE, 0);
        }

        [DebuggerStepThrough]
        public static string DecodeLanguageCodeEndonyme(this string s)
        {
            return LanguageManager.GetLanguage(s).Endonym;
        }

        [DebuggerStepThrough]
        public static string DecodeLanguageCodeEnglish(this string s)
        {
            return LanguageManager.GetLanguage(s).IsoLanguageName;
        }
        
        [DebuggerStepThrough]
        public static ulong ReadUInt64LE(this Stream s)
        {
            byte[] bufferLE = new byte[8];
            if (s.Read(bufferLE, 0, 8) != 8)
                throw new EndOfStreamException("Failed to read 8 bytes!");
            return BitConverter.ToUInt64(bufferLE, 0);
        }

        [DebuggerStepThrough]
        public static int ReadInt32LE(this Stream s)
        {
            byte[] bufferLE = new byte[4];
            if (s.Read(bufferLE, 0, 4) != 4)
                throw new EndOfStreamException("Failed to read 4 bytes!");
            return BitConverter.ToInt32(bufferLE, 0);
        }
    }
}
