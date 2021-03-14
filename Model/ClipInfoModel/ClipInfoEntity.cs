using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class ClipInfoEntity
    {
        internal ClipInfoEntity(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer, false);
            ms.Position += 2;
            ClipStreamType = ms.ReadInt8();
            ClipApplicationType = ms.ReadInt8();
            int mask32 = ms.ReadInt32BE();
            if (mask32 != 0)
            {
                Debug.WriteLine("Possible ATC Delta");
            }
            HasAtcDeltas = (mask32 & 1) != 0;
            TsRecordingRate = ms.ReadInt32BE();
            NumSourcePackets = ms.ReadInt32BE();
            ms.Position += 128;

            int tsInfoLen = ms.ReadUInt16BE();
            int tsInfoPos = (int)ms.Position;
            if (tsInfoLen != 0)
            {
                Validity = ms.ReadInt8();
                FormatId = ms.ReadFixedLengthString(4);
                ms.Position = tsInfoLen + tsInfoPos;
            }

            if (HasAtcDeltas)
            {
                ms.Position++;
                AtcDeltaCount = ms.ReadInt8();
                AtcDeltas = new AtcDelta[AtcDeltaCount];
                for (int i = 0; i < AtcDeltaCount; i++)
                {
                    AtcDeltas[i] = new AtcDelta();
                    AtcDeltas[i].Delta = ms.ReadInt32BE();
                    AtcDeltas[i].FileId = ms.ReadFixedLengthString(5);
                    AtcDeltas[i].FileCode = ms.ReadFixedLengthString(4);
                    ms.Position++;
                }
            }
        }

        public AtcDelta[] AtcDeltas { get; private set; }

        public byte AtcDeltaCount { get; private set; }

        public string FormatId { get; private set; }

        public byte Validity { get; private set; }

        public int NumSourcePackets { get; private set; }

        public int TsRecordingRate { get; private set; }

        public bool HasAtcDeltas { get; private set; }

        public byte ClipApplicationType { get; private set; }

        public byte ClipStreamType { get; private set; }
    }
}
