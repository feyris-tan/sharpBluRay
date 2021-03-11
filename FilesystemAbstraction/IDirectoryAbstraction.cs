using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.FilesystemAbstraction
{
    public interface IDirectoryAbstraction
    {
        bool TestForFile(string filename);
        byte[] ReadFileCompletely(string filename);
        bool TestForSubdirectory(string dirname);
        IDirectoryAbstraction OpenSubdirectory(string dirname);
        string Name { get; }
        string[] ListFiles();
    }
}
