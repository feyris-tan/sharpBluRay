using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel;

namespace moe.yo3explorer.sharpBluRay.Model
{
    public class Playlist
    {
        public Playlist(byte[] mplsBuffer)
        {
            MemoryStream ms = new MemoryStream(mplsBuffer, false);
            if (!ms.ReadFixedLengthString(4).Equals("MPLS"))
                throw new InvalidDataException("MPLS file is broken!");

            Version = int.Parse(ms.ReadFixedLengthString(4));

            int playlistStartAddress = ms.ReadInt32BE();
            int playlistMarkStartAddress = ms.ReadInt32BE();
            int extensionStartAddress = ms.ReadInt32BE();
            ms.Position += 20;

            int appInfoPlaylistLength = playlistStartAddress - (int)ms.Position;
            byte[] appInfoPlaylistBuffer = ms.ReadFixedLengthByteArray(appInfoPlaylistLength);
            AppInfoPlaylist = new AppInfoPlaylist(appInfoPlaylistBuffer);

            int playlistLength = playlistMarkStartAddress - (int) ms.Position;
            readPlaylist(ms, playlistLength);

            int playlistMarkLength = extensionStartAddress == 0
                ? (int) ms.Length - (int) ms.Position
                : extensionStartAddress - (int) ms.Position;
            byte[] playlistMarkBuffer = ms.ReadFixedLengthByteArray(playlistMarkLength);
            readMarks(playlistMarkBuffer);

            if (extensionStartAddress != 0)
                throw new NotImplementedException("ExtensionData");
        }

        private void readMarks(byte[] markBuffer)
        {
            MemoryStream ms = new MemoryStream(markBuffer);
            int assumedBufferLength = ms.ReadInt32BE();
            Debug.WriteLineIf(assumedBufferLength != markBuffer.Length - 4, "Mark Buffer length weird.");
            int numMarks = ms.ReadUInt16();
            Marks = new Mark[numMarks];
            for (int i = 0; i < numMarks; i++)
            {
                byte reserved = ms.ReadInt8();
                MarkType markType = (MarkType)ms.ReadInt8();
                ushort playItemId = ms.ReadUInt16();
                uint timeStamp = ms.ReadUInt32BE();
                ushort entryEsPid = ms.ReadUInt16();
                uint duration = ms.ReadUInt32BE();
                Marks[i] = new Mark(markType, playItemId, timeStamp, entryEsPid, duration);
            }
        }

        private void readPlaylist(MemoryStream ms,int playlistLength)
        {
            int assumedLength = ms.ReadInt32BE();
            Debug.WriteLineIf(assumedLength != playlistLength, String.Format("Expected Playlist length {0}, but got {1}",playlistLength,assumedLength));
            ushort reserved = ms.ReadUInt16();
            ushort playLength = ms.ReadUInt16();
            ushort subpathsLength = ms.ReadUInt16();

            PlayItems = new PlayItem[playLength];
            for (int i = 0; i < playLength; i++)
            {
                PlayItems[i] = new PlayItem(ms);
            }

            if (subpathsLength > 0)
            {
                int subpathBufferSize = ms.ReadInt32BE();
                MemoryStream ms2 = new MemoryStream(ms.ReadFixedLengthByteArray(subpathBufferSize));
                for (int i = 0; i < subpathsLength; i++)
                {
                    ms2.Position++;
                    SubPathType subPathType = (SubPathType) ms2.ReadInt8();
                    ms2.Position++;
                    bool repeat = (ms2.ReadInt8() & 0x01) != 0;
                    ms2.Position++;
                    int numSubPlayItems = ms2.ReadInt8();
                    SubPlayItem[] subPlayItems = new SubPlayItem[numSubPlayItems];
                    for (int j = 0; j < numSubPlayItems; j++)
                    {
                        subPlayItems[j] = new SubPlayItem(ms2.ReadFixedLengthByteArray(ms2.ReadUInt16()));
                    }
                }
            }
        }

        public int Version { get; private set; }

        public AppInfoPlaylist AppInfoPlaylist { get; private set; }

        public PlayItem[] PlayItems { get; private set; }

        public Mark[] Marks { get; private set; }
    }

    
}
