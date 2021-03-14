using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class AtcDelta
    {
        internal AtcDelta()
        {

        }

        public int Delta { get; internal set; }
        public string FileId { get; internal set; }
        public string FileCode { get; internal set; }
    }
}
