#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// UserInfo.cs
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
namespace OAuth2.Models
	{
	public class AvatarInfo
		{
		/// <summary>
		/// Image size constants.
		/// </summary>
		internal const int SmallSize = 36;

		internal const int LargeSize = 300;

		/// <summary>
		/// Uri of small photo.
		/// </summary>
		public string Small { get; set; }

		/// <summary>
		/// Uri of normal photo.
		/// </summary>
		public string Normal { get; set; }

		/// <summary>
		/// Uri of large photo.
		/// </summary>
		public string Large { get; set; }
		}

	/// <summary>
	/// Contains information about user who is being authenticated.
	/// </summary>
	public class UserInfo
		{
		/// <summary>
		/// Constructor.
		/// </summary>
		public UserInfo ()
			{
			AvatarUri = new AvatarInfo ();
			}

		/// <summary>
		/// Unique identifier.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Friendly name of <see cref="UserInfo"/> provider (which is, in its turn, the client of OAuth/OAuth2 provider).
		/// </summary>
		/// <remarks>
		/// Supposed to be unique per OAuth/OAuth2 client.
		/// </remarks>
		public string ProviderName { get; set; }

		/// <summary>
		/// Email address.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// First name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Photo URI.
		/// </summary>
		public string PhotoUri
			{
			get { return AvatarUri.Normal; }
			}

		/// <summary>
		/// Contains URIs of different sizes of avatar.
		/// </summary>
		public AvatarInfo AvatarUri { get; private set; }
		}
	}