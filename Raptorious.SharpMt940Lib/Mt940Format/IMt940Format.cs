/* 
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/


namespace Raptorious.SharpMt940Lib.Mt940Format
{
	/// <summary>
	/// Every bank defines it's own header and trailer in it's MT940 format.
	/// Implement this interface per specific bank.
	/// </summary>
	public interface IMt940Format
	{
		/// <summary>
		/// 
		/// </summary>
		Separator Header { get; }
		
		/// <summary>
		/// 
		/// </summary>
		Separator Trailer { get; }
	}
}
