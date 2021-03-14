using System;
using System.Runtime.Serialization;

namespace moe.yo3explorer.sharpBluRay.Languages
{
    [Serializable]
    public class UnknownLanguageCodeException : Exception
    {
        public UnknownLanguageCodeException()
        {
        }

        public UnknownLanguageCodeException(string message) : base(message)
        {
        }

        public UnknownLanguageCodeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnknownLanguageCodeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
