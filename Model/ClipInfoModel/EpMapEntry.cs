using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class EpMapEntry
    {
        public ushort PID { get; internal set; }
        public byte EpStreamType { get; internal set; }
        public ushort NumEpCoarse { get; internal set; }
        public uint NumEpFine { get; internal set; }
        public uint EpMapStreamStarAddr { get; internal set; }
        public EpCoarse[] EpCoarse { get; internal set; }
        public EpFine[] EpFine { get; internal set; }
    }
}
