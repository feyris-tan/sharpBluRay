using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel
{
    public abstract class StreamInfo
    {
        public StreamType StreamType { get; }

        protected StreamInfo(StreamType streamType)
        {
            StreamType = streamType;
        }

        public byte SubClipEntryId { get; private set; }

        public byte SubPathId { get; private set; }

        public StreamEntryType StreamEntryType { get; private set; }

        public ushort StreamPIDOfMainClip { get; private set; }

        protected void readStreamAttribute(MemoryStream ms)
        {
            //StreamEntry
            byte[] entryBuffer = ms.ReadFixedLengthByteArray(ms.ReadInt8());
            MemoryStream ms2 = new MemoryStream(entryBuffer);
            StreamEntryType = (StreamEntryType)ms2.ReadInt8();
            switch (StreamEntryType)
            {
                case StreamEntryType.STREAM_FOR_PLAYITEM:
                    StreamPIDOfMainClip = ms2.ReadUInt16();
                    ms2.Position += 6;
                    break;
                case StreamEntryType.STREAM_FOR_SUBPATH:
                    SubPathId = ms2.ReadInt8();
                    SubClipEntryId = ms2.ReadInt8();
                    StreamPIDOfMainClip = ms2.ReadUInt16();
                    ms2.Position += 4;
                    break;
                case StreamEntryType.STREAM_FOR_IN_MUX_SUBPATH:
                    SubPathId = ms2.ReadInt8();
                    StreamPIDOfMainClip = ms2.ReadUInt16();
                    ms2.Position += 5;
                    break;
                default:
                    throw new NotImplementedException(StreamEntryType.ToString());
            }
        }
    }
}
