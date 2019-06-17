/* 
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// MT940 Customer statement message.
    /// </summary>
    [Serializable]
    public class CustomerStatementMessage
    {

        /// <summary>
        /// Transaction Reference Number (TRN)
        /// In my test set the value of this property is alway ABN AMRO NV
        /// </summary>
        public string TransactionReference { get; private set; }

        /// <summary>
        /// Reference to related message/transaction
        /// (optional)
        /// </summary>
        public string RelatedMessage { get; private set; }

        /// <summary>
        /// Account identification, this is usually the account number of the bank.
        /// </summary>
        public string Account { get; private set; }

        /// <summary>
        /// Statement number is used to identify the message within a set of message of the same account.
        /// </summary>
        /// <see cref="Account"></see>
        public int StatementNumber { get; private set; }

        /// <summary>
        /// Sequence number is used to identify the message within the statement.
        /// See StatementNumber
        /// </summary>
        /// <see cref="StatementNumber"></see>
        public int SequenceNumber { get; private set; }

        /// <summary>
        /// Balance at the start of this message.
        /// </summary>
        public TransactionBalance OpeningBalance { get; private set; }

        /// <summary>
        /// Balance at the end of this message.
        /// </summary>
        public TransactionBalance ClosingBalance { get; private set; }

        /// <summary>
        /// Available balance at the end of this message.
        /// (optional)
        /// </summary>
        public TransactionBalance ClosingAvailableBalance { get; private set; }

        /// <summary>
        /// No idea, but it is in the spec.
        /// (optional)
        /// </summary>
        public TransactionBalance ForwardAvailableBalance { get; private set; }

        /// <summary>
        /// Message description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Collection of financial transactions within this message.
        /// </summary>
        public ICollection<Transaction> Transactions { get; private set; }

        /// <summary>
        /// Creates a new Customer Statement Message. And initialized an empty list
        /// of transactions.
        /// </summary>
        public CustomerStatementMessage()
        {
            Transactions = new List<Transaction>();
        }

        /// <summary>
        /// Creates a new Customer Statement Message based on  the given CustomerStatementMessage.
        /// </summary>
        public CustomerStatementMessage(CustomerStatementMessage customerStatementMessage)
        {
            if(customerStatementMessage == null)
            {
                throw new ArgumentNullException(nameof(customerStatementMessage));
            }

            Account = customerStatementMessage.Account;
            ClosingAvailableBalance = customerStatementMessage.ClosingAvailableBalance;
            ClosingBalance = customerStatementMessage.ClosingBalance;
            Description = customerStatementMessage.Description;
            ForwardAvailableBalance = customerStatementMessage.ForwardAvailableBalance;
            OpeningBalance = customerStatementMessage.OpeningBalance;
            RelatedMessage = customerStatementMessage.RelatedMessage;
            SequenceNumber = customerStatementMessage.SequenceNumber;
            StatementNumber = customerStatementMessage.StatementNumber;
            TransactionReference = customerStatementMessage.TransactionReference;
            Transactions = customerStatementMessage.Transactions;
        }



        /// <summary>
        /// Sets the sequence and statement number for this customer statement message.
        /// </summary>
        /// <param name="transactionData">A string of data formatted as ([0-9]{5})/([0-9]{2,3})</param>
        /// <param name="cultureInfo">The culture to use</param>
        public void SetSequenceNumber(string transactionData, IFormatProvider cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(transactionData))
            {
                throw new ArgumentException("transactionData can not be empty", nameof(transactionData));
            }
            
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }


            var transaction = transactionData.Split('/');

            // First part of this message is the statement number
            if (ValueConverter.TryParseInteger(transaction[0], cultureInfo, out int statementNumber))
            {
                StatementNumber = statementNumber;
            }

            // Second part, if available is the sequence number.
            if (transaction.Length > 1)
            {
                if (ValueConverter.TryParseInteger(transaction[1], cultureInfo, out int sequenceNumber))
                {
                    SequenceNumber = sequenceNumber;
                }
            }
        }

        /// <summary>
        /// Sets the transaction reference and returns a new customer statement message
        /// </summary>
        /// <param name="transactionData"></param>
        /// <returns></returns>
        public CustomerStatementMessage SetTransactionReference(string transactionData)
        {
            return new CustomerStatementMessage(this) { TransactionReference = transactionData };
        }
    
        /// <summary>
        /// Sets the related message and returns a new customer statement message.
        /// </summary>
        public CustomerStatementMessage SetRelatedMessage(string relatedMessage)
        {
            return new CustomerStatementMessage(this) { RelatedMessage = relatedMessage };
        }

        /// <summary>
        /// Sets the account number and returns a new customer statement message.
        /// </summary>
        public CustomerStatementMessage SetAccount(string transactionData, bool clearValue)
        {
            return new CustomerStatementMessage(this) { Account = clearValue ? TextParser.OnlyAlphanumeric(transactionData) : transactionData };
        }

        /// <summary>
        /// Sets the opening balance and returns a new customer statement message.
        /// </summary>
        public CustomerStatementMessage SetOpeningBalance(TransactionBalance transactionBalance)
        {
            return new CustomerStatementMessage(this) { OpeningBalance = transactionBalance };
        }

        /// <summary>
        /// Sets the description and returns a new customer statement message.
        /// </summary>
        public CustomerStatementMessage SetDescription(string transactionData)
        {
            return new CustomerStatementMessage(this) { Description = transactionData };            
        }

        /// <summary>
        /// Sets the closing balance and returns a new customer statement message.
        /// </summary>
        public CustomerStatementMessage SetClosingBalance(TransactionBalance transactionBalance)
        {
            return new CustomerStatementMessage(this) { ClosingBalance = transactionBalance };
        }

        /// <summary>
        /// Sets the closing available balance and returns a new customer statement message.
        /// </summary>
        public CustomerStatementMessage SetClosingAvailableBalance(TransactionBalance transactionBalance)
        {
            return new CustomerStatementMessage(this) { ClosingAvailableBalance = transactionBalance }; 
        }

        /// <summary>
        /// Sets the forward availabe balance and returns a new customer statement message.
        /// </summary>
        public CustomerStatementMessage SetForwardAvailableBalance(TransactionBalance transactionBalance)
        {
            return new CustomerStatementMessage(this) { ForwardAvailableBalance = transactionBalance }; 
            
        }
    }
}