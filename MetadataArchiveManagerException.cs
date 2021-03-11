using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace moe.yo3explorer.sharpBluRay
{
    [Serializable]
    public class MetadataArchiveManagerException : Exception
    {
        public MetadataArchiveManagerException()
        {
        }

        public MetadataArchiveManagerException(string message) : base(message)
        {
        }

        public MetadataArchiveManagerException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MetadataArchiveManagerException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
