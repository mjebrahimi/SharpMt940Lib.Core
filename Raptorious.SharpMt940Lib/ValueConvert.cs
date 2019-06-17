/* 
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/

using System;
using System.Globalization;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// Description of ValueConvert.
    /// </summary>
    public static class ValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo">The culture to use</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "integer")]
        public static int ParseInteger(string value, IFormatProvider cultureInfo)
        {
            if (TryParseInteger(value, cultureInfo, out int result))
                return result;

            throw new InvalidCastException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="integer"></param>
        /// <param name="result"></param>
        /// <param name="cultureInfo">The culture to use</param>
        /// <returns></returns>
        public static bool TryParseInteger(string integer, IFormatProvider cultureInfo, out int result)
        {
            return int.TryParse
                (
                    integer,
                    NumberStyles.Any,
                    cultureInfo,
                    out result
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo">The culture to use</param>
        /// <returns></returns>
        public static decimal ParseDecimal(string value, IFormatProvider cultureInfo)
        {
            if (TryParseDecimal(value, cultureInfo, out decimal result))
                return result;

            throw new InvalidCastException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="result"></param>
        /// <param name="cultureInfo">The culture to use</param>
        /// <returns></returns>
        public static bool TryParseDecimal(string dec, IFormatProvider cultureInfo, out decimal result)
        {
            return decimal.TryParse
                (
                    dec,
                    NumberStyles.Any,
                    cultureInfo,
                    out result
                );
        }
    }
}
