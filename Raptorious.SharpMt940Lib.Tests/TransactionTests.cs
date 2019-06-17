using NUnit.Framework;
using System;
using System.Data;
using Raptorious.SharpMt940Lib.Mt940Format;

namespace Raptorious.SharpMt940Lib.Tests
{
    [TestFixture]
    public class TransactionTests
    {
        [Test, Category("Issue2")]
        public void EntryMonthAndEntryDayAvailableText()
        {
            var transaction = new Transaction("1312231223DR0,95N011NONREF", new Currency("EUR"));
                        
            Assert.That(transaction.EntryDate.HasValue, Is.True);
            Assert.That(transaction.EntryDate.Value, Is.EqualTo(new DateTime(2013, 12, 23)));
            Assert.That(transaction.ValueDate, Is.EqualTo(new DateTime(2013, 12, 23)));
        }

        [Test, Category("Issue2")]
        public void EntryMonthAndEntryDayOptionalText()
        {
            var transaction = new Transaction("131223DR0,95N011NONREF", new Currency("EUR"));            
            
            Assert.That(transaction.EntryDate, Is.Null);
            Assert.That(transaction.EntryDate.HasValue, Is.False);
            Assert.That(transaction.ValueDate, Is.EqualTo(new DateTime(2013, 12, 23)));
        }

        [Test, Category("Issue2")]
        public void EntryMonthOptionalText()
        {
            Assert.Throws<InvalidExpressionException>(() => 
                new Transaction("13122323DR0,95N011NONREF", new Currency("EUR")));            
        }

        [Test, Category("Issue2")]
        public void EntryDayOptionalText()
        {
            Assert.Throws<InvalidExpressionException>(() => 
                new Transaction("13122312DR0,95N011NONREF", new Currency("EUR")));
        }

        [Test]
        public void CreditDebitIsParsedCorrectly_Debit()
        {
            var transaction = new Transaction("1312231223DR0,95N011NONREF", new Currency("EUR"));

            Assert.AreEqual(transaction.DebitCredit, DebitCredit.Debit);
            Assert.AreEqual(transaction.FundsCode, "R");
        }

        [Test]
        public void CreditDebitIsParsedCorrectly_Credit()
        {
            var transaction = new Transaction("1407010701C388,15NDIV0597894779", new Currency("EUR"));

            Assert.AreEqual(transaction.DebitCredit, DebitCredit.Credit);
            Assert.AreEqual(transaction.FundsCode, "");
        }

        [Test]
        public void WithSupplementaryDetails_IsParsedCorrectly()
        {
            var expectedTransaction = ExpectedSwiftTransaction.CreateExpected(new DateTime(2015, 1, 26), DebitCredit.Credit,
                "R", "EUR", 18790.00M, "NTRF", "PON0000002534162", null, "Inward Payment", "");

            var transaction = new Transaction("1501260126CR18790,00NTRFPON0000002534162\r\nInward Payment", new Currency("EUR"), Mt940Test.NlCultureInfo);

            ExpectedSwiftTransaction.AssertTransaction(expectedTransaction, transaction);
        }

        [Test]
        public void WithSupplementaryDetailsAndServiceReference_IsParsedCorrectly()
        {
            var expectedTransaction = ExpectedSwiftTransaction.CreateExpected(new DateTime(2015, 1, 26), DebitCredit.Credit,
                "R", "EUR", 18790.00M, "NTRF", "PON0000002534162", null, "Inward Payment", "GBG260150R2ETGXS");

            var transaction = new Transaction("1501260126CR18790,00NTRFPON0000002534162//GBG260150R2ETGXS\r\nInward Payment", new Currency("EUR"), Mt940Test.NlCultureInfo);

            ExpectedSwiftTransaction.AssertTransaction(expectedTransaction, transaction);
        }

        [Test]
        public void WithoutSupplementaryDetailsOrServiceReference_IsParsedCorrectly()
        {
            var expectedTransaction = ExpectedSwiftTransaction.CreateExpected(new DateTime(2015, 1, 26), DebitCredit.Credit,
                "R", "EUR", 18790.00M, "NTRF", "PON0000002534162", null, "", "");

            var transaction = new Transaction("1501260126CR18790,00NTRFPON0000002534162", new Currency("EUR"), Mt940Test.NlCultureInfo);

            ExpectedSwiftTransaction.AssertTransaction(expectedTransaction, transaction);
        }
    }
}
