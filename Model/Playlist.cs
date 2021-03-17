using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.ComponentModel;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel;

namespace moe.yo3explorer.sharpBluRay.Model
{
    [TypeConverter(typeof(PlaylistTypeConverter))]
    public class Playlist
    {
        public Playlist(byte[] mplsBuffer, int id)
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
            ParseAppInfoPlaylist(appInfoPlaylistBuffer);

            int playlistLength = playlistMarkStartAddress - (int) ms.Position;
            readPlaylist(ms, playlistLength);

            int playlistMarkLength = extensionStartAddress == 0
                ? (int) ms.Length - (int) ms.Position
                : extensionStartAddress - (int) ms.Position;
            byte[] playlistMarkBuffer = ms.ReadFixedLengthByteArray(playlistMarkLength);
            readMarks(playlistMarkBuffer);

            if (extensionStartAddress != 0)
                throw new NotImplementedException("ExtensionData");

            this.ID = id;
        }

        public int ID { get; private set; }

        private void readMarks(byte[] markBuffer)
        {
            MemoryStream ms = new MemoryStream(markBuffer);
            int assumedBufferLength = ms.ReadInt32BE();
            Debug.WriteLineIf(assumedBufferLength != markBuffer.Length - 4, "Mark Buffer length weird.");
            int numMarks = ms.ReadUInt16BE();
            Marks = new Mark[numMarks];
            for (int i = 0; i < numMarks; i++)
            {
                byte reserved = ms.ReadInt8();
                MarkType markType = (MarkType)ms.ReadInt8();
                ushort playItemId = ms.ReadUInt16BE();
                uint timeStamp = ms.ReadUInt32BE();
                ushort entryEsPid = ms.ReadUInt16BE();
                uint duration = ms.ReadUInt32BE();
                Marks[i] = new Mark(markType, playItemId, timeStamp, entryEsPid, duration, PlayItems[0].InTime);
            }
        }

        private void readPlaylist(MemoryStream ms,int playlistLength)
        {
            int assumedLength = ms.ReadInt32BE();
            Debug.WriteLineIf(assumedLength != playlistLength - 4, String.Format("Expected Playlist length {0}, but got {1}",playlistLength,assumedLength));
            ushort reserved = ms.ReadUInt16BE();
            ushort playLength = ms.ReadUInt16BE();
            ushort subpathsLength = ms.ReadUInt16BE();

            PlayItems = new PlayItem[playLength];
            for (int i = 0; i < playLength; i++)
            {
                PlayItems[i] = new PlayItem(ms);
            }

            if (subpathsLength > 0)
            {
                for (int i = 0; i < subpathsLength; i++)
                {
                    int subpathBufferSize = ms.ReadInt32BE();
                    MemoryStream ms2 = new MemoryStream(ms.ReadFixedLengthByteArray(subpathBufferSize));
                    ms2.Position++;
                    SubPathType subPathType = (SubPathType) ms2.ReadInt8();
                    ms2.Position++;
                    bool repeat = (ms2.ReadInt8() & 0x01) != 0;
                    ms2.Position++;
                    int numSubPlayItems = ms2.ReadInt8();
                    SubPlayItem[] subPlayItems = new SubPlayItem[numSubPlayItems];
                    for (int j = 0; j < numSubPlayItems; j++)
                    {
                        subPlayItems[j] = new SubPlayItem(ms2.ReadFixedLengthByteArray(ms2.ReadUInt16BE()));
                    }

                    PlayItems[i].SubPlayItems = subPlayItems;
                }
            }
        }

        private void ParseAppInfoPlaylist(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer, false);
            int length = ms.ReadInt32BE();
            int assumedLength = buffer.Length - 4;
            Debug.WriteLineIf(length != assumedLength, String.Format("AppInfoPlaylist Length is {0}, expected {1}", length, assumedLength));
            byte reserved = ms.ReadInt8();

            int type = ms.ReadInt8();
            PlaybackType = (PlaybackType)type;
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

        public int Version { get; private set; }
        
        public PlayItem[] PlayItems { get; private set; }

        [Description("The chapters of a track")]
        [Browsable(true)]
        public Mark[] Marks { get; private set; }

        public override string ToString()
        {
            return $"{nameof(ID)}: {ID}, {nameof(PlaybackType)}: {PlaybackType}, {nameof(PlaybackCount)}: {PlaybackCount}";
        }
    }
}
