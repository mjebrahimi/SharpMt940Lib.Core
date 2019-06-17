using NUnit.Framework;

namespace Raptorious.SharpMt940Lib.Tests
{
	[TestFixture]
	public class CurrencyTests
	{
        [Test, Category("Currency")]
        public void CurrencyNotEqualOperatorTest()
        {
            var left = new Currency("EUR");
            var right = new Currency("USD");

            Assert.IsTrue(left != right);
            Assert.IsTrue(right != left);
            Assert.IsTrue(left != null);
            Assert.IsTrue(null != right);
            Assert.IsFalse(null != null);
        }


        [Test, Category("Currency")]
		public void CurrencyEqualTest ()
		{
			var left = new Currency ("EUR");
			var right = new Currency ("EUR");

			Assert.AreEqual (left, right);
			Assert.AreEqual (right, left);
		}

        [Test, Category("Currency")]
		public void CurrencyNotEqualTest ()
		{
			var left = new Currency ("EUR");
			var right = new Currency ("USD");

			Assert.AreNotEqual (left, right);
			Assert.AreNotEqual (right, left);
		}

        [Test, Category("Currency")]
		public void CurrencyEqualsTest ()
		{
			var left = new Currency ("EUR");
			var right = new Currency ("EUR");

			Assert.True (left.Equals (right));
			Assert.True (right.Equals (left));
		}

        [Test, Category("Currency")]
		public void CurrencyNotEqualsTest ()
		{
			var left = new Currency ("EUR");
			var right = new Currency ("USD");

			Assert.False (left.Equals (right));
			Assert.False (right.Equals (left));
		}

        [Test, Category("Currency")]
		public void CurrencyEqualityTest ()
		{
			var left = new Currency ("EUR");
			var right = new Currency ("EUR");

			Assert.True (left == right);
			Assert.True (right == left);
		}
		
        [Test, Category("Currency")]
		public void CurrencyEqualsAsObjectTest ()
		{
			var left = (object)new Currency ("EUR");
			var right = (object)new Currency ("EUR");

			Assert.False (left == right);
			Assert.False (right == left);
		}

        [Test, Category("Currency")]
		public void CurrencyNotEqualityTest ()
		{
			var left = new Currency ("EUR");
			var right = new Currency ("USD");

			Assert.True (left != right);
			Assert.True (right != left);
		}
	}
}