using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class AtcSequence
    {
        internal AtcSequence()
        {

        }

        public int SpnAtcStart { get; internal set; }
        public byte NumStcSequences { get; internal set; }
        public byte OffsetStcId { get; internal set; }
        public StcSequence[] StcSequences { get; internal set; }
    }
}
