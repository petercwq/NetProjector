using System.Drawing;
using System.IO;

namespace NetProjector.Pc
{
    static class Utils
    {
        public static Image GetImageFrom(byte[] bytes)
        {
            return Bitmap.FromStream(new MemoryStream(bytes));
        }
    }
}
