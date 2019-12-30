#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// SalesforceClient.cs
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
using Newtonsoft.Json.Linq;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace OAuth2.Client.Impl
	{
	/// <summary>
	/// Salesforce authentication client.
	/// </summary>
	public class SalesforceClient : OAuth2Client
		{
		/// <summary>
		/// Initializes a new instance of the <see cref="SalesforceClient"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="configuration">The configuration.</param>
		public SalesforceClient (IRequestFactory factory, IClientConfiguration configuration)
			: base (factory, configuration)
			{
			}

		/// <summary>
		/// Defines URI of service which issues access code.
		/// </summary>
		protected override Endpoint AccessCodeServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://login.salesforce.com",
					Resource = "/services/oauth2/authorize"
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
					BaseUri = "https://login.salesforce.com",
					Resource = "/services/oauth2/token"
					};
				}
			}

		public string SalesforceProfileUrl { get; set; }

		/// <summary>
		/// Defines URI of service which allows to obtain information about user which is currently logged in.
		/// </summary>
		protected override Endpoint UserInfoServiceEndpoint
			{
			get
				{
				Uri uri = new Uri (SalesforceProfileUrl);
				return new Endpoint
					{
					BaseUri = uri.GetLeftPart (UriPartial.Authority),
					Resource = uri.PathAndQuery
					};
				}
			}

		/// <summary>
		/// Friendly name of provider (OAuth2 service).
		/// </summary>
		public override string Name
			{
			get { return "Salesforce"; }
			}

		protected override string ParseTokenResponse (string content, string key)
			{
			// save the user's identity service url which is included in the response
			SalesforceProfileUrl = base.ParseTokenResponse (content, "id");

			return base.ParseTokenResponse (content, key);
			}

		/// <summary>
		/// Should return parsed <see cref="UserInfo"/> from content received from third-party service.
		/// </summary>
		/// <param name="content">The content which is received from third-party service.</param>
		protected override UserInfo ParseUserInfo (string content)
			{
			var response = JObject.Parse (content);

			return new UserInfo
				{
				Id = response["id"].Value<string> (),
				Email = response["email"].SafeGet (x => x.Value<string> ()),
				FirstName = response["first_name"].Value<string> (),
				LastName = response["last_name"].Value<string> (),
				AvatarUri =
					{
					Small = response["photos"]["thumbnail"].Value<string> (),
					Normal = response["photos"]["picture"].Value<string> (),
					Large = null
					}
				};
			}
		}
	}