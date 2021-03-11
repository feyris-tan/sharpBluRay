using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.FilesystemAbstraction
{
    public class DirectoryInfoWrapper : IDirectoryAbstraction
    {
        [DebuggerStepThrough]
        public DirectoryInfoWrapper(DirectoryInfo diw, string displayName)
        {
            this.PhysicalDirectory = diw;
            this.Name = displayName;
        }

        private DirectoryInfo PhysicalDirectory;

        public string Name { get; }

        [DebuggerStepThrough]
        public bool TestForFile(string filename)
        {
            FileInfo fi = new FileInfo(Path.Combine(PhysicalDirectory.FullName, filename));
            return fi.Exists;
        }

        [DebuggerStepThrough]
        public byte[] ReadFileCompletely(string filename)
        {
            FileInfo fi = new FileInfo(Path.Combine(PhysicalDirectory.FullName, filename));
            if (!fi.Exists)
                throw new FileNotFoundException("A file to-be-read was not found.", fi.FullName);
            return File.ReadAllBytes(fi.FullName);
        }

        [DebuggerStepThrough]
        public bool TestForSubdirectory(string dirname)
        {
            DirectoryInfo fi = new DirectoryInfo(Path.Combine(PhysicalDirectory.FullName, dirname));
            return fi.Exists;
        }

        [DebuggerStepThrough]
        public IDirectoryAbstraction OpenSubdirectory(string dirname)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(PhysicalDirectory.FullName, dirname));
            if (!di.Exists)
                throw new DirectoryNotFoundException(di.FullName);
            return new DirectoryInfoWrapper(di, dirname);
        }
        
        [DebuggerStepThrough]
        public string[] ListFiles()
        {
            FileInfo[] fileInfos = PhysicalDirectory.GetFiles();
            string[] convertAll = Array.ConvertAll(fileInfos, x => x.Name);
            return convertAll;
        }
    }
}
