using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Extension
{
    public static class Extensions
    {
        public static string ToUniversalIso8601(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("u").Replace(" " ,"T");
        }
    }
}
