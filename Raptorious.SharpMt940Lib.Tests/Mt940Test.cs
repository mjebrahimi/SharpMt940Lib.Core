/* 
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/

using System;
using System.Linq;
using NUnit.Framework;
using System.Globalization;

namespace Raptorious.SharpMt940Lib.Mt940Format
{
	[TestFixture]
	public class Mt940Test
	{
        const string x = @":20:STARTUMSE
:25:76351040/0008179863
:28C:00000/001
:60F:C140128EUR895,68
:61:1401300130CR2368,22NTRFNONREF
:86:153?00LOHN  GEHALT?109201?20EREF+14011800375309300002G7?21411
01?22SVWZ+Lohn/Gehalt 00375309/2?2301401/GSS 2368.22, 741101 1?24
4011800375309300002G741101?25ABWA+Siemens AGGSS HRS DE P?26S-HRS4
You?30BYLADEMMXXX?31DE90700500000002055382?32Siemens AG CF TRE
:61:1401300130DR58,52N037NONREF//100000250001
:86:006?00SONSTIGER EINZUG?109272?20EC 85547192 290114172006OC2?3
074320073?315403626?32OMV EFFENTRICH 7409, EFFELT?34011
:62F:C140130EUR3205,38
-
:20:STARTUMSE
:25:76351040/0008179863
:28C:00000/001
:60F:C140130EUR3205,38
:61:1401310131DR20,00N041NONREF//100000080968
:86:835?00LADEVORGANG PREPAID - KARTE?109218?20VOM 29.01.2014 18.
06 UHR?21MOBILFUNKNR015783680861?22E-PLUS GRUPPE?3025050000?31901
0470514?32HB-HANDY-LADEN, TAN 661088?34072
:62F:C140131EUR3185,38
-";

        public static readonly CultureInfo GbCultureInfo = CultureInfo.GetCultureInfo("en-GB");
        public static readonly CultureInfo NlCultureInfo = CultureInfo.GetCultureInfo("nl-NL");

        [Test]
        public void M()
        {
            var data = x;
            var result = Mt940Parser.ParseData(new AbnAmro(), data, NlCultureInfo);
        }

		[Test]
		public void FullParsersTest ()
		{
            var fr = CultureInfo.GetCultureInfo("fr-FR");

			var data = Data;
            var result = Mt940Parser.ParseData(new AbnAmro(), data, NlCultureInfo);

			Assert.AreEqual (4, result.Count);

			var message = result.First ();
			Assert.AreEqual ("500950253", message.Account);
			Assert.AreEqual (2, message.Transactions.Count);

			var messageFirst = message.Transactions.First ();

			Assert.AreEqual (DebitCredit.Debit, messageFirst.DebitCredit);

			Assert.AreEqual (@"GIRO   428428 KPN BV             BETALINGSKENM.  000000018503995
5109227317                       BETREFT TestUUR D.D. 20-06-2009
INCL. 1,20 BTW", messageFirst.Description);

			Assert.AreEqual (new DateTime (2009, 06, 23), messageFirst.ValueDate);
			Assert.AreEqual (new DateTime (2009, 06, 24), messageFirst.EntryDate);
			Assert.AreEqual (DebitCredit.Debit, messageFirst.DebitCredit);
			Assert.AreEqual (new Money ("7,5", new Currency ("EUR"), fr), messageFirst.Amount);
			Assert.AreEqual ("NONREF", messageFirst.Reference);
			Assert.AreEqual ("N192", messageFirst.TransactionType);

		}
		// data from: http://wiki.yuki.nl/Default.aspx?Page=ABNAMRO%20MT940%20voorbeeld&NS=&AspxAutoDetectCookieSupport=1
		private const string Data = @"ABNANL2A
940
ABNANL2A
:20:ABN AMRO BANK NV
:25:500950253
:28:17501/1
:60F:C090623EUR17369,99
:61:0906230624D7,5N192NONREF
:86:GIRO   428428 KPN BV             BETALINGSKENM.  000000018503995
5109227317                       BETREFT TestUUR D.D. 20-06-2009
INCL. 1,20 BTW
:61:0906230624D203,N369NONREF
:86:BEA               23.06.09/22.22 VILLA DORIA SCHOTEN,PAS590
:62F:C090624EUR17159,49
-
ABNANL2A
940
ABNANL2A
:20:ABN AMRO BANK NV
:25:500950253
:28:17601/1
:60F:C090624EUR17159,49
:61:0906240625D1027,91N422NONREF
:86:64.45.12.113 G.B. ROTTERDAM     AANSLAGBILJETNUMMER 30076145
BETALINGSKENMERK 0000 0000       3007 6145 MOZARTLN 136 WOZ2009
:62F:C090625EUR16131,58
-
ABNANL2A
940
ABNANL2A
:20:ABN AMRO BANK NV
:25:500950253
:28:17701/1
:60F:C090625EUR16131,58
:61:0906250626D19,45N192NONREF
:86:34.01.56.740                    CANAL DIGITAAL B.V.
BETALINGSKENM.  0000000120357271 TestURATIE MAAND JULI 2009
77064387 ENTERTAINMENT-PAKKET    ZIE WWW.CANALDIGITAAL.NL/TestUUR
:62F:C090626EUR16112,13
-
ABNANL2A
940
ABNANL2A
:20:ABN AMRO BANK NV
:25:485082713
:28:17501/1
:60F:C090623EUR295922,68
:61:0906260624C25111,08N838NONREF
:86:VERKOOP BECAM HOLDING PER        23/06 ST 1.439 @ 17.52246
:62F:C090624EUR321033,76
-";
	}
}

