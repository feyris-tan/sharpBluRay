using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.Model.ClipInfoModel;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel;

namespace moe.yo3explorer.sharpBluRay.Model
{
    public class ClipInfo
    {
        public ClipInfo(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer, false);
            if (!ms.ReadFixedLengthString(4).Equals("HDMV"))
                throw new InvalidDataException("Invalid magic number!");
            this.Version = int.Parse(ms.ReadFixedLengthString(4));

            int SequenceInfoStartAddress = ms.ReadInt32BE();
            int ProgramInfoStartAddress = ms.ReadInt32BE();
            int CpiStartAddress = ms.ReadInt32BE();
            int ClipMarkStartAddress = ms.ReadInt32BE();
            int ExtDataStartAddress = ms.ReadInt32BE();

            ms.Position = 40;
            int entityLength = ms.ReadInt32BE();
            byte[] entityBuffer = ms.ReadFixedLengthByteArray(entityLength);
            Entity = new ClipInfoEntity(entityBuffer);

            if (SequenceInfoStartAddress != 0)
            {
                ms.Position = SequenceInfoStartAddress;
                int sequenceInfoLength = ms.ReadInt32BE();
                byte[] sequenceInfoBuffer = ms.ReadFixedLengthByteArray(sequenceInfoLength);
                ParseSequence(sequenceInfoBuffer);
            }

            if (ProgramInfoStartAddress != 0)
            {
                ms.Position = ProgramInfoStartAddress;
                int programInfoLength = ms.ReadInt32BE();
                byte[] programInfoBuf = ms.ReadFixedLengthByteArray(programInfoLength);
                ParseProgram(programInfoBuf);
            }

            if (CpiStartAddress != 0)
            {
                ms.Position = CpiStartAddress;
                int cpiLength = ms.ReadInt32BE();
                if (cpiLength > 0)
                {
                    byte[] cpiBuf = ms.ReadFixedLengthByteArray(cpiLength);
                    ParseCpi(cpiBuf);
                }
                else
                {
                    Debug.WriteLine("CPI Length = 0");
                }
            }
        }

        private void ParseCpi(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer, false);
            ushort mask16 = ms.ReadUInt16BE();
            CPI = new CPI();
            CPI.Type = mask16 & 0x0f;

            int mapPos = (int) ms.Position;
            ms.Position++;
            CPI.NumStreamPids = ms.ReadInt8();
            CPI.Entries = new EpMapEntry[CPI.NumStreamPids];
            for (int i = 0; i < CPI.NumStreamPids; i++)
            {
                ulong mask64 = ms.ReadUInt64BE();
                CPI.Entries[i] = new EpMapEntry();
                CPI.Entries[i].PID = (ushort)        ((mask64 & 0xFFFF000000000000) >> 48);
                ulong reserved =                     ((mask64 & 0x0000FFC000000000) >> 38);
                CPI.Entries[i].EpStreamType = (byte) ((mask64 & 0x0000003C00000000) >> 34);
                CPI.Entries[i].NumEpCoarse = (ushort)((mask64 & 0x00000003FFFC0000) >> 18);
                CPI.Entries[i].NumEpFine = (uint)    ((mask64 & 0x000000000003FFFF));
                CPI.Entries[i].EpMapStreamStarAddr = ms.ReadUInt32BE() + (uint)mapPos;
            }

