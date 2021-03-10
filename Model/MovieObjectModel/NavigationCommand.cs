using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection.Emit;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.MovieObjectModel
{
    public class NavigationCommand
    {
        internal NavigationCommand(byte[] buffer)
        {
            if (buffer.Length != 12)
                throw new ArgumentException("A navigation command must be exactly 12-bytes long.");

            MemoryStream ms = new MemoryStream(buffer, false);
            byte readInt8 = ms.ReadInt8();
            OperandCount = (readInt8 & 0xe0) >> 5;
            int commandGroup = (readInt8 & 0x18) >> 3;
            int commandSubGroup = (readInt8 & 0x07);

            readInt8 = ms.ReadInt8();
            IFlagOperand1 = (readInt8 & 0x80) != 0;
            IFlagOperand2 = (readInt8 & 0x40) != 0;
            int branchOption = (readInt8 & 0x0f);

            readInt8 = ms.ReadInt8();
            int compareOptions = (readInt8 & 0x0f);

            readInt8 = ms.ReadInt8();
            int setOptions = (readInt8 & 0x1f);

            Destination = ms.ReadUInt32BE();
            Source = ms.ReadUInt32BE();

            //Actually Parse the extracted stuff
            Group = (Group) commandGroup;
            switch (Group)
            {
                case Group.Branch:
                    SubGroupBranch = (GroupBranch) commandSubGroup;
                    switch (SubGroupBranch)
                    {
                        case GroupBranch.Goto:
                            BranchOptionsGoto = (GroupBranchGoto) branchOption;
                            break;
                        case GroupBranch.Jump:
                            BranchOptionsJump = (GroupBranchJump) branchOption;
                            break;
                        case GroupBranch.Play:
                            BranchOptionsPlay = (GroupBranchPlay) branchOption;
                            break;
                        default:
                            throw new NotImplementedException(SubGroupBranch.ToString());
                    }
                    break;
                case Group.Compare:
                    CompareOperator = (GroupCompare) compareOptions;
                    break;
                case Group.Set:
                    SubGroupSet = (GroupSet) commandSubGroup;
                    switch (SubGroupSet)
                    {
                        case GroupSet.Set:
                            SetOperator = (GroupSetSet) setOptions;
                            break;
                        case GroupSet.SetSystem:
                            SetSystemOption = (GroupSetSystem) setOptions;
                            break;
                        default:
                            throw new NotImplementedException(SubGroupSet.ToString());
                    }
                    break;
                default:
                    throw new NotImplementedException(Group.ToString());
            }
        }

        public uint Source { get; private set; }
        public uint Destination { get; private set; }
        public Group Group { get; private set; }
        public GroupBranch SubGroupBranch { get; private set; }
        public GroupBranchGoto BranchOptionsGoto { get; private set; }
        public GroupBranchJump BranchOptionsJump { get; private set; }
        public GroupBranchPlay BranchOptionsPlay { get; private set; }
        public GroupCompare CompareOperator { get; private set; }
        public GroupSet SubGroupSet { get; private set; }
        public GroupSetSet SetOperator { get; private set; }
        public GroupSetSystem SetSystemOption { get; private set; }
        public int OperandCount { get; private set; }
        public bool IFlagOperand1 { get; private set; }
        public bool IFlagOperand2 { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}, ", Group);
            switch (Group)
            {
                case Group.Branch:
                    switch (SubGroupBranch)
                    {
                        case GroupBranch.Goto: sb.AppendFormat("{0}, {1}", BranchOptionsGoto.ToString(),PrintOperands()); break;
                        case GroupBranch.Jump: sb.AppendFormat("{0}, {1}", BranchOptionsJump.ToString(),PrintOperands()); break;
                        case GroupBranch.Play: sb.AppendFormat("{0}, {1}", BranchOptionsPlay.ToString(),PrintOperands()); break;
                        default: sb.Append("???, "); break;
                    }
                    break;
                case Group.Compare:
                    sb.AppendFormat("{0}, {1}", CompareOperator.ToString(), PrintOperands());
                    break;
                case Group.Set:
                    switch (SubGroupSet)
                    {
                        case GroupSet.Set: sb.AppendFormat("{0}, {1}", SetOperator.ToString(), PrintOperands()); break;
                        case GroupSet.SetSystem: sb.AppendFormat("{0}, {1}", SetOperator.ToString(), PrintOperands()); break;
                        default: sb.Append("???, "); break;
                    }
                    break;
            }
            return sb.ToString();
        }

        private string PrintOperands()
        {
            StringBuilder sb = new StringBuilder();
            uint psr1 = UInt32.MaxValue, psr2 = UInt32.MaxValue;
            if (OperandCount > 0)
            {
                sb.AppendFormat("{0}, ",PrintOperand(IFlagOperand1, Destination, ref psr1));
                if (OperandCount > 1)
                {
                    sb.AppendFormat("{0}, ", PrintOperand(IFlagOperand2, Source, ref psr2));
                }
            }
            return sb.ToString();
        }

        private string PrintOperand(bool immediate, uint op, ref uint psr)
        {
            if (!immediate)
            {
                if ((op & 0x80000000) != 0)
                {
                    psr = op & (int)0x7f;
                    return String.Format("PSR{0:000}", psr);
                }
                else
                {
                    return String.Format("r{0:00000}", op & 0xfff);
                }
            }
            else
            {
                return String.Format("{0}", op);
            }
        }
    }
}
