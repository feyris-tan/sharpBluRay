using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.MovieObjectModel
{
    public class MovieObjectEntry
    {
        internal MovieObjectEntry(MemoryStream ms)
        {
            byte int8 = ms.ReadInt8();
            ms.Position++;
            TerminalInfo = new TerminalInfo(int8);

            ushort NumNavigationCommands = ms.ReadUInt16();
            NavigationCommands = new NavigationCommand[NumNavigationCommands];
            for (ushort i = 0; i < NumNavigationCommands; i++)
            {
                NavigationCommands[i] = new NavigationCommand(ms.ReadFixedLengthByteArray(12));
            }
        }

        public TerminalInfo TerminalInfo { get; private set; }
        public NavigationCommand[] NavigationCommands { get; private set; }
    }
}
