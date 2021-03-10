using System;
using System.Diagnostics;
using System.IO;

namespace moe.yo3explorer.sharpBluRay.Model.IndexModel
{
    public class Indices
    {
        internal Indices(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            int totalLen = ms.ReadInt32BE();
            int assumedLen = buffer.Length - 4;
            Debug.WriteLineIf(totalLen != assumedLen, "Unexpected Indices size!");
            FirstPlayback = ReadIndexObject(ms);
            TopMenu = ReadIndexObject(ms);

            ushort titles = ms.ReadUInt16();
            Titles = new Title[titles];
            for (int i = 0; i < titles; i++)
            {
                Titles[i] = new Title(ms);
            }
        }

        private IndexObject ReadIndexObject(Stream stream)
        {
            byte b = stream.ReadInt8();
            stream.Position += 3;

            int type = ((b & 0xc0) >> 6);
            if (type == 1)
            {
                //Movie Object
                return new MovieIndexObject(stream);
            }
            else
            {
                //BDJ Object
                throw new NotImplementedException();
            }
        }

        public IndexObject FirstPlayback { get; }
        public IndexObject TopMenu { get; }
        public Title[] Titles { get; }
    }
}
