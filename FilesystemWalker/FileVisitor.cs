using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.FilesystemWalker
{
    public interface FileVisitor
    {
        FileVisitorResult PreVisitDirectory(DirectoryInfo di);
        FileVisitorResult VisitFile(FileInfo fi);
        FileVisitorResult VisitFileFailed(FileInfo fi, IOException exception);
        FileVisitorResult PostVisitDirectory(DirectoryInfo di, IOException exception);
    }
}
