using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using moe.yo3explorer.sharpBluRay.ComponentModel;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel
{
    [TypeConverter(typeof(MarkTypeConverter))]
    public class Mark
    {
        public MarkType MarkType { get; }
        public ushort PlayItemId { get; }
        public uint TimeStamp { get; }
        public ushort EntryEsPid { get; }
        public uint Duration { get; }
        public int Offset { get; }

        internal Mark(MarkType markType, ushort playItemId, uint timeStamp, ushort entryEsPid, uint duration, int inTime)
        {
            MarkType = markType;
            PlayItemId = playItemId;
            TimeStamp = timeStamp;
            EntryEsPid = entryEsPid;
            Duration = duration;
            Offset = inTime;
        }

        public long OffsetTimestamp => TimeStamp - Offset;

        public long TotalSeconds => OffsetTimestamp / 45000;

        public long TotalTenthSeconds => OffsetTimestamp / 4500;

        public long TotalHundrethSeconds => OffsetTimestamp / 450;

        public long TotalMilliseconds => OffsetTimestamp / 45;

        public long MillisecondsAfterTotalSeconds => TotalMilliseconds % 1000;

        public TimeSpan DotNetTimeStamp
        {
            get { return new TimeSpan(0, 0, 0, (int)TotalSeconds, (int)MillisecondsAfterTotalSeconds); }
        }

        public override string ToString()
        {
            return $"{nameof(DotNetTimeStamp)}: {DotNetTimeStamp}, {nameof(MarkType)}: {MarkType}, {nameof(PlayItemId)}: {PlayItemId}, {nameof(TimeStamp)}: {TimeStamp}";
        }
    }
}
