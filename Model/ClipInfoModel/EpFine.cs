using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class EpFine
    {
        public bool IsAngleChangePoint { get; internal set; }
        public byte IEndPositionOffset { get; internal set; }
        public ushort PtsEp { get; internal set; }
        public uint SpnEp { get; internal set; }
    }
}
