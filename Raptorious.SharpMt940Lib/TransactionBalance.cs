/* 
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TransactionBalance
    {
        /// <summary>
        /// 
        /// </summary>
        public DebitCredit DebitCredit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EntryDate { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Currency Currency { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Money Balance { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        [Obsolete("Use the TransactionBalance with cultureinfo")]
        public TransactionBalance(string data) : this(data, CultureInfo.CurrentCulture)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("data can not be empty", data);
            }            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cultureInfo">The culture to use</param>
        public TransactionBalance(string data, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("data can not be empty", data);
            }
            
            if(cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }
            
            var regex = new Regex(@"([C|D]{1})([0-9]{2})([0-9]{2})([0-9]{2})([A-Z]{3})(\d.*)");
            var match = regex.Match(data);

            if (match.Groups.Count < 7)
                throw new System.Data.InvalidExpressionException(data);

            DebitCredit = DebitCreditFactory.Create(match.Groups[1].Value);

            EntryDate = ParseDate(match, cultureInfo);

            Currency = new Currency(match.Groups[5].Value);
            Balance = new Money(match.Groups[6].Value, Currency, cultureInfo);
        }

        private static DateTime ParseDate(Match match, CultureInfo cultureInfo)
        {
            return DateParser.ParseDate
                (
                    match.Groups[2].Value,
                    match.Groups[3].Value,
                    match.Groups[4].Value,
                    cultureInfo
                );
        }

        /// <summary>
        /// Returns the string represantion of the transaction balance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Balance.ToString();
        }
    }
}