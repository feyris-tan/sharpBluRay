using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel
{
    public enum StreamEntryType : byte
    {
        RESERVED,
        STREAM_FOR_PLAYITEM,
        STREAM_FOR_SUBPATH,
        STREAM_FOR_IN_MUX_SUBPATH
    }
}
