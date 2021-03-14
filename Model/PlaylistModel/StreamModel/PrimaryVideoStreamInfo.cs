using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel
{
    public class PrimaryVideoStreamInfo : StreamInfo
    {
        public PrimaryVideoStreamInfo(MemoryStream ms ) : base(StreamType.PrimaryVideo)
        {
            readStreamAttribute(ms);

            //StreamAttribute
            byte[] attributeBuffer = ms.ReadFixedLengthByteArray(ms.ReadInt8());
            MemoryStream ms2 = new MemoryStream(attributeBuffer);
            VideoCodec = (StreamModel.VideoCodec) ms2.ReadInt8();
            int value = ms2.ReadInt8();
            VideoResolution = (VideoResolution) (value >> 4);
            FrameRate frameRate = (FrameRate) (value & 0x0f);
            switch (frameRate)
            {
                case StreamModel.FrameRate._23_076: FrameRate = 23.976; break;
                case StreamModel.FrameRate._24: FrameRate = 24; break;
                case StreamModel.FrameRate._25: FrameRate = 25; break;
                case StreamModel.FrameRate._29_976: FrameRate = 29.976; break;
                case StreamModel.FrameRate._50: FrameRate = 50; break;
                case StreamModel.FrameRate._59_94: FrameRate = 59.94; break;
                default: throw new NotImplementedException(frameRate.ToString());
            }
            ms2.Position += 3;
        }
        
        public VideoCodec VideoCodec { get; private set; }
        
        public VideoResolution VideoResolution { get; private set; }

        public double FrameRate { get; private set; }

        public override string ToString()
        {
            return $"{nameof(VideoCodec)}: {VideoCodec}, {nameof(VideoResolution)}: {VideoResolution}, {nameof(FrameRate)}: {FrameRate}";
        }
    }
}
