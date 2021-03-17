using System;
using System.Collections.Generic;
using System.Text;

namespace moe.yo3explorer.sharpBluRay.FilesystemAbstraction
{
    class MetadataArchiveWrapper : IDirectoryAbstraction
    {
        private List<MetadataArchiveWrapper> subdirectories;
        private Dictionary<string, byte[]> files;

        public MetadataArchiveWrapper(string name, List<MetadataArchiveWrapper> subdirectories, Dictionary<string, byte[]> files)
        {
            Name = name;
            this.subdirectories = subdirectories;
            this.files = files;
        }

        public bool TestForFile(string filename)
        {
            return files.ContainsKey(filename);
        }

        public byte[] ReadFileCompletely(string filename)
        {
            return files[filename];
        }

        public bool TestForSubdirectory(string dirname)
        {
            return subdirectories.Exists(x => x.Name.Equals(dirname));
        }

        public IDirectoryAbstraction OpenSubdirectory(string dirname)
        {
            return subdirectories.Find(x => x.Name.Equals(dirname));
        }

        public string Name { get; }
        public string[] ListFiles()
        {
            string[] filenames = new string[files.Count];
            int i = 0;
            foreach (KeyValuePair<string, byte[]> keyValuePair in files)
            {
                filenames[i++] = keyValuePair.Key;
            }
            return filenames;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
