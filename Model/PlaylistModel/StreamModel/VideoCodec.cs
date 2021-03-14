using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel
{
    public enum VideoCodec : byte
    {
        MPEG1 = 0x01,
        MPEG2 = 0x02,
        MPEG4 = 0x1b,
        MVC = 0x20,
        HEVC = 0x24,
        VC1 = 0xea,
    }
}
