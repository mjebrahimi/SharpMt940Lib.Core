/* 
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/

using System;
using System.Text.RegularExpressions;
using System.Data;
using System.Globalization;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Transaction
    {
        /// <summary>
        /// Unparsed raw data.
        /// Code 61.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Description of the transaction.
        /// Code 86
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ??
        /// </summary>
        public DateTime ValueDate { get; private set; }

        /// <summary>
        /// Optional date.
        /// </summary>
        public DateTime? EntryDate { get; private set; }

        /// <summary>
        /// ??
        /// </summary>
        public string FundsCode { get; set; }

        /// <summary>
        /// Transaction amount
        /// </summary>
        public Money Amount { get; private set; }

        /// <summary>
        /// Transaction type, a value that starts with N and is followed by 3 numbers.
        /// </summary>
        public string TransactionType { get; private set; }

        /// <summary>
        /// NONREF or account number of the other party.
        /// </summary>
        public string Reference { get; private set; }

        /// <summary>
        /// Debit-credit indication
        /// </summary>
        public DebitCredit DebitCredit { get; private set; }

        /// <summary>
        /// Description details (see MT940Params)
        /// </summary>
        public TransactionDetails Details { get; set; }

        /// <summary>
        /// Subfield 9 Supplementary Details (optional)
        /// </summary>
        public string SupplementaryDetails { get; private set; }

        /// <summary>
        /// Subfield 8 [//16x] (Reference of the Account Servicing Institution)
        /// </summary>
        public string AccountServicingReference { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currency">The currency that is used in this transaction.</param>
        public Transaction(string data, Currency currency)
            : this(data, currency, CultureInfo.CurrentCulture)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("data can not be empty", nameof(data));
            }

            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currency">The currency that is used in this transaction.</param>
        /// <param name="cultureInfo">The culture to use</param>
        public Transaction(string data, Currency currency, CultureInfo cultureInfo)
        {
            if(string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("data can not be empty", nameof(data));
            }

            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }
            

            // TODO: Finish/Fix regex
            // @See: https://bitbucket.org/raptux/sharpmt940lib/issue/1/regex-problem-in-transactioncs
            
            // not done.
            //Regex regex = new Regex(@"^(?<valuedate>(?<year>\d{2})(?<month>\d{2})(?<day>\d{2}))(?<entrydate>(?<entrymonth>\d{2})(?<entryday>\d{2}))?(?<creditdebit>C|D|RC|RD)(?<fundscode>[A-z]{0,1}?)(?<ammount>\d*,\d{0,2})(?<transactiontype>[\w\s]{4})(?<reference>[\s\w]{0,16})");

            var regex = new Regex(@"^(?<valuedate>(?<year>\d{2})(?<month>\d{2})(?<day>\d{2}))(?<entrydate>(?<entrymonth>\d{2})(?<entryday>\d{2}))?(?<creditdebit>C|D|RC|RD)(?<fundscode>[A-z]{0,1}?)(?<ammount>\d*[,.]\d{0,2})(?<transactiontype>[\w\s]{4})(?<reference>[\s\w]{0,16})(?:(?<servicingreference>//[\s\w]{0,16}))*(?<supplementary>\r\n[\s\w]{0,34})*");


            var match = regex.Match(data);

            if (!match.Success)
            {
                throw new InvalidExpressionException(data);
            }

            // Raw line.
            Value = data;

            Details = new TransactionDetails();
            ValueDate = ExtractValueDate(match, cultureInfo);
            EntryDate = ExtractEntryDate(match, cultureInfo);
            DebitCredit = ExtractDebitCredit(match);

            FundsCode = match.Groups["fundscode"].Value;

            Amount = new Money(match.Groups["ammount"].Value, currency, cultureInfo);

            TransactionType = match.Groups["transactiontype"].Value;
            Reference = match.Groups["reference"].Value;

            AccountServicingReference = match.Groups["servicingreference"].Value.Trim("//".ToCharArray());
            SupplementaryDetails = match.Groups["supplementary"].Value.Trim("\r\n".ToCharArray());
        }

        static DebitCredit ExtractDebitCredit(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            string debitCreditCode = match.Groups["creditdebit"].Value;
            var debitCredit = DebitCreditFactory.Create(debitCreditCode);
            return debitCredit;
        }

        static DateTime ExtractValueDate(Match match, CultureInfo cultureInfo)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            return DateParser.ParseDate(match.Groups["year"].Value, match.Groups["month"].Value, match.Groups["day"].Value, cultureInfo);
        }

        static DateTime? ExtractEntryDate(Match match, CultureInfo cultureInfo)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            if(!match.Groups["entrydate"].Success)
            {
                return null;
            }

            var entryYear = ExtractEntryYear(match);
            var entryMonth = ExtractEntryMonth(match);
            var entryDay = ExtractEntryDay(match);

            var entryDate = DateParser.ParseDate(entryYear, entryMonth, entryDay, cultureInfo);
            return entryDate;
        }

        static string ExtractEntryDay(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var entryDay = match.Groups["entryday"].Value;

            if (string.IsNullOrWhiteSpace(entryDay))
                throw new InvalidExpressionException(match.Value);

            return entryDay;
        }

        static string ExtractEntryMonth(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var entryMonth = match.Groups["entrymonth"].Value;
            return entryMonth;
        }

        static string ExtractEntryYear(Match match)
        {
            if(match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            string entryYear = match.Groups["year"].Value;
            return entryYear;
        }
    }
}
