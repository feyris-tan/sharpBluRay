using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel
{
    public class PresentationGraphicsStreamInfo : StreamInfo
    {
        public PresentationGraphicsStreamInfo(MemoryStream ms) : base(StreamType.PresentationGraphics)
        {
            readStreamAttribute(ms);

            //StreamAttribute
            byte[] attributeBuffer = ms.ReadFixedLengthByteArray(ms.ReadInt8());
            MemoryStream ms2 = new MemoryStream(attributeBuffer);
            byte streamCoding = ms2.ReadInt8();
            if (streamCoding != 0x90)
                throw new NotImplementedException(String.Format("PGS Codec {0}", streamCoding));
            LanguageCode = ms2.ReadFixedLengthString(3);
            byte reserved = ms2.ReadInt8();
        }

        public string LanguageCode { get; private set; }

        public override string ToString()
        {
            return $"{nameof(LanguageCode)}: {LanguageCode.DecodeLanguageCodeEndonyme()}";
        }
    }
}
