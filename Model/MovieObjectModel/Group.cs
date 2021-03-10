using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.Model.MovieObjectModel
{
    public enum Group : int
    {
        Branch,
        Compare,
        Set
    }

    public enum GroupBranch : int
    {
        Goto,
        Jump,
        Play
    }

    public enum GroupBranchGoto : int
    {
        Nop,
        Goto,
        Break
    }

    public enum GroupBranchJump : int
    {
        JumpObject,
        JumpTitle,
        CallObject,
        CallTitle,
        Resume
    }

    public enum GroupBranchPlay : int
    {
        PlayPl,
        PlayPlPi,
        PlayPlPm,
        TerminatePl,
        LinkPi,
        LinkMk
    }

    public enum GroupCompare : int
    {
        BC,
        Equals,
        NotEquals,
        GreaterOrEquals,
        GreaterThan,
        LessOrEquals,
        LessThan
    }

    public enum GroupSet : int
    {
        Set,
        SetSystem
    }

    public enum GroupSetSet : int
    {
        Move,
        Swap,
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulo,
        Randomize,
        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,
        SetBit,
        ClearBit,
        ShiftLeft,
        ShiftRight
    }

    public enum GroupSetSystem : int
    {
        SetStream,
        SetNonVolatileTimer,
        SetButtonPage,
        EnableButton,
        DisableButton,
        SetSecondaryStream,
        PopupOff,
        StillOn,
        StillOff,
        SetOutputMode,
        SetStreamSs,
        Unknown0x10 = 0x10
    }
}
