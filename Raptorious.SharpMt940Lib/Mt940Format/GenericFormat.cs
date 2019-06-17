
namespace Raptorious.SharpMt940Lib.Mt940Format
{
    /// <summary>
    /// Non-specific IMt940Format implementation.
    /// </summary>
    public class GenericFormat : IMt940Format
    {
        /// <summary>
        /// 
        /// </summary>
        public Separator Header
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Separator Trailer
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new Mt940 format based on given values.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="trailer"></param>
        public GenericFormat(Separator header, Separator trailer)
        {
            Header = header;
            Trailer = trailer;
        }
    }
}
