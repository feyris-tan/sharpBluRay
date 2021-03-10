using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.Model.IndexModel;

namespace moe.yo3explorer.sharpBluRay.Model
{
    public class Index
    {
        private Index() {}

        internal Index(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            if (!ms.ReadFixedLengthString(4).Equals("INDX"))
                throw new InvalidDataException("index.bdmv is not valid!");

            Version = int.Parse(ms.ReadFixedLengthString(4));

            int indexAddr = ms.ReadInt32BE();
            int extAddr = ms.ReadInt32BE();
            ms.Position += 24;

            byte[] appInfoBuffer = new byte[indexAddr - (4 * 4 + 24)];
            ms.ReadFully(appInfoBuffer);
            AppInfo = new AppInfo(appInfoBuffer);

            int indexSize = extAddr == 0 ? (int)ms.Length - indexAddr : extAddr - indexAddr;
            byte[] indicesBytes = new byte[indexSize];
            ms.ReadFully(indicesBytes);
            Indices = new Indices(indicesBytes);

            if (extAddr != 0)
            {
                throw new NotImplementedException("index.bdmv extensions");
            }
        }
        

        public int Version { get; private set; }
        public AppInfo AppInfo { get; private set; }
        public Indices Indices { get; private set; }

        public bool HasMovieObjects()
        {
            if (Indices.FirstPlayback.ObjectType == IndexObjectType.MOVIE_OBJECT)
                return true;
            if (Indices.TopMenu.ObjectType == IndexObjectType.MOVIE_OBJECT)
                return true;
            foreach (Title title in Indices.Titles)
            {
                if (title.IndexObject.ObjectType == IndexObjectType.MOVIE_OBJECT)
                    return true;
            }

            return false;
        }
    }
}
