using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class CPI
    {
        public int Type { get; internal set; }
        public byte NumStreamPids { get; internal set; }
        public EpMapEntry[] Entries { get; internal set; }
    }
}
