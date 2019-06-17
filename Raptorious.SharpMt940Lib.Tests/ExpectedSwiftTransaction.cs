using System;
using NUnit.Framework;

namespace Raptorious.SharpMt940Lib.Tests
{
    public class ExpectedSwiftTransaction
    {
        public string Description { get; set; }
        public DateTime ValueDate { get; set; }
        public string FundsCode { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Reference { get; set; }
        public DebitCredit DebitCredit { get; set; }
        public string CurrencyCode { get; set; }
        public string SupplementaryDetails { get; set; }
        public string AccountServicingReference { get; set; }

        public static void AssertTransaction(ExpectedSwiftTransaction expected, Transaction actual)
        {
            Assert.That(actual.Amount.Value, Is.EqualTo(expected.Amount), "Amount matches.");
            Assert.That(actual.Amount.Currency.Code, Is.EqualTo(expected.CurrencyCode), "CurrencyCode matches.");
            Assert.That(actual.DebitCredit, Is.EqualTo(expected.DebitCredit), "DebitCredit matches.");
            Assert.That(actual.Description, Is.EqualTo(expected.Description), "Description matches.");
            Assert.That(actual.FundsCode, Is.EqualTo(expected.FundsCode), "FundsCode matches.");
            Assert.That(actual.Reference, Is.EqualTo(expected.Reference), "Reference matches.");
            Assert.That(actual.TransactionType, Is.EqualTo(expected.TransactionType), "TransactionType matches.");
            Assert.That(actual.ValueDate, Is.EqualTo(expected.ValueDate), "ValueDate matches.");
            Assert.That(actual.SupplementaryDetails, Is.EqualTo(expected.SupplementaryDetails), "SupplementaryDetails matches.");
            Assert.That(actual.AccountServicingReference, Is.EqualTo(expected.AccountServicingReference),
                "AccountServicingReference matches.");
        }

        public static ExpectedSwiftTransaction CreateExpected(DateTime valueDate, DebitCredit debitCredit, string fundsCode,
            string currencyCode, decimal amount, string transactionType, string reference, string description, string supplementaryDetails,
            string servicingReference = "")
        {
            return new ExpectedSwiftTransaction
            {
                DebitCredit = debitCredit,
                Amount = amount,
                CurrencyCode = currencyCode,
                Description = description,
                ValueDate = valueDate,
                FundsCode = fundsCode,
                Reference = reference,
                TransactionType = transactionType,
                SupplementaryDetails = supplementaryDetails,
                AccountServicingReference = servicingReference
            };
        }
    }
}