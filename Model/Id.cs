using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using moe.yo3explorer.sharpBluRay.ComponentModel;

namespace moe.yo3explorer.sharpBluRay.Model
{
    [TypeConverter(typeof(IdTypeConverter))]
    public class Id
    {
        internal Id(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer, false);
            if (!ms.ReadFixedLengthString(4).Equals("BDID"))
                throw new InvalidDataException("BDID corrupted");
            Version = int.Parse(ms.ReadFixedLengthString(4));

            ms.Position += 32;
            OrganisationId = ms.ReadUInt32BE();
            DiscId = new Guid(ms.ReadFixedLengthByteArray(16));
        }

        public Guid DiscId { get; private set; }

        public uint OrganisationId { get; private set; }

        public int Version { get; }
    }
}
