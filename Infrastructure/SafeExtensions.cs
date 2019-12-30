#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// SafeExtensions.cs
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

namespace OAuth2.Infrastructure
	{
	/// <summary>
	/// Set of extension methods for safe operation on nullable types.
	/// </summary>
	public static class SafeExtensions
		{
		/// <summary>
		/// Executes selector on instance and returns result or returns default value of target type if given instance is null.
		/// </summary>
		public static T SafeGet<TModel, T> (this TModel instance, Func<TModel, T> selector) where TModel : class
			{
			return instance == null ? default(T) : selector (instance);
			}

#if ASYNC
	/// <summary>
	/// Executes selector on instance and returns result or returns default value of target type if given instance is null.
	/// </summary>
     public static async Task<T> SafeGetAsync<TModel, T>(this TModel instance, Func<TModel, Task<T>> selector) where TModel : class
        {
            return instance == null ? default : await selector(instance).ConfigureAwait(false);
        }
#endif
		}
	}