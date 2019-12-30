#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// XingClient.cs
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
using System.Collections.Generic;
using Newtonsoft.Json;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace OAuth2.Client.Impl
	{
	/// <summary>
	/// Xing authentication client.
	/// </summary>
	public class XingClient : OAuthClient
		{
		private const string BaseApiUrl = "https://api.xing.com";

		/// <summary>
		/// Initializes a new instance of the <see cref="GoogleClient"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="configuration">The configuration.</param>
		public XingClient (IRequestFactory factory, IClientConfiguration configuration)
			: base (factory, configuration)
			{
			}

		/// <inheritdoc cref="OAuthClient.Name" />
		public override string Name
			{
			get { return "Xing"; }
			}

		/// <inheritdoc cref="OAuthClient.AccessTokenServiceEndpoint" />
		protected override Endpoint RequestTokenServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = BaseApiUrl,
					Resource = "/v1/request_token"
					};
				}
			}

		/// <inheritdoc cref="OAuthClient.AccessTokenServiceEndpoint" />
		protected override Endpoint LoginServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = BaseApiUrl,
					Resource = "/v1/authorize"
					};
				}
			}

		/// <inheritdoc cref="OAuthClient.AccessTokenServiceEndpoint" />
		protected override Endpoint AccessTokenServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = BaseApiUrl,
					Resource = "/v1/access_token"
					};
				}
			}

		/// <inheritdoc cref="OAuthClient.UserInfoServiceEndpoint" />
		protected override Endpoint UserInfoServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = BaseApiUrl,
					Resource = "/v1/users/me"
					};
				}
			}

		/// <inheritdoc cref="OAuthClient.ParseUserInfo" />
		protected override UserInfo ParseUserInfo (string content)
			{
			var users = JsonConvert.DeserializeObject<UserContainer> (content);
			var userInfo = new UserInfo ();

			if (users != null && users.Users != null && users.Users.Count > 0)
				{
				userInfo.Id = users.Users[0].Id;
				userInfo.FirstName = users.Users[0].FirstName;
				userInfo.LastName = users.Users[0].LastName;
				userInfo.Email = users.Users[0].Email;
				if (users.Users[0].PhotoUrls != null)
					{
					userInfo.AvatarUri.Small = users.Users[0].PhotoUrls.Small;
					userInfo.AvatarUri.Normal = users.Users[0].PhotoUrls.Normal;
					userInfo.AvatarUri.Large = users.Users[0].PhotoUrls.Large;
					}
				}

			return userInfo;
			}

		private class UserContainer
			{
			[JsonProperty (PropertyName = "users")]
			public List<User> Users { get; set; }
			}

		private class User
			{
			[JsonProperty (PropertyName = "id")]
			public string Id { get; set; }

			[JsonProperty (PropertyName = "first_name")]
			public string FirstName { get; set; }

			[JsonProperty (PropertyName = "last_name")]
			public string LastName { get; set; }

			[JsonProperty (PropertyName = "active_email")]
			public string Email { get; set; }

			[JsonProperty (PropertyName = "photo_urls")]
			public PhotoUrls PhotoUrls { get; set; }
			}

		private class PhotoUrls
			{
			[JsonProperty (PropertyName = "size_48x48")]
			public string Small { get; set; }

			[JsonProperty (PropertyName = "size_128x128")]
			public string Normal { get; set; }

			[JsonProperty (PropertyName = "size_256x256")]
			public string Large { get; set; }
			}
		}
	}