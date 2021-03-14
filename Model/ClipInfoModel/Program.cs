using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class Program
    {
        internal Program() { }
        public int ProgramSequenceStart { get; internal set; }
        public ushort ProgramMapPid { get; internal set; }
        public byte NumStreams { get; internal set; }
        public byte NumGroups { get; internal set; }
        public ProgramStream[] ProgramStreams { get; internal set; }
    }
}
