using System;
using System.Collections.Generic;
using System.Text;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel.StreamModel;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class AudioProgramStream : ProgramStream
    {
        public AudioCodec AudioCodec { get; internal set; }
        public AudioPresentationMode AudioPresentationMode { get; internal set; }
        public int PrimarySamplingRate { get; internal set; }
        public string LanguageCode { get; internal set; }
        public override ProgramStreamType ProgramStreamType => ClipInfoModel.ProgramStreamType.AUDIO;

        public override string ToString()
        {
            return $"{nameof(LanguageCode)}: {LanguageCode.DecodeLanguageCodeEndonyme()}, {nameof(AudioCodec)}: {AudioCodec}, {nameof(AudioPresentationMode)}: {AudioPresentationMode}, {nameof(PrimarySamplingRate)}: {PrimarySamplingRate}, {nameof(ProgramStreamType)}: {ProgramStreamType}";
        }
    }
}
