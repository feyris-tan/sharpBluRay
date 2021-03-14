using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using moe.yo3explorer.sharpBluRay.FilesystemAbstraction;

namespace moe.yo3explorer.sharpBluRay
{
    public static class MetadataArchiveManager
    {
        private const ulong MAGIC = 5279417649501403713;
        private const byte OPCODE_V1 = 0;
        private const byte OPCODE_BEGIN_DIRECTORY = 1;
        private const byte OPCODE_END_OF_ARCHIVE = 2;
        private const byte OPCODE_STRING = 3;
        private const byte OPCODE_FILE = 4;
        private const byte OPCODE_END_OF_DIRECTORY = 5;

        public static byte[] Serialize(IDirectoryAbstraction ida)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(MAGIC);
            bw.Write(OPCODE_V1);
            SerializeDirectory(ida, bw);
            bw.Write(OPCODE_END_OF_ARCHIVE);
            return ms.ToArray();
        }

        private static void SerializeDirectory(IDirectoryAbstraction ida, BinaryWriter bw)
        {
            bw.Write(OPCODE_BEGIN_DIRECTORY);
            SerializeString(ida.Name, bw);
            if (ida.TestForSubdirectory("BDMV"))
            {
                SerializeDirectory(ida.OpenSubdirectory("BDMV"), bw);
            }
            if (ida.TestForSubdirectory("CERTIFICATE"))
            {
                SerializeDirectory(ida.OpenSubdirectory("CERTIFICATE"), bw);
            }
            if (ida.TestForSubdirectory("PLAYLIST"))
            {
                SerializeDirectory(ida.OpenSubdirectory("PLAYLIST"), bw);
            }
            if (ida.TestForSubdirectory("CLIPINF"))
            {
                SerializeDirectory(ida.OpenSubdirectory("CLIPINF"), bw);
            }

            foreach (string listFile in ida.ListFiles())
            {
                if (!listFile.EndsWith(".bdmv") && !listFile.EndsWith(".mpls") && !listFile.EndsWith(".clpi"))
                    throw new NotImplementedException(listFile);
                bw.Write(OPCODE_FILE);
                byte[] buffer = ida.ReadFileCompletely(listFile);
                SerializeString(listFile, bw);
                bw.Write(buffer.Length);
                bw.Write(buffer);
            }
            bw.Write(OPCODE_END_OF_DIRECTORY);
        }
        
        private static void SerializeString(string input, BinaryWriter output)
        {
            output.Write(OPCODE_STRING);
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            output.Write(bytes.Length);
            output.Write(bytes);
        }

        private static string DeserializeString(Stream s)
        {
            if (s.ReadInt8() != OPCODE_STRING)
                throw new MetadataArchiveManagerException("out of sync!");
            int readInt32Le = s.ReadInt32LE();
            byte[] readFixedLengthByteArray = s.ReadFixedLengthByteArray(readInt32Le);
            return Encoding.UTF8.GetString(readFixedLengthByteArray);
        }

        public static IDirectoryAbstraction Deserialize(Stream s)
        {
            if (s.ReadUInt64LE() != MAGIC)
                throw new InvalidDataException("Invalid Metdata Archive Wrapper");

            MetadataArchiveWrapper rootResult = null;

            while (true)
            {
                int opcode = s.ReadInt8();
                switch (opcode)
                {
                    case OPCODE_V1:
                        continue;
                    case OPCODE_BEGIN_DIRECTORY:
                        if (rootResult != null)
                            throw new MetadataArchiveManagerException("multiple roots defined!");
                        rootResult = DeserializeDirectory(s);
                        continue;
                    case OPCODE_END_OF_ARCHIVE:
                        if (rootResult == null)
                            throw new NullReferenceException("No root found in file!");
                        return rootResult;
                    default:
                        throw new NotImplementedException(String.Format("Unimplemented opcode: " + opcode));
                }
            }
        }

        public static IDirectoryAbstraction Deserialize(byte[] buffer)
        {
            return Deserialize(new MemoryStream(buffer));
        }

        public static IDirectoryAbstraction Deserialize(FileInfo fi)
        {
            FileStream fileStream = fi.OpenRead();
            IDirectoryAbstraction deserialize = Deserialize(fileStream);
            fileStream.Close();
            return deserialize;
        }

        private static MetadataArchiveWrapper DeserializeDirectory(Stream s)
        {
            string name = DeserializeString(s);
            List<MetadataArchiveWrapper> subdirectories = new List<MetadataArchiveWrapper>();
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            while (true)
            {
                byte readInt8 = s.ReadInt8();
                switch (readInt8)
                {
                    case OPCODE_BEGIN_DIRECTORY:
                        subdirectories.Add(DeserializeDirectory(s));
                        continue;
                    case OPCODE_FILE:
                        string fname = DeserializeString(s);
                        int flen = s.ReadInt32LE();
                        byte[] buffer = s.ReadFixedLengthByteArray(flen);
                        files.Add(fname, buffer);
                        continue;
                    case OPCODE_END_OF_DIRECTORY:
                        return new MetadataArchiveWrapper(name, subdirectories, files);
                }
            }
        }
    }
}
