using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class StcSequence
    {
        internal  StcSequence() { }
        public ushort PcrPid { get; internal set; }
        public int SpnStcStart { get; internal set; }
        public int PresentationStartTime { get; internal set; }
        public int PresentationEndTime { get; internal set; }
    }
}
