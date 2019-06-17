using System;
using System.Globalization;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// Represents a monetary value. (http://martinfowler.com/eaaCatalog/money.html)
    /// </summary>
    public class Money : IEquatable<Money>, IComparable, IComparable<Money>
    {
        /// <summary>
        /// Gets the currency of the money
        /// </summary>
        public Currency Currency
        {
            get
            {
                return _currency;
            }
        }
        private readonly Currency _currency;


        /// <summary>
        /// The decimal value of the money.
        /// </summary>
        public decimal Value { get; private set; }

        /// <summary>
        /// Initializes the money object with the given string in decimal format for the given currency.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currency"></param>
        [Obsolete("Please use the constructor with CultureInfo")]
        public Money(string value, Currency currency) : 
            this(value, currency, CultureInfo.CurrentCulture)
        {
            if(currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value can not be empty", value);
            }
        }

        /// <summary>
        /// Initializes the money object with the given string in decimal format for the given currency.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currency"></param>
        /// <param name="cultureInfo">The culture to use</param>
        public Money(string value, Currency currency, IFormatProvider cultureInfo)
        {
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value can not be empty", value);
            }

            var formattedValue = string.Format(cultureInfo, "{0:C}", value);
            Value = ValueConverter.ParseDecimal(formattedValue, cultureInfo);

            _currency = currency;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Returns true if equal</returns>
        public bool Equals(Money other)
        {
            if (other == null)
            {
                return false;
            }

            if (!(other.Currency.Equals(Currency)))
            {
                return false;
            }

            return other.Value == Value;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as Currency);
        }

        /// <summary>
        /// Returns the hash code of the object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 5 ^ Value.GetHashCode();
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as Money);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Money other)
        {
            if (other == null)
            {
                return 1;
            }

            return Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Money left, Money right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }


        /// <summary>
        /// Indicates whether the current object is not equal to another object of the same type.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Money left, Money right)
        {
            if (left == null && right == null)
            {
                return false;
            }
            
            if (left == null || right == null)
            {
                return true;
            }
            
            return !left.Equals(right);
        }

        /// <summary>
        /// Check if the left object is greater than the right object
        /// </summary>
        public static bool operator >(Money left, Money right)
        {
            if(
                left == null || 
                right == null ||
                (left.Currency != right.Currency)
            )
            {
                return false;
            }
            
            return left.Value > right.Value;
        }

        /// <summary>
        /// Check if the left object is less than the right object
        /// </summary>
        public static bool operator <(Money left, Money right)
        {
            if (
                left == null ||
                right == null ||
                (left.Currency != right.Currency)
            )
            {
                return false;
            }

            
            return left.Value < right.Value;            
        }

        /// <summary>
        /// Returns the string representation of the money object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", Currency.Code, Value);
        }
    }
}