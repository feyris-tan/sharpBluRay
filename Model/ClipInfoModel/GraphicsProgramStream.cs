using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.ClipInfoModel
{
    public class GraphicsProgramStream : ProgramStream
    {
        internal GraphicsProgramStream() { }
        public string LanguageCode { get; internal set; }
        public override ProgramStreamType ProgramStreamType => ClipInfoModel.ProgramStreamType.GRAPHICS;

        public override string ToString()
        {
            return $"{nameof(LanguageCode)}: {LanguageCode.DecodeLanguageCodeEndonyme()}, {nameof(ProgramStreamType)}: {ProgramStreamType}";
        }
    }
}
