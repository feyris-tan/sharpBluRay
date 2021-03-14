using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.FilesystemWalker
{
    public static class FileWalker
    {
        public static void WalkFilesystemTree(FileVisitor fileVisitor, DirectoryInfo directoryInfo)
        {
            Walk(fileVisitor, directoryInfo);
        }

        private static FileVisitorResult Walk(FileVisitor fileVisitor, DirectoryInfo directoryInfo)
        {
            FileVisitorResult preVisitResult = fileVisitor.PreVisitDirectory(directoryInfo);
            bool preSkipSiblings = false;
            switch (preVisitResult)
            {
                case FileVisitorResult.TERMINATE:
                    return FileVisitorResult.TERMINATE;
                case FileVisitorResult.CONTINUE:
                    break;
                case FileVisitorResult.SKIP_SUBTREE:
                    return FileVisitorResult.CONTINUE;
                case FileVisitorResult.SKIP_SIBLINGS:
                    preSkipSiblings = true;
                    break;
            }

            IOException occurredException = null;
            foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
            {
                if (((int)fileSystemInfo.Attributes & (int)FileAttributes.Directory) != 0)
                {
                    DirectoryInfo subdir = (DirectoryInfo)fileSystemInfo;
                    FileVisitorResult walkSubdirResult = Walk(fileVisitor, subdir);
                    switch (walkSubdirResult)
                    {
                        case FileVisitorResult.CONTINUE:
                            break;
                        case FileVisitorResult.SKIP_SIBLINGS:
                            return FileVisitorResult.CONTINUE;
                        case FileVisitorResult.TERMINATE:
                            return FileVisitorResult.TERMINATE;
                        default:
                            throw new NotImplementedException(walkSubdirResult.ToString());
                    }
                }
                else
                {
                    FileInfo subfile = (FileInfo) fileSystemInfo;
                    try
                    {
                        FileVisitorResult visitFileResult = fileVisitor.VisitFile(subfile);
                        switch (visitFileResult)
                        {
                            case FileVisitorResult.CONTINUE:
                                break;
                            case FileVisitorResult.SKIP_SIBLINGS:
                                return FileVisitorResult.CONTINUE;
                            case FileVisitorResult.TERMINATE:
                                return FileVisitorResult.TERMINATE;
                            default:
                                throw new NotImplementedException(visitFileResult.ToString());
                        }
                    }
                    catch (IOException ioe)
                    {
                        FileVisitorResult visitFileFailedResult = fileVisitor.VisitFileFailed(subfile, ioe);
                        switch (visitFileFailedResult)
                        {
                            case FileVisitorResult.TERMINATE:
                                return FileVisitorResult.TERMINATE;
                            case FileVisitorResult.CONTINUE:
                                break;
                            case FileVisitorResult.SKIP_SIBLINGS:
                                return FileVisitorResult.CONTINUE;
                            default:
                                throw new NotImplementedException(visitFileFailedResult.ToString());
                        }
                    }
                }
            }

            FileVisitorResult postVisitResult = fileVisitor.PostVisitDirectory(directoryInfo, occurredException);
            return preSkipSiblings ? FileVisitorResult.SKIP_SIBLINGS : postVisitResult;
        }


    }
}
