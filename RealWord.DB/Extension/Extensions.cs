using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RealWord.DB.Extension
{
    public static class Extensions
    {
        public static string ToUniversalIso8601(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.sssZ" ,CultureInfo.InvariantCulture);
            // dateTime.Utc.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz" ,CultureInfo.InvariantCulture);
        }
    }
}
