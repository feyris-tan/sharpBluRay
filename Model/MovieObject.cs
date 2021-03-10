using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.Model.MovieObjectModel;

namespace moe.yo3explorer.sharpBluRay.Model
{
    public class MovieObject
    {
        internal MovieObject(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer, false);
            if (!ms.ReadFixedLengthString(4).Equals("MOBJ"))
                throw new InvalidDataException("MovieObject.bdmv is invalid.");

            Version = Int32.Parse(ms.ReadFixedLengthString(4));
            if (Version != 100 && Version != 200)
                throw new InvalidDataException("Unsupported MovieObject.bdmv version.");

            int extensionAddr = ms.ReadInt32BE();
            ms.Position += 28;

            int movieObjectsLength = extensionAddr == 0
                ? (int) ms.Length - (int) ms.Position
                : extensionAddr - (int) ms.Position;
            byte[] movieObjectsBuffer = ms.ReadFixedLengthByteArray(movieObjectsLength);
            readMovieObjects(movieObjectsBuffer);

            if (extensionAddr != 0)
                throw new NotImplementedException("movie object extensions");
        }

        private void readMovieObjects(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer, false);
            int totalLen = ms.ReadInt32BE();
            int assumedLen = buffer.Length - 4;
            Debug.WriteLineIf(totalLen != assumedLen, "Length of Movie object is funky...");
            int reserved = ms.ReadInt32BE();
            ushort numObjects = ms.ReadUInt16();
            MovieObjects = new MovieObjectEntry[numObjects];
            for (ushort i = 0; i < numObjects; i++)
            {
                MovieObjects[i] = new MovieObjectEntry(ms);
            }

        }

        public int Version { get; private set; }
        public MovieObjectEntry[] MovieObjects { get; private set; }

        public bool HasPlaylistDependencies()
        {
            foreach (var movieObjectEntry in MovieObjects)
            {
                foreach (NavigationCommand navigationCommand in movieObjectEntry.NavigationCommands)
                {
                    if (navigationCommand.Group != Group.Branch)
                        continue;
                    if (navigationCommand.BranchOptionsPlay != GroupBranchPlay.PlayPl)
                        continue;
                    if (navigationCommand.IFlagOperand1)
                        return true;
                }
            }

            return false;
        }

        private List<uint> _playlistDependencies;
        public ReadOnlyCollection<uint> GetPlaylistDependencies()
        {
            if (_playlistDependencies != null)
                return new ReadOnlyCollection<uint>(_playlistDependencies);

            _playlistDependencies = new List<uint>();
            foreach (var movieObjectEntry in MovieObjects)
            {
                foreach (NavigationCommand navigationCommand in movieObjectEntry.NavigationCommands)
                {
                    if (navigationCommand.Group != Group.Branch)
                        continue;
                    if (navigationCommand.BranchOptionsPlay != GroupBranchPlay.PlayPl)
                        continue;
                    if (!navigationCommand.IFlagOperand1)
                        continue;
                    uint destination = navigationCommand.Destination;
                    if (!_playlistDependencies.Contains(destination))
                        _playlistDependencies.Add(destination);
                }
            }
            _playlistDependencies.Sort();
            return GetPlaylistDependencies();
        }
    }
}