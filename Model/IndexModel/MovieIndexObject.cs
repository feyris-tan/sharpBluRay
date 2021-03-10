using System.IO;

namespace moe.yo3explorer.sharpBluRay.Model.IndexModel
{
    class MovieIndexObject : IndexObject
    {
        internal MovieIndexObject(Stream s)
        {
            int playback = s.ReadInt8();
            playback = ((playback & 0xc0 >> 6));
            PlaybackType = (MovieIndexObjectType) playback;

            s.Position++;

            Name = s.ReadUInt16();

            s.Position += 4;
        }

        public IndexObjectType ObjectType => IndexObjectType.MOVIE_OBJECT;
        public MovieIndexObjectType PlaybackType { get; private set; }
        public ushort Name { get; private set; }

        public override string ToString()
        {
            return $"{nameof(PlaybackType)}: {PlaybackType}, {nameof(Name)}: {Name}";
        }
    }
}
