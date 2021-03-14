using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel
{
    public class AngleClipInfo
    {
        public int Id { get; }
        public string ClipName { get; }
        public string CodecId { get; }
        public byte StcId { get; }

        public AngleClipInfo(int id, string clipName, string codecId, byte stcId)
        {
            Id = id;
            ClipName = clipName;
            CodecId = codecId;
            StcId = stcId;
        }
    }
}
