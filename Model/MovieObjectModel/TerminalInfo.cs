using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.MovieObjectModel
{
    public struct TerminalInfo
    {
        public TerminalInfo(byte b)
        {
            ResumeIntentionFlag = (b & 0x80) != 0;
            MenuCallMask = (b & 0x40) != 0;
            TitleSearchMask = (b & 0x20) != 0;
        }

        public bool TitleSearchMask { get; private set; }

        public bool MenuCallMask { get; private set; }

        public bool ResumeIntentionFlag { get; private set; }
    } 
}
