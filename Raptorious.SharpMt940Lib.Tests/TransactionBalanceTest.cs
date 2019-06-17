/*
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/

using System;
using NUnit.Framework;
using System.Data;
using System.Globalization;

namespace Raptorious.SharpMt940Lib.Mt940Format
{
    [TestFixture]
    public class TransactionBalanceTest
    {
        CultureInfo _fr = CultureInfo.GetCultureInfo("fr-FR");

        [Test]
        public void Test()
        {
            const string data = "C140626EUR12062,29";
            var balance = new TransactionBalance(data, _fr);

            var currency = new Currency("EUR");
            Assert.That(balance.Balance, Is.EqualTo(new Money("12062,29", currency, _fr)));

        }

        [Test]
        public void Credit_100_Eur_On_20_06_2011_Test()
        {
            const string data = "C110620EUR100";
            var balance = new TransactionBalance(data, _fr);

            var currency = new Currency("EUR");

            Assert.AreEqual(new Money("100", currency, _fr), balance.Balance);
            Assert.AreEqual(new Money("100", currency, _fr).Value, balance.Balance.Value);
            Assert.AreEqual(new DateTime(2011, 06, 20), balance.EntryDate);
            Assert.AreEqual(currency, balance.Currency);
            Assert.AreEqual(DebitCredit.Credit, balance.DebitCredit);
        }

        [Test]
        public void BadDataInsertedTest()
        {
            const string data = "JIBBERISH!";
            Assert.Throws<InvalidExpressionException>(() => new TransactionBalance(data, _fr));
        }

        [Test]
        public void Credit_100_EUR_On_20_16_2011_Test()
        {
            const string data = "C111620EUR100";
            Assert.Throws<ArgumentOutOfRangeException>(() => new TransactionBalance(data, _fr));
        }
    }
}
