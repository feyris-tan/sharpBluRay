using System;
using System.IO;

namespace moe.yo3explorer.sharpBluRay.Model.IndexModel
{
    public class Title
    {
        internal Title(Stream stream)
        {
            byte b = stream.ReadInt8();
            stream.Position += 3;

            TitleAccessType = ((b & 0x30) >> 4);
            int type = ((b & 0xc0) >> 6);
            if (type == 1)
            {
                //Movie Object
                IndexObject = new MovieIndexObject(stream);
            }
            else
            {
                //BDJ Object
                throw new NotImplementedException();
            }
        }

        public int TitleAccessType { get; private set; }
        public IndexObject IndexObject { get; private set; }

        public override string ToString()
        {
            return $"{nameof(TitleAccessType)}: {TitleAccessType}, {nameof(IndexObject)}: {IndexObject}";
        }
    }
}
