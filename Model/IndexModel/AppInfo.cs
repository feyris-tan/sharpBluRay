using System.Diagnostics;
using System.IO;

namespace moe.yo3explorer.sharpBluRay.Model.IndexModel
{
    public class AppInfo
    {
        internal AppInfo(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer, false);
            int length = ms.ReadInt32BE();
            if (length != 34)
            {
                Debug.Print("Unexpected AppInfo Length!");
            }

            ms.Position += 2;
            ContentProviderData = ms.ReadFixedLengthByteArray(32);
        }

        public byte[] ContentProviderData { get; private set; }
    }
}
