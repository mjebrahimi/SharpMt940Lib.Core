using System;
using NUnit.Framework;
using System.Globalization;

namespace Raptorious.SharpMt940Lib.Tests
{
	[TestFixture]
	public class MoneyTests
	{
        readonly CultureInfo _fr = CultureInfo.GetCultureInfo("fr-FR");

        [Test, Category("Money")]
        public void MoneyValue_NotEqualsOverload_OperatorTest()
        {
            var left = new Money("2,0", new Currency("EUR"), _fr);
            var right = new Money("1,0", new Currency("EUR"), _fr);

            Assert.IsTrue(left != right);
            Assert.IsTrue(right != left);
            Assert.IsTrue(left != null);
            Assert.IsTrue(null != right);
            Assert.IsFalse(null != null);
        }

        [Test, Category("Money")]
        public void MoneyCurrency_NotEqualsOverload_OperatorTest()
        {
            var left = new Money("1,0", new Currency("EUR"), _fr);
            var right = new Money("1,0", new Currency("USD"), _fr);

            Assert.IsTrue(left != right);
            Assert.IsTrue(right != left);
            Assert.IsTrue(left != null);
            Assert.IsTrue(null != right);
            Assert.IsFalse(null != null);
        }

        [Test, Category("Money")]
        public void MoneyCurrencyValue_NotEqualsOverload_OperatorTest()
        {
            var left = new Money("2,0", new Currency("EUR"), _fr);
            var right = new Money("1,0", new Currency("USD"), _fr);

            Assert.IsTrue(left != right);
            Assert.IsTrue(right != left);
            Assert.IsTrue(left != null);
            Assert.IsTrue(null != right);
            Assert.IsFalse(null != null);
        }


        [Test, Category("Money")]
        public void Money_EqualsOverload_OperatorTest()
        {
            var left = new Money("1,0", new Currency("EUR"), _fr);
            var right = new Money("1,0", new Currency("EUR"), _fr);

            Assert.IsTrue(default(Money) == default(Money));
            Assert.IsTrue(left == right);
            Assert.IsTrue(right == left);
            Assert.IsFalse(left == null);
            Assert.IsFalse(null == right);
        }

        [Test, Category("Money")]
        public void Money_LessOverload_OperatorTest()
        {
            var left = new Money("1,0", new Currency("EUR"), _fr);
            var right = new Money("2,0", new Currency("EUR"), _fr);

            Assert.IsTrue(left < right);
            Assert.IsFalse(right < left);
            Assert.IsFalse(left < null);
            Assert.IsFalse(null < right);
            Assert.IsFalse(default(Money) < default(Money));
        }

        [Test, Category("Money")]
        public void Money_GreaterOverload_OperatorTest()
        {
            var left = new Money("1,0", new Currency("EUR"), _fr);
            var right = new Money("2,0", new Currency("EUR"), _fr);

            Assert.IsTrue(right > left);
            Assert.IsFalse(left > right);            
            Assert.IsFalse(left > null);
            Assert.IsFalse(null > right);
            Assert.IsFalse(default(Money) > default(Money));
        }



		[Test]
		public void Money_ValueIsCorrect ()
		{
			Assert.AreEqual
            (
                new Money("7,5", new Currency("EUR"), _fr).Value,
				new decimal(7.5)
			);
		}

		[Test]
		public void Money_Equal_WithSameCurrency ()
		{
			Assert.AreEqual
            (
                new Money("7,5", new Currency("EUR"), _fr),
                new Money("7,5", new Currency("EUR"), _fr)
			);
		}

		[Test]
		public void Money_Equality_WithSameCurrency ()
		{
			Assert.True
            (
                new Money("7,5", new Currency("EUR"), _fr) == new Money("7,5", new Currency("EUR"), _fr)
			);
		}

		[Test]
		public void Money_Equals_WithSameCurrency ()
		{
			Assert.True
            (
                new Money("7,5", new Currency("EUR"), _fr).Equals(new Money("7,5", new Currency("EUR"), _fr))
			);
		}
	}
}
