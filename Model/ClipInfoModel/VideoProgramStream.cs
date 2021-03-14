using System;
using System.Collections.Generic;
using System.Text;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class VideoProgramStream : ProgramStream
    {
        public VideoResolution VideoResolution { get; internal set; }
        public double FrameRate { get; internal set; }
        public VideoCodec VideoCodec { get; internal set; }
        public override ProgramStreamType ProgramStreamType => ClipInfoModel.ProgramStreamType.VIDEO;
    }
}
