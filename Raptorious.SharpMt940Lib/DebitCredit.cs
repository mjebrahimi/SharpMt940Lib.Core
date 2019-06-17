/* 
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/

using System;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum DebitCredit
    {
        /// <summary>
        /// 
        /// </summary>
        Debit,
        /// <summary>
        /// 
        /// </summary>
        Credit,
        /// <summary>
        /// 
        /// </summary>
        RC, // What are these? - R*?Credit
        /// <summary>
        /// 
        /// </summary>
        RD  // What are these? - R*?Debit
    };
}
