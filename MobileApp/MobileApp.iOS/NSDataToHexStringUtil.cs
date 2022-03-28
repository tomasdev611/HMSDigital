using System.Text;
using Foundation;

namespace MobileApp.iOS
{
    public static class NSDataToHexStringUtil
    {
        internal static string ToHexString(this NSData data)
        {
            var bytes = data.ToArray();

            if (bytes == null)
            {
                return null;
            }

            var sb = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString().ToUpperInvariant();
        }
    }
}
