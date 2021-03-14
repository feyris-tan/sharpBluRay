using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public abstract class ProgramStream
    {
        internal ProgramStream() {}
        public ushort PID { get; internal set; }

        public abstract ProgramStreamType ProgramStreamType { get; }
    }
}
