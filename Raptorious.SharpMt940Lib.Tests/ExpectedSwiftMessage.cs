using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Raptorious.SharpMt940Lib.Tests
{
    public class ExpectedSwiftMessage
    {
        public decimal OpeningBalance { get; set; }
        public string Account { get; set; }
        public string TransactionReference { get; set; }
        public string OpeningBalanceCurrencyCode { get; set; }
        public DebitCredit OpeningBalanceDebitCredit { get; set; }
        public DateTime OpeningBalanceDate { get; set; }
        public int StatementNumber { get; set; }
        public int SequenceNumber { get; set; }
        public string ClosingBalanceCurrencyCode { get; set; }
        public DebitCredit ClosingBalanceDebitCredit { get; set; }
        public DateTime ClosingBalanceDate { get; set; }
        public string ClosingAvailableBalanceCurrencyCode { get; set; }
        public DebitCredit ClosingAvailableBalanceDebitCredit { get; set; }
        public DateTime ClosingAvailableBalanceDate { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal ClosingAvailableBalance { get; set; }
        public string Description { get; set; }
        public int TransactionsCount { get; set; }

        public static void AssertCustomerStatementMessage(IList<CustomerStatementMessage> dataList, int expectedMessageCount,
            params ExpectedSwiftMessage[] expectedList)
        {
            Assert.That(dataList.Count, Is.EqualTo(expectedMessageCount), "Returned correct number of records.");
            for (var i = 0; i < expectedList.Length; i++)
            {
                var message = dataList[i];
                var expected = expectedList[i];
                Assert.That(message.Description, Is.EqualTo(expected.Description), "Description matches");
                Assert.That(message.Account, Is.EqualTo(expected.Account), "Account matches");
                Assert.That(message.TransactionReference, Is.EqualTo(expected.TransactionReference), "TransactionReference matches");
                Assert.That(message.OpeningBalance.Balance.Value, Is.EqualTo(expected.OpeningBalance), "OpeningBalance Value is correct");
                Assert.That(message.OpeningBalance.Balance.Currency.Code, Is.EqualTo(expected.OpeningBalanceCurrencyCode),
                    "OpeningBalance Currency is correct");
                Assert.That(message.OpeningBalance.DebitCredit, Is.EqualTo(expected.OpeningBalanceDebitCredit),
                    "OpeningBalance DebitCredit is correct");
                Assert.That(message.OpeningBalance.EntryDate, Is.EqualTo(expected.OpeningBalanceDate), "OpeningBalance EntryDate is correct");
                Assert.That(message.StatementNumber, Is.EqualTo(expected.StatementNumber), "StatementNumber is correct");
                Assert.That(message.SequenceNumber, Is.EqualTo(expected.SequenceNumber), "SequenceNumber is correct");
                Assert.That(message.ClosingBalance.Balance.Value, Is.EqualTo(expected.ClosingBalance), "ClosingBalance Value is correct");
                Assert.That(message.ClosingBalance.Balance.Currency.Code, Is.EqualTo(expected.ClosingBalanceCurrencyCode),
                    "ClosingBalance Currency is correct");
                Assert.That(message.ClosingBalance.DebitCredit, Is.EqualTo(expected.ClosingBalanceDebitCredit),
                    "ClosingBalance DebitCredit is correct");
                Assert.That(message.ClosingBalance.EntryDate, Is.EqualTo(expected.ClosingBalanceDate), "ClosingBalance EntryDate is correct");
                Assert.That(message.ClosingAvailableBalance.Balance.Value, Is.EqualTo(expected.ClosingAvailableBalance),
                    "ClosingAvailableBalance Value is correct");
                Assert.That(message.ClosingAvailableBalance.Balance.Currency.Code, Is.EqualTo(expected.ClosingAvailableBalanceCurrencyCode),
                    "ClosingAvailableBalance Currency is correct");
                Assert.That(message.ClosingAvailableBalance.DebitCredit, Is.EqualTo(expected.ClosingAvailableBalanceDebitCredit),
                    "ClosingAvailableBalance DebitCredit is correct");
                Assert.That(message.ClosingAvailableBalance.EntryDate, Is.EqualTo(expected.ClosingAvailableBalanceDate),
                    "ClosingAvailableBalance EntryDate is correct");
                Assert.That(message.Transactions.Count, Is.EqualTo(expected.TransactionsCount), "Correct number of transactions.");
            }
        }

        public static void AssertCustomerStatementTransactions(CustomerStatementMessage message,
            List<ExpectedSwiftTransaction> expectedTransactions)
        {
            Assert.That(message.Transactions.Count, Is.EqualTo(expectedTransactions.Count), "Correct number of transactions.");
            for (var i = 0; i < expectedTransactions.Count(); i++)
            {
                var expected = expectedTransactions[i];
                var actual = message.Transactions.ToList()[i];
                ExpectedSwiftTransaction.AssertTransaction(expected, actual);
            }
        }
    }
}