using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.FilesystemAbstraction;
using moe.yo3explorer.sharpBluRay.Model;

namespace moe.yo3explorer.sharpBluRay
{
    public class BluRayDiscMovie
    {
        public static IEnumerable<DirectoryInfoWrapper> FindPhysicalBluRayDiscMovies()
        {
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in driveInfos)
            {
                if (!driveInfo.IsReady)
                    continue;
                if (driveInfo.DriveType != DriveType.CDRom)
                    continue;
                DirectoryInfo certificate = new DirectoryInfo(Path.Combine(driveInfo.RootDirectory.FullName, "CERTIFICATE"));
                if (!certificate.Exists)
                    continue;
                DirectoryInfo bdmv = new DirectoryInfo(Path.Combine(driveInfo.RootDirectory.FullName, "BDMV"));
                if (!bdmv.Exists)
                    continue;
                FileInfo indexBdmv = new FileInfo(Path.Combine(bdmv.FullName, "index.bdmv"));
                if (!indexBdmv.Exists)
                    continue;
                yield return new DirectoryInfoWrapper(driveInfo.RootDirectory);
            }
        }

        public static BluRayDiscMovie OpenFirstPhysicalBluRayDiscMovie()
        {
            IEnumerator<DirectoryInfoWrapper> enumerator = FindPhysicalBluRayDiscMovies().GetEnumerator();
            if (!enumerator.MoveNext())
                return null;
            DirectoryInfoWrapper first = enumerator.Current;
            if (first == null)
                return null;
            return new BluRayDiscMovie(first);
        }

        public BluRayDiscMovie(IDirectoryAbstraction source)
        {
            RootDirectory = source;
            if (!source.TestForSubdirectory("BDMV"))
                throw new DirectoryNotFoundException("BDMV directory not found!");
            IDirectoryAbstraction bdmvRoot = source.OpenSubdirectory("BDMV");

            if (!bdmvRoot.TestForFile("index.bdmv"))
                throw new FileNotFoundException("BDMV index not found!", "index.bdmv");
            byte[] indexBdmv = bdmvRoot.ReadFileCompletely("index.bdmv");
            Index = new Index(indexBdmv);

            if (Index.HasMovieObjects())
            {
                byte[] movieObjectBdmv = bdmvRoot.ReadFileCompletely("MovieObject.bdmv");
                MovieObject = new MovieObject(movieObjectBdmv);
                bool playlistDirExists = bdmvRoot.TestForSubdirectory("PLAYLIST");

                if (MovieObject.HasPlaylistDependencies() && playlistDirExists)
                {
                    ReadOnlyCollection<uint> readOnlyCollection = MovieObject.GetPlaylistDependencies();
                    Playlists = new Playlist[readOnlyCollection.Count];
                    IDirectoryAbstraction playlistDir = bdmvRoot.OpenSubdirectory("PLAYLIST");
                    for (int i = 0; i < readOnlyCollection.Count; i++)
                    {
                        string fname = String.Format("{0:00000}.mpls", readOnlyCollection[i]);
                        if (!playlistDir.TestForFile(fname))
                            continue;
                        byte[] mplsBuffer = playlistDir.ReadFileCompletely(fname);
                        Playlists[i] = new Playlist(mplsBuffer);
                    }
                }
            }

            if (source.TestForSubdirectory("CERTIFICATE"))
            {
                IDirectoryAbstraction certificate = source.OpenSubdirectory("CERTIFICATE");
                if (certificate.TestForFile("id.bdmv"))
                {
                    byte[] idBdmv = certificate.ReadFileCompletely("id.bdmv");
                    Id = new Id(idBdmv);
                }
            }
        }
        
        public IDirectoryAbstraction RootDirectory { get; private set; }
        public Index Index { get; private set; }
        public MovieObject MovieObject { get; private set; }
        public Playlist[] Playlists { get; private set; }
        public Id Id { get; private set; }
    }
}
