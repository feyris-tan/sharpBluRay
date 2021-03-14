using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel
{
    public class AppInfoPlaylist
    {
        internal AppInfoPlaylist(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer, false);
            int length = ms.ReadInt32BE();
            int assumedLength = buffer.Length - 4;
            Debug.WriteLineIf(length != assumedLength, String.Format("AppInfoPlaylist Length is {0}, expected {1}", length, assumedLength));
            byte reserved = ms.ReadInt8();

            int type = ms.ReadInt8();
            PlaybackType = (PlaybackType) type;
            PlaybackCount = ms.ReadUInt16BE();
            UOMaskTable = new UserOperationMaskTable(ms.ReadFixedLengthByteArray(8));

            byte b = ms.ReadInt8();
            ms.Position++;
            PlayListRandomAccessFlags = (b & 80) != 0;
            AudioMixAppFlag = (b & 40) != 0;
            LosslessFlag = (b & 20) != 0;
        }

        public bool LosslessFlag { get; private set; }

        public bool AudioMixAppFlag { get; private set; }

        public bool PlayListRandomAccessFlags { get; private set; }

        public PlaybackType PlaybackType { get; private set; }
        public ushort PlaybackCount { get; private set; }
        public UserOperationMaskTable UOMaskTable { get; private set; }
    }
}
