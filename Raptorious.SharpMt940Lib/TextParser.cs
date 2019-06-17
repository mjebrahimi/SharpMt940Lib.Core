using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// Responsible for parsing strings
    /// </summary>
    public static class TextParser
    {
        /// <summary>
        /// Remove non alphanumeric characters
        /// </summary>
        /// <param name="value"></param>
        /// <returns>return olny alphanumeric characters</returns>
        public static string OnlyAlphanumeric(string value) 
        {
            return Regex.Replace(value, @"[^A-Za-z0-9]+", "");
        }
    }
}
