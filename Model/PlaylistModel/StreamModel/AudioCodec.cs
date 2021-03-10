using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel
{
    enum AudioCodec : byte
    {
        LPCM = 0x80,
        DOLBY_DIGITAL = 0x81,
        DTS = 0x82,
        DOLBY_LOSSLESS = 0x83,
        DOLBY_DIGITAL_PLUS = 0x84,
        DTS_HD = 0x85,
        DTS_HD_XLL = 0x86,
        DOLBY_DIGITAL_PLUS_SECONDARY = 0xA1,
        DTS_HD_SECONDARY = 0xA2

    }
}