            for (int i = 0; i < CPI.NumStreamPids; i++)
            {
                ms.Position = CPI.Entries[i].EpMapStreamStarAddr;
                uint fineStart = ms.ReadUInt32BE();

                CPI.Entries[i].EpCoarse = new EpCoarse[CPI.Entries[i].NumEpCoarse];
                for (int j = 0; j < CPI.Entries[i].NumEpCoarse; j++)
                {
                    CPI.Entries[i].EpCoarse[j] = new EpCoarse();
                    uint mask32 = ms.ReadUInt32BE();
                    CPI.Entries[i].EpCoarse[j].RefEpFineId = (uint) ((mask32 & 0xFFFFC000) >> 14);
                    CPI.Entries[i].EpCoarse[j].PtsEp = (ushort) ((mask32 & 0x00003FFF));
                    CPI.Entries[i].EpCoarse[j].SpnEp = ms.ReadUInt32BE();
                }

                ms.Position = CPI.Entries[i].EpMapStreamStarAddr + fineStart;
                CPI.Entries[i].EpFine = new EpFine[CPI.Entries[i].NumEpFine];
                for (int j = 0; j < CPI.Entries[i].NumEpFine; j++)
                {
                    CPI.Entries[i].EpFine[j] = new EpFine();
                    uint mask32 = ms.ReadUInt32BE();
                    CPI.Entries[i].EpFine[j].IsAngleChangePoint =        ((mask32 & 0x80000000) >> 31) != 0;
                    CPI.Entries[i].EpFine[j].IEndPositionOffset = (byte) ((mask32 & 0x70000000) >> 28);
                    CPI.Entries[i].EpFine[j].PtsEp =            (ushort) ((mask32 & 0x0FFE0000) >> 17);
                    CPI.Entries[i].EpFine[j].SpnEp =              (uint) ((mask32 & 0x0001FFFF));
                }
            }
        }
        

        private void ParseProgram(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            ms.Position++;
            byte numPrograms = ms.ReadInt8();
            Programs = new Program[numPrograms];
            for (int i = 0; i < numPrograms; i++)
            {
                Programs[i] = new Program();
                Programs[i].ProgramSequenceStart = ms.ReadInt32BE();
                Programs[i].ProgramMapPid = ms.ReadUInt16BE();
                Programs[i].NumStreams = ms.ReadInt8();
                Programs[i].NumGroups = ms.ReadInt8();
                Programs[i].ProgramStreams = new ProgramStream[Programs[i].NumStreams];
                for (int j = 0; j < Programs[i].NumStreams; j++)
                {
                    ushort PID = ms.ReadUInt16BE();
                    int len = ms.ReadInt8();
                    int pos = (int) ms.Position;
                    byte codecType = ms.ReadInt8();
                    switch (codecType)
                    {
                        case 0x01:
                        case 0x02:
                        case 0x1b:
                        case 0xea:
                            VideoProgramStream child = new VideoProgramStream();
                            child.VideoCodec = (VideoCodec) codecType;
                            int value = ms.ReadInt8();
                            child.VideoResolution = (VideoResolution)(value >> 4);
                            FrameRate frameRate = (FrameRate)(value & 0x0f);
                            switch (frameRate)
                            {
                                case PlaylistModel.StreamModel.FrameRate._23_076: child.FrameRate = 23.976; break;
                                case PlaylistModel.StreamModel.FrameRate._24: child.FrameRate = 24; break;
                                case PlaylistModel.StreamModel.FrameRate._25: child.FrameRate = 25; break;
                                case PlaylistModel.StreamModel.FrameRate._29_976: child.FrameRate = 29.976; break;
                                case PlaylistModel.StreamModel.FrameRate._50: child.FrameRate = 50; break;
                                case PlaylistModel.StreamModel.FrameRate._59_94: child.FrameRate = 59.94; break;
                                default: throw new NotImplementedException(frameRate.ToString());
                            }
                            Programs[i].ProgramStreams[j] = child;
                            break;
                        case 0x03:
                        case 0x04:
                        case 0x80:
                        case 0x81:
                        case 0x82:
                        case 0x83:
                        case 0x84:
                        case 0x85:
                        case 0x86:
                        case 0xa1:
                        case 0xa2:
                            AudioProgramStream child2 = new AudioProgramStream();
                            child2.AudioCodec = (AudioCodec) codecType;
                            int value2 = ms.ReadInt8();
                            child2.AudioPresentationMode = (AudioPresentationMode)(value2 >> 4);
                            SamplingRate samplingRate = (SamplingRate)(value2 & 0x0f);
                            switch (samplingRate)
                            {
                                case SamplingRate._48: child2.PrimarySamplingRate = 48000; break;
                                case SamplingRate._96: child2.PrimarySamplingRate = 96000; break;
                                case SamplingRate._192: child2.PrimarySamplingRate = 192000; break;
                                default: throw new NotImplementedException(samplingRate.ToString());
                            }
                            child2.LanguageCode = ms.ReadFixedLengthString(3);
                            Programs[i].ProgramStreams[j] = child2;
                            break;
                        case 0x90:
                        case 0x91:
                        case 0xa0:
                            GraphicsProgramStream child3 = new GraphicsProgramStream();
                            child3.LanguageCode = ms.ReadFixedLengthString(3);
                            Programs[i].ProgramStreams[j] = child3;
                            break;
                        case 0x92:
                            SubtitleProgramStream child4 = new SubtitleProgramStream();
                            child4.CharCode = ms.ReadInt8();
                            child4.LanguageCode = ms.ReadFixedLengthString(3);
                            Programs[i].ProgramStreams[j] = child4;
                            break;
                        default:
                            Debug.WriteLine(String.Format("Unknown codec type: {0}", codecType));
                            break;
                    }

                    ms.Position = pos + len;
                    Programs[i].ProgramStreams[j].PID = PID;
                }
            }
        }

        private void ParseSequence(byte[] sequenceInfoBuffer)
        {
            MemoryStream ms = new MemoryStream(sequenceInfoBuffer, false);
            ms.Position++;
            int NumAtcSequences = ms.ReadInt8();
            Sequences = new AtcSequence[NumAtcSequences];
            for (int i = 0; i < NumAtcSequences; i++)
            {
                Sequences[i] = new AtcSequence();
                Sequences[i].SpnAtcStart = ms.ReadInt32BE();
                Sequences[i].NumStcSequences = ms.ReadInt8();
                Sequences[i].OffsetStcId = ms.ReadInt8();
                Sequences[i].StcSequences = new StcSequence[Sequences[i].NumStcSequences];
                for (int j = 0; j < Sequences[i].NumStcSequences; j++)
                {
                    Sequences[i].StcSequences[j] = new StcSequence();
                    Sequences[i].StcSequences[j].PcrPid = ms.ReadUInt16BE();
                    Sequences[i].StcSequences[j].SpnStcStart = ms.ReadInt32BE();
                    Sequences[i].StcSequences[j].PresentationStartTime = ms.ReadInt32BE();
                    Sequences[i].StcSequences[j].PresentationEndTime = ms.ReadInt32BE();
                }
            }
        }

        public int Version { get; set; }

        public ClipInfoEntity Entity { get; private set; }

        public AtcSequence[] Sequences { get; private set; }

        public Program[] Programs { get; private set; }

        public CPI CPI { get; private set; }
    }
}
