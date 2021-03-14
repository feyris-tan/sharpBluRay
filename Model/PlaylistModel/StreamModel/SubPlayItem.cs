using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel
{
    public class SubPlayItem
    {
        public SubPlayItem(byte[] readFixedLengthByteArray)
        {
            MemoryStream ms = new MemoryStream(readFixedLengthByteArray, false);
            FileName = ms.ReadFixedLengthString(5);
            CodecId = ms.ReadFixedLengthString(4);

            ms.Position += 3;
            byte b = ms.ReadInt8();
            SpConnectionCondition = b & 0x1e;
            IsMultiClipEntries = (b & 0x01) != 0;
            StdId = ms.ReadInt8();
            SubPlayItemInTime = ms.ReadUInt32BE();
            SubPlayItemOutTime = ms.ReadUInt32BE();
            SyncPlayItemId = ms.ReadUInt16BE();
            SyncStartPtsOfPlayItem = ms.ReadInt32BE();

            _clipInfos = new List<AngleClipInfo>();
            _clipInfos.Add(new AngleClipInfo(0, FileName, CodecId, StdId));
            if (IsMultiClipEntries)
            {
                int length = ms.ReadInt8();
                ms.Position++;
                for (int i = 0; i < length; i++)
                {
                    FileName = ms.ReadFixedLengthString(5);
                    CodecId = ms.ReadFixedLengthString(4);
                    StdId = ms.ReadInt8();
                    _clipInfos.Add(new AngleClipInfo(i + 1, FileName, CodecId, StdId));
                }
            }
        }

        private List<AngleClipInfo> _clipInfos;
        public ReadOnlyCollection<AngleClipInfo> SubClipEntires { get { return new ReadOnlyCollection<AngleClipInfo>(_clipInfos); } }

        public int SyncStartPtsOfPlayItem { get; private set; }

        public ushort SyncPlayItemId { get; private set; }

        public uint SubPlayItemOutTime { get; private set; }

        public uint SubPlayItemInTime { get; private set; }

        public byte StdId { get; private set; }

        public bool IsMultiClipEntries { get; private set; }

        public int SpConnectionCondition { get; private set; }

        public string CodecId { get; private set; }

        public string FileName { get; private set; }
    }
}
