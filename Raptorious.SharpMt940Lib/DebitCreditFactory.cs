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
    public static class DebitCreditFactory
    {
        /// <summary>
        /// Returns the correct EnumFlag for the correct given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DebitCredit Create(string type)
        {
            switch (type)
            {
                case "D":
                    return DebitCredit.Debit;
                case "C":
                    return DebitCredit.Credit;
                case "RC":
                    return DebitCredit.RC;
                case "RD":
                    return DebitCredit.RD;
            }

            throw new ArgumentException(type, nameof(type));
        }
    }
}
