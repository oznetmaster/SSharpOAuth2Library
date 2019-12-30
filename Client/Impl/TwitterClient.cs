#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// TwitterClient.cs
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
using Newtonsoft.Json.Linq;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace OAuth2.Client.Impl
	{
	/// <summary>
	/// Twitter authentication client.
	/// </summary>
	public class TwitterClient : OAuthClient
		{
		public TwitterClient (IRequestFactory factory, IClientConfiguration configuration)
			: base (factory, configuration)
			{
			}

		/// <summary>
		/// Defines URI of service which is called for obtaining request token.
		/// </summary>
		protected override Endpoint RequestTokenServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://api.twitter.com",
					Resource = "/oauth/request_token"
					};
				}
			}

		/// <summary>
		/// Defines URI of service which should be called to initiate authentication process.
		/// </summary>
		protected override Endpoint LoginServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://api.twitter.com",
					Resource = "/oauth/authenticate"
					};
				}
			}

		/// <summary>
		/// Defines URI of service which issues access token.
		/// </summary>
		protected override Endpoint AccessTokenServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://api.twitter.com",
					Resource = "/oauth/access_token"
					};
				}
			}

		/// <summary>
		/// Defines URI of service which is called to obtain user information.
		/// </summary>
		protected override Endpoint UserInfoServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://api.twitter.com",
					Resource = "/1.1/account/verify_credentials.json"
					};
				}
			}

		/// <summary>
		/// Friendly name of provider (OAuth service).
		/// </summary>
		public override string Name
			{
			get { return "Twitter"; }
			}

		/// <summary>
		/// Should return parsed <see cref="UserInfo" /> using content of callback issued by service.
		/// </summary>
		protected override UserInfo ParseUserInfo (string content)
			{
			var response = JObject.Parse (content);

			var name = response["name"].Value<string> ();
			var index = name.IndexOf (' ');

			string firstName;
			string lastName;
			if (index == -1)
				{
				firstName = name;
				lastName = null;
				}
			else
				{
				firstName = name.Substring (0, index);
				lastName = name.Substring (index + 1);
				}
			var avatarUri = response["profile_image_url"].Value<string> ();
			return new UserInfo
				{
				Id = response["id"].Value<string> (),
				Email = null,
				FirstName = firstName,
				LastName = lastName,
				AvatarUri =
					{
					Small = avatarUri.Replace ("normal", "mini"),
					Normal = avatarUri,
					Large = avatarUri.Replace ("normal", "bigger")
					}
				};
			}
		}
	}