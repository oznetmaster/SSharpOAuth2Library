#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// ObjectExtensions.cs
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
using System.Linq;
using Crestron.SimplSharp.Reflection;

namespace OAuth2.Infrastructure
	{
	/// <summary>
	/// Common extensions.
	/// </summary>
	public static class ObjectExtensions
		{
		/// <summary>
		/// Returns true if all equally named and typed properties have 
		/// same values on two different objects (types of objects can be different).
		/// </summary>
		public static bool AllPropertiesAreEqualTo (this object @this, object other)
			{
			var thisProperties = @this.GetCType ().GetProperties ().Where (x => x.CanRead).ToList ();
			var otherProperties = other.GetCType ().GetProperties ().Where (x => x.CanRead).ToList ();

			return (from thisProperty in thisProperties
				let otherProperty = otherProperties.FirstOrDefault (
					x => x.Name == thisProperty.Name &&
					     x.PropertyType == thisProperty.PropertyType)
				let value = thisProperty.GetValue (@this, null)
				let otherValue = otherProperty == null ? null : otherProperty.GetValue (other, null)
				where value == null && otherValue == null || value != null && value.Equals (otherValue)
				select 1).Sum () == thisProperties.Count;
			}
		}
	}