using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel
{
    public class PlayItem
    {
        internal PlayItem(MemoryStream ms)
        {
            ushort len = ms.ReadUInt16BE();
            byte[] buffer = ms.ReadFixedLengthByteArray(len);
            ms = new MemoryStream(buffer, false);

            ClipName = ms.ReadFixedLengthString(5);
            CodecId = ms.ReadFixedLengthString(4);
            ms.Position++;

            byte b = ms.ReadInt8();
            IsMultiAngle = (b & 0x10) != 0;
            ConnectionCondition = (b & 0x0f);
            StcId = ms.ReadInt8();
            InTime = ms.ReadInt32BE();
            OutTime = ms.ReadInt32BE();
            UOMaskTable = new UserOperationMaskTable(ms.ReadFixedLengthByteArray(8));

            b = ms.ReadInt8();
            PlayItemRandomAccessFlag = (b & 0x80) != 0;
            StillMode = ms.ReadInt8();
            if (StillMode == 1)
                StillTime = ms.ReadUInt16BE();
            else
                ms.Position += 2;

            _angles = new List<AngleClipInfo>();
            _angles.Add(new AngleClipInfo(0, ClipName, CodecId, StcId));
            if (IsMultiAngle)
            {
                int entries = ms.ReadInt8();
                b = ms.ReadInt8();
                MultiAngleDifferentAudios = (b & 0x02) != 0;
                SeamlessAngleChange = (b & 0x01) != 0;
                for (int i = 1; i < entries; i++)
                {
                    string subClipName = ms.ReadFixedLengthString(5);
                    string subcodecid = ms.ReadFixedLengthString(4);
                    byte subStcId = ms.ReadInt8();
                    _angles.Add(new AngleClipInfo(i, subClipName, subcodecid, subStcId));
                }
            }

            int streamsLength = ms.ReadUInt16BE();
            ms.Position += 2;
            byte numPrimaryVideo = ms.ReadInt8();
            byte numPrimaryAudio = ms.ReadInt8();
            byte numPgText = ms.ReadInt8();
            byte numIgStream = ms.ReadInt8();
            byte numSecondaryAudio = ms.ReadInt8();
            byte numSecondaryVideo = ms.ReadInt8();
            byte numPipPgText = ms.ReadInt8();
            ms.Position += 5;

            streamInfos = new List<StreamInfo>();
            while (numPrimaryVideo > 0)
            {
                streamInfos.Add(new PrimaryVideoStreamInfo(ms));
                numPrimaryVideo--;
            }

            while (numPrimaryAudio > 0)
            {
                streamInfos.Add(new PrimaryAudioStreamInfo(ms));
                numPrimaryAudio--;
            }

            while (numPgText > 0)
            {
                streamInfos.Add(new PresentationGraphicsStreamInfo(ms));
                numPgText--;
            }

            while (numIgStream > 0)
            {
                streamInfos.Add(new InteractiveGraphicsSteamInfo(ms));
                numIgStream--;
            }

            while (numSecondaryAudio > 0)
            {
                throw new NotImplementedException("Secondary Audio");
            }

            while (numSecondaryVideo > 0)
            {
                throw new NotImplementedException("Secondary Video");
            }

            while (numPipPgText > 0)
            {
                throw new NotImplementedException("Picture-In-Picture PGS");
            }
        }


        public bool SeamlessAngleChange { get; private set; }

        public bool MultiAngleDifferentAudios { get; private set; }

        private List<AngleClipInfo> _angles;
        public ReadOnlyCollection<AngleClipInfo> Angles { get { return new ReadOnlyCollection<AngleClipInfo>(_angles);} }

        public ushort StillTime { get; private set; }

        public byte StillMode { get; private set; }

        public bool PlayItemRandomAccessFlag { get; private set; }

        public UserOperationMaskTable UOMaskTable { get; private set; }

        public int OutTime { get; set; }

        public int InTime { get; private set; }

        public byte StcId { get; private set; }

        public int ConnectionCondition { get; private set; }

        public bool IsMultiAngle { get; private set; }

        public string ClipName { get; private set; }
        public string CodecId { get; private set; }

        private List<StreamInfo> streamInfos;
        public ReadOnlyCollection<StreamInfo> Streams { get { return new ReadOnlyCollection<StreamInfo>(streamInfos); } }
        public SubPlayItem[] SubPlayItems { get; internal set; }
    }
}
