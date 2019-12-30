#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// StringExtensions.cs
// 
// Copyright (c) 2012-2013 Constantin Titarenko, Andrew Semack and others
// 
// Copyright © 2019 Nivloc Enterprises Ltd.  All rights reserved.
// 
// -----------------------------------------------------------------------------------------------------------------
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
// 
// 
// 
#endregion
using System;
using System.Collections.Generic;
using SSMono.Security.Cryptography;
using System.Text;

namespace OAuth2.Infrastructure
	{
	/// <summary>
	/// Set of extension methods for <see cref="string"/>.
	/// </summary>
	public static class StringExtensions
		{
		/// <summary>
		/// Alias for <code>string.Format</code>.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public static string Fill (this string format, params object[] args)
			{
			return string.Format (format, args);
			}

		/// <summary>
		/// Alias for <code>string.Join</code>.
		/// </summary>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="separator">The separator.</param>
		public static string Join<T> (this IEnumerable<T> enumerable, string separator)
			{
			return StringEx.Join (separator, enumerable);
			}

		/// <summary>
		/// Returns true if given line is null, empty or contains only whitespaces.
		/// </summary>
		/// <param name="line">The line.</param>
		public static bool IsEmpty (this string line)
			{
			return StringEx.IsNullOrWhiteSpace (line);
			}


		/// <summary>
		/// Returns MD5 Hash of input.
		/// </summary>
		/// <param name="input">The line.</param>
		public static string GetMd5Hash (this string input)
			{
			var provider = new MD5CryptoServiceProvider ();
			var bytes = Encoding.UTF8.GetBytes (input);
			bytes = provider.ComputeHash (bytes);
			return BitConverter.ToString (bytes).Replace ("-", "").ToLowerInvariant ();
			}
		}
	}