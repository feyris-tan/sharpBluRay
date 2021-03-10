using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel
{
    class PrimaryAudioStreamInfo : StreamInfo
    {
        public PrimaryAudioStreamInfo(MemoryStream ms) : base(PlaylistModel.StreamType.PrimaryAudio)
        {
            readStreamAttribute(ms);

            //StreamAttribute
            byte[] attributeBuffer = ms.ReadFixedLengthByteArray(ms.ReadInt8());
            MemoryStream ms2 = new MemoryStream(attributeBuffer);
            AudioCodec = (AudioCodec)ms2.ReadInt8();
            int value = ms2.ReadInt8();
            AudioPresentationMode = (StreamModel.AudioPresentationMode) (value >> 4);
            SamplingRate samplingRate = (SamplingRate) (value & 0x0f);
            switch (samplingRate)
            {
                case SamplingRate._48: PrimarySamplingRate = 48000; break;
                case SamplingRate._96: PrimarySamplingRate = 96000; break;
                case SamplingRate._192: PrimarySamplingRate = 192000; break;
                default: throw new NotImplementedException(samplingRate.ToString());
            }

            LanguageCode = ms2.ReadFixedLengthString(3);
        }

        public AudioCodec AudioCodec { get; private set; }

        public AudioPresentationMode AudioPresentationMode { get; private set; }

        public int PrimarySamplingRate { get; private set; }

        public string LanguageCode { get; private set; }

        public override string ToString()
        {
            return $"{nameof(LanguageCode)}: {LanguageCode.DecodeLanguageCodeEndonyme()}, {nameof(AudioCodec)}: {AudioCodec}, {nameof(AudioPresentationMode)}: {AudioPresentationMode}, {nameof(PrimarySamplingRate)}: {PrimarySamplingRate}";
        }
    }
}
