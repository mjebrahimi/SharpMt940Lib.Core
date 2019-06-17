/* 
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/

using System;

namespace Raptorious.SharpMt940Lib.Mt940Format
{
    /// <summary>
    /// Description of Header.
    /// </summary>
    public class Separator
    {
        /// <summary>
        /// 
        /// </summary>
        public string Data
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int LineCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public Separator(params string[] data)
        {
            if(data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if(data.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data));
            }            
            
            LineCount = data.Length;
            Data = string.Join(Environment.NewLine, data);
        }
    }
}
