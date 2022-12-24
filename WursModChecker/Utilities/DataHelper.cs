using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WursModChecker.Utilities
{
    public static class DataHelper
    {
        public static string ConvertByteArrayToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public static IList<string> SplitStringToListBySeparator(string data, char separator)
        {
            return data.Split(separator).Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim()).ToList();
        }
    }
}
