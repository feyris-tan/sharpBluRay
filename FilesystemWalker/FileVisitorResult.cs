using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.FilesystemWalker
{
    public enum FileVisitorResult
    {
        CONTINUE,
        TERMINATE,
        SKIP_SUBTREE,
        SKIP_SIBLINGS,
    }
}
