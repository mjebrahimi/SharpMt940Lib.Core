
using NUnit.Framework;
using Raptorious.SharpMt940Lib.Mt940Format;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Raptorious.SharpMt940Lib.Tests
{
    [TestFixture]
    public class FullParseTests
    {
        private IMt940Format SnsFormat
        {
            get
            {
                var header = new Separator("{1:F01SNSBNL2AXXXX0000000000}{2:O94000000000000000000000000000000000000000N}{3:}{4:");
                var footer = new Separator("-}{5:}");
                return new GenericFormat(header, footer);
            }
        }

        private IMt940Format StandardBankFormat
        {
            get
            {
                var header = new Separator("1:Fxxxxxxxxxxxxxxxxxxxxxxxx}{2:Ixxxxxxxxxxxxxxxxx{3:{108:xxxxx}}{4:");
                var footer = new Separator("-}");
                return new GenericFormat(header, footer);
            }
        }

        public FullParseTests()
        {
        }

        [Test, Category("FullParserTests")]
        public void AbnAmro_US_ServerEnvironment()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture 
                = System.Globalization.CultureInfo.GetCultureInfo("en-us");

            var nlNL = CultureInfo.GetCultureInfo("nl-NL");

            var header = new Separator("ABNANL2A", "940", "ABNANL2A");
            var footer = new Separator("-");

            var format = new GenericFormat(header, footer);
            var sample = GetSample(format, "Raptorious.SharpMt940Lib.Tests.Samples.abnamro.txt", nlNL);

            Assert.That(sample.Count, Is.EqualTo(4));


            var csm1 = sample.Skip(0).Take(1).First();

            #region Customer Statement Message 1
            Assert.That(csm1.Account, Is.EqualTo("500950253"));
            Assert.That(csm1.ClosingAvailableBalance, Is.Null);
            Assert.That(csm1.ClosingBalance.Balance, Is.EqualTo(new Money("17159,49", new Currency("EUR"), nlNL)));
            Assert.That(csm1.ClosingBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm1.ClosingBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm1.ClosingBalance.EntryDate, Is.EqualTo(new DateTime(2009, 06, 24)));
            Assert.That(csm1.Description, Is.Null);
            Assert.That(csm1.ForwardAvailableBalance, Is.Null);
            Assert.That(csm1.OpeningBalance.Balance, Is.EqualTo(new Money("17369,99", new Currency("EUR"), nlNL)));
            Assert.That(csm1.OpeningBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm1.OpeningBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm1.OpeningBalance.EntryDate, Is.EqualTo(new DateTime(2009, 06, 23)));
            Assert.That(csm1.RelatedMessage, Is.Null);
            Assert.That(csm1.SequenceNumber, Is.EqualTo(1));
            Assert.That(csm1.StatementNumber, Is.EqualTo(17501));
            Assert.That(csm1.TransactionReference, Is.EqualTo("ABN AMRO BANK NV"));
            Assert.That(csm1.Transactions.Count, Is.EqualTo(2));

            var transaction = csm1.Transactions.Skip(0).Take(1).First();
            Assert.That(transaction.Amount, Is.EqualTo(new Money("7,5", new Currency("EUR"), nlNL)));
            Assert.That(transaction.DebitCredit, Is.EqualTo(DebitCredit.Debit));
            Assert.That(transaction.Description, Is.EqualTo(@"GIRO   428428 KPN BV             BETALINGSKENM.  000000018503995
5109227317                       BETREFT FACTUUR D.D. 20-06-2009
INCL. 1,20 BTW"));
            Assert.That(transaction.EntryDate, Is.EqualTo(new DateTime(2009, 06, 24)));
            Assert.That(transaction.FundsCode, Is.EqualTo(string.Empty));
            Assert.That(transaction.Reference, Is.EqualTo("NONREF"));
            Assert.That(transaction.TransactionType, Is.EqualTo("N192"));
            Assert.That(transaction.Value, Is.EqualTo("0906230624D7,5N192NONREF"));
            Assert.That(transaction.ValueDate, Is.EqualTo(new DateTime(2009, 06, 23)));

            var transaction2 = csm1.Transactions.Skip(1).Take(1).First();
            Assert.That(transaction2.Amount, Is.EqualTo(new Money("203", new Currency("EUR"), nlNL)));
            Assert.That(transaction2.DebitCredit, Is.EqualTo(DebitCredit.Debit));
            Assert.That(transaction2.Description, Is.EqualTo(@"BEA               23.06.09/22.22 VILLA DORIA SCHOTEN,PAS590"));
            Assert.That(transaction2.EntryDate, Is.EqualTo(new DateTime(2009, 06, 24)));
            Assert.That(transaction2.FundsCode, Is.EqualTo(string.Empty));
            Assert.That(transaction2.Reference, Is.EqualTo("NONREF"));
            Assert.That(transaction2.TransactionType, Is.EqualTo("N369"));
            Assert.That(transaction2.Value, Is.EqualTo("0906230624D203,N369NONREF"));
            Assert.That(transaction2.ValueDate, Is.EqualTo(new DateTime(2009, 06, 23)));

            #endregion

            var csm2 = sample.Skip(1).Take(1).First();

            #region Customer Statement Message 2
            Assert.That(csm2.Account, Is.EqualTo("500950253"));
            Assert.That(csm2.ClosingAvailableBalance, Is.Null);
            Assert.That(csm2.ClosingBalance.Balance, Is.EqualTo(new Money("16131,58", new Currency("EUR"), nlNL)));
            Assert.That(csm2.ClosingBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm2.ClosingBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm2.ClosingBalance.EntryDate, Is.EqualTo(new DateTime(2009, 06, 25)));
            Assert.That(csm2.Description, Is.Null);
            Assert.That(csm2.ForwardAvailableBalance, Is.Null);
            Assert.That(csm2.OpeningBalance.Balance, Is.EqualTo(new Money("17159,49", new Currency("EUR"), nlNL)));
            Assert.That(csm2.OpeningBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm2.OpeningBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm2.OpeningBalance.EntryDate, Is.EqualTo(new DateTime(2009, 06, 24)));
            Assert.That(csm2.RelatedMessage, Is.Null);
            Assert.That(csm2.SequenceNumber, Is.EqualTo(1));
            Assert.That(csm2.StatementNumber, Is.EqualTo(17601));
            Assert.That(csm2.TransactionReference, Is.EqualTo("ABN AMRO BANK NV"));
            Assert.That(csm2.Transactions.Count, Is.EqualTo(1));

            var csm2_transaction = csm2.Transactions.Skip(0).Take(1).First();
            Assert.That(csm2_transaction.Amount, Is.EqualTo(new Money("1027,91", new Currency("EUR"), nlNL)));
            Assert.That(csm2_transaction.DebitCredit, Is.EqualTo(DebitCredit.Debit));
            Assert.That(csm2_transaction.Description, Is.EqualTo(@"64.45.12.113 G.B. ROTTERDAM     AANSLAGBILJETNUMMER 30076145
BETALINGSKENMERK 0000 0000       3007 6145 MOZARTLN 136 WOZ2009"));
            Assert.That(csm2_transaction.EntryDate, Is.EqualTo(new DateTime(2009, 06, 25)));
            Assert.That(csm2_transaction.FundsCode, Is.EqualTo(string.Empty));
            Assert.That(csm2_transaction.Reference, Is.EqualTo("NONREF"));
            Assert.That(csm2_transaction.TransactionType, Is.EqualTo("N422"));
            Assert.That(csm2_transaction.Value, Is.EqualTo("0906240625D1027,91N422NONREF"));
            Assert.That(csm2_transaction.ValueDate, Is.EqualTo(new DateTime(2009, 06, 24)));

            #endregion

        }


        [Test, Category("FullParserTests")]
        public void AbnAmro()
        {
            var header = new Separator("ABNANL2A", "940", "ABNANL2A");
            var footer = new Separator("-");

            var format = new GenericFormat(header, footer);
            var sample = GetSample(format, "Raptorious.SharpMt940Lib.Tests.Samples.abnamro.txt");

            Assert.That(sample.Count, Is.EqualTo(4));

            
            var csm1 = sample.Skip(0).Take(1).First();
            
            #region Customer Statement Message 1
            Assert.That(csm1.Account, Is.EqualTo("500950253"));
            Assert.That(csm1.ClosingAvailableBalance, Is.Null);
            Assert.That(csm1.ClosingBalance.Balance, Is.EqualTo(new Money("17159,49", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(csm1.ClosingBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm1.ClosingBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm1.ClosingBalance.EntryDate, Is.EqualTo(new DateTime(2009, 06, 24)));
            Assert.That(csm1.Description, Is.Null);
            Assert.That(csm1.ForwardAvailableBalance, Is.Null);
            Assert.That(csm1.OpeningBalance.Balance, Is.EqualTo(new Money("17369,99", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(csm1.OpeningBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm1.OpeningBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm1.OpeningBalance.EntryDate, Is.EqualTo(new DateTime(2009, 06, 23)));
            Assert.That(csm1.RelatedMessage, Is.Null);
            Assert.That(csm1.SequenceNumber, Is.EqualTo(1));
            Assert.That(csm1.StatementNumber, Is.EqualTo(17501));
            Assert.That(csm1.TransactionReference, Is.EqualTo("ABN AMRO BANK NV"));
            Assert.That(csm1.Transactions.Count, Is.EqualTo(2));

            var transaction = csm1.Transactions.Skip(0).Take(1).First();
            Assert.That(transaction.Amount, Is.EqualTo(new Money("7,5", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(transaction.DebitCredit, Is.EqualTo(DebitCredit.Debit));
            Assert.That(transaction.Description, Is.EqualTo(@"GIRO   428428 KPN BV             BETALINGSKENM.  000000018503995
5109227317                       BETREFT FACTUUR D.D. 20-06-2009
INCL. 1,20 BTW"));
            Assert.That(transaction.EntryDate, Is.EqualTo(new DateTime(2009, 06, 24)));
            Assert.That(transaction.FundsCode, Is.EqualTo(string.Empty));
            Assert.That(transaction.Reference, Is.EqualTo("NONREF"));
            Assert.That(transaction.TransactionType, Is.EqualTo("N192"));
            Assert.That(transaction.Value, Is.EqualTo("0906230624D7,5N192NONREF"));
            Assert.That(transaction.ValueDate, Is.EqualTo(new DateTime(2009, 06, 23)));

            var transaction2 = csm1.Transactions.Skip(1).Take(1).First();
            Assert.That(transaction2.Amount, Is.EqualTo(new Money("203", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(transaction2.DebitCredit, Is.EqualTo(DebitCredit.Debit));
            Assert.That(transaction2.Description, Is.EqualTo(@"BEA               23.06.09/22.22 VILLA DORIA SCHOTEN,PAS590"));
            Assert.That(transaction2.EntryDate, Is.EqualTo(new DateTime(2009, 06, 24)));
            Assert.That(transaction2.FundsCode, Is.EqualTo(string.Empty));
            Assert.That(transaction2.Reference, Is.EqualTo("NONREF"));
            Assert.That(transaction2.TransactionType, Is.EqualTo("N369"));
            Assert.That(transaction2.Value, Is.EqualTo("0906230624D203,N369NONREF"));
            Assert.That(transaction2.ValueDate, Is.EqualTo(new DateTime(2009, 06, 23)));

#endregion

            var csm2 = sample.Skip(1).Take(1).First();

            #region Customer Statement Message 2
            Assert.That(csm2.Account, Is.EqualTo("500950253"));
            Assert.That(csm2.ClosingAvailableBalance, Is.Null);
            Assert.That(csm2.ClosingBalance.Balance, Is.EqualTo(new Money("16131,58", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(csm2.ClosingBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm2.ClosingBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm2.ClosingBalance.EntryDate, Is.EqualTo(new DateTime(2009, 06, 25)));
            Assert.That(csm2.Description, Is.Null);
            Assert.That(csm2.ForwardAvailableBalance, Is.Null);
            Assert.That(csm2.OpeningBalance.Balance, Is.EqualTo(new Money("17159,49", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(csm2.OpeningBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm2.OpeningBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm2.OpeningBalance.EntryDate, Is.EqualTo(new DateTime(2009, 06, 24)));
            Assert.That(csm2.RelatedMessage, Is.Null);
            Assert.That(csm2.SequenceNumber, Is.EqualTo(1));
            Assert.That(csm2.StatementNumber, Is.EqualTo(17601));
            Assert.That(csm2.TransactionReference, Is.EqualTo("ABN AMRO BANK NV"));
            Assert.That(csm2.Transactions.Count, Is.EqualTo(1));

            var csm2_transaction = csm2.Transactions.Skip(0).Take(1).First();
            Assert.That(csm2_transaction.Amount, Is.EqualTo(new Money("1027,91", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(csm2_transaction.DebitCredit, Is.EqualTo(DebitCredit.Debit));
            Assert.That(csm2_transaction.Description, Is.EqualTo(@"64.45.12.113 G.B. ROTTERDAM     AANSLAGBILJETNUMMER 30076145
BETALINGSKENMERK 0000 0000       3007 6145 MOZARTLN 136 WOZ2009"));
            Assert.That(csm2_transaction.EntryDate, Is.EqualTo(new DateTime(2009, 06, 25)));
            Assert.That(csm2_transaction.FundsCode, Is.EqualTo(string.Empty));
            Assert.That(csm2_transaction.Reference, Is.EqualTo("NONREF"));
            Assert.That(csm2_transaction.TransactionType, Is.EqualTo("N422"));
            Assert.That(csm2_transaction.Value, Is.EqualTo("0906240625D1027,91N422NONREF"));
            Assert.That(csm2_transaction.ValueDate, Is.EqualTo(new DateTime(2009, 06, 24)));          

            #endregion
            
        }

        [Test, Category("FullParserTests")]
        public void Ing()
        {
            var header = new Separator("0000 01INGBNL2AXXXX00001", "0000 01INGBNL2AXXXX00001", "940 00");
            var footer = new Separator("XXX");

            var format = new GenericFormat(header, footer);
            var sample = GetSample(format, "Raptorious.SharpMt940Lib.Tests.Samples.ing.txt");

            Assert.That(sample.Count, Is.EqualTo(1));


            var csm1 = sample.Skip(0).Take(1).First();

            #region Customer Statement Message 1
            Assert.That(csm1.Account, Is.EqualTo("0000000000"));
            Assert.That(csm1.ClosingAvailableBalance, Is.Null);
            Assert.That(csm1.ClosingBalance.Balance, Is.EqualTo(new Money("3,47", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(csm1.ClosingBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm1.ClosingBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm1.ClosingBalance.EntryDate, Is.EqualTo(new DateTime(2010, 07, 23)));
            Assert.That(csm1.Description, Is.EqualTo("D000004C000002D25,24C28,71"));
            Assert.That(csm1.ForwardAvailableBalance, Is.Null);
            Assert.That(csm1.OpeningBalance.Balance, Is.EqualTo(new Money("0,00", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(csm1.OpeningBalance.Currency, Is.EqualTo(new Currency("EUR")));
            Assert.That(csm1.OpeningBalance.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(csm1.OpeningBalance.EntryDate, Is.EqualTo(new DateTime(2010, 07, 22)));
            Assert.That(csm1.RelatedMessage, Is.Null);
            Assert.That(csm1.SequenceNumber, Is.EqualTo(0));
            Assert.That(csm1.StatementNumber, Is.EqualTo(100));
            Assert.That(csm1.TransactionReference, Is.EqualTo("MPBZ"));
            Assert.That(csm1.Transactions.Count, Is.EqualTo(6));

            var transaction = csm1.Transactions.Skip(0).Take(1).First();
            Assert.That(transaction.Amount, Is.EqualTo(new Money("25,03", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(transaction.DebitCredit, Is.EqualTo(DebitCredit.Credit));
            Assert.That(transaction.Description, Is.EqualTo(@"0111111111 ING Bank N.V. inzake TEST
EJ004GREENP29052010T1137"));
            Assert.That(transaction.EntryDate, Is.Null);
            Assert.That(transaction.FundsCode, Is.EqualTo(string.Empty));
            Assert.That(transaction.Reference, Is.EqualTo("NONREF"));
            Assert.That(transaction.TransactionType, Is.EqualTo("NOV "));
            Assert.That(transaction.Value, Is.EqualTo("100722C25,03NOV NONREF"));
            Assert.That(transaction.ValueDate, Is.EqualTo(new DateTime(2010, 07, 22)));

            var transaction2 = csm1.Transactions.Skip(1).Take(1).First();
            Assert.That(transaction2.Amount, Is.EqualTo(new Money("3,03", new Currency("EUR"), CultureInfo.GetCultureInfo("nl-NL"))));
            Assert.That(transaction2.DebitCredit, Is.EqualTo(DebitCredit.Debit));
            Assert.That(transaction2.Description, Is.EqualTo(@"0111111111 GPSEOUL
SPOEDBETALING MPBZS1016000047 GPSEOUL"));
            Assert.That(transaction2.EntryDate, Is.Null);
            Assert.That(transaction2.FundsCode, Is.EqualTo(string.Empty));
            Assert.That(transaction2.Reference, Is.EqualTo("NONREF"));
            Assert.That(transaction2.TransactionType, Is.EqualTo("NOV "));
            Assert.That(transaction2.Value, Is.EqualTo("100722D3,03NOV NONREF"));
            Assert.That(transaction2.ValueDate, Is.EqualTo(new DateTime(2010, 07, 22)));

            #endregion

        }

        [Test, Category("FullParserTests")]
        public void SNS()
        {
            var header = new Separator("{1:F01SNSBNL2AXXXX0000000000}{2:O94000000000000000000000000000000000000000N}{3:}{4:");
            var footer = new Separator("-}{5:}");
            var format = new GenericFormat(header, footer);

            var parsedData = GetSample(format, "Raptorious.SharpMt940Lib.Tests.Samples.sns.txt");
            var dataList = parsedData.ToList();

            Assert.That(dataList.Count, Is.EqualTo(1));
        }


        [Test, Category("FullParserTests")]
        public void SnsTransactions()
        {
            var parsedData = GetSample(SnsFormat, "Raptorious.SharpMt940Lib.Tests.Samples.sns_transactions.txt");
            var dataList = parsedData.ToList();

            Assert.That(dataList.Count, Is.EqualTo(1));
            Assert.That(dataList[0].Transactions.Count, Is.EqualTo(3));
        }

        [Test]
        public void StandardBankSa()
        {
            var expected = new ExpectedSwiftMessage
            {
                Description = null,
                OpeningBalance = 10235.43M,
                Account = "12345678",
                TransactionReference = "10150126",
                OpeningBalanceCurrencyCode = "EUR",
                OpeningBalanceDebitCredit = DebitCredit.Credit,
                OpeningBalanceDate = new DateTime(2015, 01, 23),
                StatementNumber = 620,
                SequenceNumber = 1,
                ClosingBalance = 28412.15M,
                ClosingBalanceCurrencyCode = "EUR",
                ClosingBalanceDebitCredit = DebitCredit.Credit,
                ClosingBalanceDate = new DateTime(2015, 01, 26),
                ClosingAvailableBalance = 28412.15M,
                ClosingAvailableBalanceCurrencyCode = "EUR",
                ClosingAvailableBalanceDebitCredit = DebitCredit.Credit,
                ClosingAvailableBalanceDate = new DateTime(2015, 01, 26),
                TransactionsCount = 13
            };

            var messageList = GetSample(StandardBankFormat, "Raptorious.SharpMt940Lib.Tests.Samples.StandardBank_transactions.txt").ToList();

            ExpectedSwiftMessage.AssertCustomerStatementMessage(messageList, 1, expected);
        }

        [Test]
        public void StandardBankSaTransactions()
        {
            var expectedTransactions = new List<ExpectedSwiftTransaction>();
            expectedTransactions.Add(ExpectedSwiftTransaction.CreateExpected(new DateTime(2015, 1, 26), DebitCredit.Debit,
                "R", "EUR", 52.58M, "NTRF", "ITM1234567890123", "yyyyyyyyyyyyyyyyy\r\nBasel\r\nCHF 50.00   0.950932 on 23/01/2015", "Visa"));

            expectedTransactions.Add(ExpectedSwiftTransaction.CreateExpected(new DateTime(2015, 1, 26), DebitCredit.Debit,
                "R", "EUR", 25.31M, "NTRF", "ITM1234567890123", "yyyyyyyyyyyyyyyyy\r\nAMZN.COM/BILL\r\non 23/01/2015", "Visa"));

            expectedTransactions.Add(ExpectedSwiftTransaction.CreateExpected(new DateTime(2015, 1, 26), DebitCredit.Credit,
                "R", "EUR", 18790M, "NTRF", "PON1234567890123", "yyyyyyyyyyyyyyyyy", "Inward Payment", "ABC12345678HRSXS"));

            var messageList =
                GetSample(StandardBankFormat, "Raptorious.SharpMt940Lib.Tests.Samples.StandardBank_few_transactions.txt").ToList();

            var message = messageList[0];
            ExpectedSwiftMessage.AssertCustomerStatementTransactions(message, expectedTransactions);
        }

        [Test]
        public void StandardBankGb()
        {
            var expected = new ExpectedSwiftMessage
            {
                Description = null,
                OpeningBalance = 10235.43M,
                Account = "12345678",
                TransactionReference = "10150126",
                OpeningBalanceCurrencyCode = "GBP",
                OpeningBalanceDebitCredit = DebitCredit.Credit,
                OpeningBalanceDate = new DateTime(2015, 01, 23),
                StatementNumber = 620,
                SequenceNumber = 1,
                ClosingBalance = 28412.15M,
                ClosingBalanceCurrencyCode = "GBP",
                ClosingBalanceDebitCredit = DebitCredit.Credit,
                ClosingBalanceDate = new DateTime(2015, 01, 26),
                ClosingAvailableBalance = 28412.15M,
                ClosingAvailableBalanceCurrencyCode = "GBP",
                ClosingAvailableBalanceDebitCredit = DebitCredit.Credit,
                ClosingAvailableBalanceDate = new DateTime(2015, 01, 26),
                TransactionsCount = 13
            };

            var messageList = GetSample(StandardBankFormat, "Raptorious.SharpMt940Lib.Tests.Samples.StandardBank_transactions_GB.txt" , CultureInfo.GetCultureInfo("en-GB")).ToList();

            ExpectedSwiftMessage.AssertCustomerStatementMessage(messageList, 1, expected);
        }

        [Test]
        public void StandardBankTransactionsGb()
        {
            var expectedTransactions = new List<ExpectedSwiftTransaction>();
            expectedTransactions.Add(ExpectedSwiftTransaction.CreateExpected(new DateTime(2015, 1, 26), DebitCredit.Debit,
                "R", "GBP", 52.58M, "NTRF", "ITM1234567890123", "yyyyyyyyyyyyyyyyy\r\nBasel\r\nCHF 50.00   0.950932 on 23/01/2015", "Visa"));

            expectedTransactions.Add(ExpectedSwiftTransaction.CreateExpected(new DateTime(2015, 1, 26), DebitCredit.Debit,
                "R", "GBP", 25.31M, "NTRF", "ITM1234567890123", "yyyyyyyyyyyyyyyyy\r\nAMZN.COM/BILL\r\non 23/01/2015", "Visa"));

            expectedTransactions.Add(ExpectedSwiftTransaction.CreateExpected(new DateTime(2015, 1, 26), DebitCredit.Credit,
                "R", "GBP", 18790M, "NTRF", "PON1234567890123", "yyyyyyyyyyyyyyyyy", "Inward Payment", "ABC12345678HRSXS"));

            var messageList =
                GetSample(StandardBankFormat, "Raptorious.SharpMt940Lib.Tests.Samples.StandardBank_few_transactions_GB.txt", CultureInfo.GetCultureInfo("en-GB")).ToList();

            var message = messageList[0];
            ExpectedSwiftMessage.AssertCustomerStatementTransactions(message, expectedTransactions);
        }

        private ICollection<CustomerStatementMessage> GetSample(IMt940Format format, string resourceName)
        {
            return GetSample(format, resourceName, CultureInfo.GetCultureInfo("nl-NL"));
        }

        private ICollection<CustomerStatementMessage> GetSample(IMt940Format format, string resourceName, CultureInfo cultureInfo)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                var reader = new StreamReader(stream);
                return Mt940Parser.Parse(format, reader, cultureInfo);
            }
        }
    }
}

