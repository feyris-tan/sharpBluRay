using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class SubtitleProgramStream : ProgramStream
    {
        internal SubtitleProgramStream() { }
        public byte CharCode { get; internal set; }
        public string LanguageCode { get; internal set; }
        public override ProgramStreamType ProgramStreamType => ClipInfoModel.ProgramStreamType.SUBTITLE;

        public override string ToString()
        {
            return $"{nameof(LanguageCode)}: {LanguageCode.DecodeLanguageCodeEndonyme()}, {nameof(CharCode)}: {CharCode}, {nameof(ProgramStreamType)}: {ProgramStreamType}";
        }
    }
}
