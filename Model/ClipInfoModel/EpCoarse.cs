using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class EpCoarse
    {
        public uint RefEpFineId { get; internal set; }
        public ushort PtsEp { get; internal set; }
        public uint SpnEp { get; set; }
    }
}
