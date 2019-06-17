/* 
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/


namespace Raptorious.SharpMt940Lib.Mt940Format
{
    /// <summary>
    /// IMt940Format implementation for Abn Amro.
    /// </summary>
    /// <see cref="IMt940Format"></see>
    public class AbnAmro : IMt940Format
    {
        /// <summary>
        /// Header property
        /// </summary>
        /// <value>ABN Amro defines it's header as such:
        /// ABNANL2A[newline]
        /// 940[newline]
        /// ABNANL2A
        /// </value>
        public Separator Header
        {
            get;
            private set;
        }

        /// <summary>
        /// Trailer
        /// </summary>
        /// <value>
        /// ABN Amro defines it's trailer as -
        /// </value>
        /// 
        public Separator Trailer
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AbnAmro()
        {
            Header = new Separator("ABNANL2A", "940", "ABNANL2A");
            Trailer = new Separator("-");
        }
    }




}
