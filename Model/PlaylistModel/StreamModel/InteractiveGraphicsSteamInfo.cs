using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel
{
    public class InteractiveGraphicsSteamInfo : StreamInfo
    {
        public InteractiveGraphicsSteamInfo(MemoryStream ms) : base(PlaylistModel.StreamType.InteractiveGraphics)
        {
            readStreamAttribute(ms);

            //StreamAttribute
            byte[] attributeBuffer = ms.ReadFixedLengthByteArray(ms.ReadInt8());
            MemoryStream ms2 = new MemoryStream(attributeBuffer);
            int t = ms2.ReadInt8();
            if (t != 0x91)
                throw new NotImplementedException("IG Codec " + t);

            LanguageCode = ms2.ReadFixedLengthString(3);
        }

        public string LanguageCode { get; private set; }
    }
}
