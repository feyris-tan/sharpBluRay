using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.PlaylistModel
{
    public class UserOperationMaskTable
    {
        internal UserOperationMaskTable(byte[] buffer)
        {
            if (buffer.Length != 8)
                throw new ArgumentException("Length should be 8!", "buffer");

            int ptr = 0;
            byte b = buffer[ptr++];
            ChapterSearchMask = (b & 0x20) != 0;
            TimeSearchMask = (b & 0x10) != 0;
            SkipToNextPointMask = (b & 0x08) != 0;
            SkipBackToPreviousPointMask = (b & 0x04) != 0;
            StopMask = (b & 0x01) != 0;

            b = buffer[ptr++];
            PauseOnMask = (b & 0x80) != 0;
            StillOffMask = (b & 0x20) != 0;
            ForwardPlayMask = (b & 0x10) != 0;
            BackwardPlayMask = (b & 0x08) != 0;
            ResumeMask = (b & 0x04) != 0;
            MoveUpSelectedButtonMask = (b & 0x02) != 0;
            MoveDownSelectedButtonMask = (b & 0x01) != 0;

            b = buffer[ptr++];
            MoveLeftSelectedButtonMask = (b & 0x80) != 0;
            MoveRightSelectedButtonMask = (b & 0x40) != 0;
            SelectButtonMask = (b & 0x20) != 0;
            ActivateButtonMask = (b & 0x10) != 0;
            SelectButtonAndActivateMask = (b & 0x08) != 0;
            PrimaryAudioStreamNumberChangeMask = (b & 0x04) != 0;
            AngleNumberChangeMask = (b & 0x01) != 0;

            b = buffer[ptr++];
            PopupOnMask = (b & 0x80) != 0;
            PopupOffMask = (b & 0x40) != 0;
            PGTextSTEnableDisableMask = (b & 0x20) != 0;
            PGTextSTStreamNumberChangeMask = (b & 0x10) != 0;
            SecondaryVideoEnableDisableMask = (b & 0x08) != 0;
            SecondaryVideoStreamNumberChangeMask = (b & 0x04) != 0;
            SecondaryAudioEnableDisableMask = (b & 0x02) != 0;
            SecondaryAudioStreamNumberChangeMask = (b & 0x01) != 0;

            b = buffer[ptr++];
            PipPGTextSTStreamNumberChangeMask = (b & 0x40) != 0;
        }

        public bool ChapterSearchMask { get; private set; }

        public bool TimeSearchMask { get; private set; }

        public bool SkipToNextPointMask { get; private set; }

        public bool SkipBackToPreviousPointMask { get; private set; }

        public bool StopMask { get; private set; }

        public bool PauseOnMask { get; private set; }

        public bool StillOffMask { get; private set; }

        public bool ForwardPlayMask { get; private set; }

        public bool BackwardPlayMask { get; private set; }

        public bool ResumeMask { get; private set; }

        public bool MoveUpSelectedButtonMask { get; private set; }

        public bool MoveDownSelectedButtonMask { get; private set; }

        public bool MoveLeftSelectedButtonMask { get; private set; }

        public bool MoveRightSelectedButtonMask { get; private set; }

        public bool SelectButtonMask { get; private set; }

        public bool ActivateButtonMask { get; private set; }

        public bool SelectButtonAndActivateMask { get; private set; }

        public bool PrimaryAudioStreamNumberChangeMask { get; private set; }

        public bool AngleNumberChangeMask { get; private set; }

        public bool PopupOnMask { get; private set; }

        public bool PopupOffMask { get; private set; }

        public bool PGTextSTEnableDisableMask { get; private set; }

        public bool PGTextSTStreamNumberChangeMask { get; private set; }

        public bool SecondaryVideoEnableDisableMask { get; private set; }

        public bool SecondaryVideoStreamNumberChangeMask { get; private set; }

        public bool SecondaryAudioEnableDisableMask { get; private set; }

        public bool SecondaryAudioStreamNumberChangeMask { get; private set; }

        public bool PipPGTextSTStreamNumberChangeMask { get; private set; }
    }
}
