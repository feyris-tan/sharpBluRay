using System;
using System.Collections.Generic;
using System.Text;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel
{
    public class Mark
    {
        public MarkType MarkType { get; }
        public ushort PlayItemId { get; }
        public uint TimeStamp { get; }
        public ushort EntryEsPid { get; }
        public uint Duration { get; }

        internal Mark(MarkType markType, ushort playItemId, uint timeStamp, ushort entryEsPid, uint duration)
        {
            MarkType = markType;
            PlayItemId = playItemId;
            TimeStamp = timeStamp;
            EntryEsPid = entryEsPid;
            Duration = duration;
        }
    }
}
