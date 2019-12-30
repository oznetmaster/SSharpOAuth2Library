#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// AsanaClient.cs
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
using System.Linq;
using Newtonsoft.Json.Linq;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;
using RestSharp.Authenticators;

namespace OAuth2.Client.Impl
	{
	/// <summary>
	/// Asana authentication client.
	/// </summary>
	public class AsanaClient : OAuth2Client
		{
		/// <summary>
		/// Initializes a new instance of the <see cref="AsanaClient"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="configuration">The configuration.</param>
		public AsanaClient (IRequestFactory factory, IClientConfiguration configuration)
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
					BaseUri = "https://app.asana.com",
					Resource = "/-/oauth_authorize"
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
					BaseUri = "https://app.asana.com",
					Resource = "/-/oauth_token"
					};
				}
			}

		/// <summary>
		/// Defines URI of service which allows to obtain information about user which is currently logged in.
		/// </summary>
		protected override Endpoint UserInfoServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://app.asana.com",
					Resource = "/api/1.0/users/me"
					};
				}
			}

		/// <summary>
		/// Called just before issuing request to third-party service when everything is ready.
		/// Allows to add extra parameters to request or do any other needed preparations.
		/// </summary>
		protected override void BeforeGetUserInfo (BeforeAfterRequestArgs args)
			{
			args.Request.AddParameter ("opt_fields", "id,name,photo.image_128x128,photo.image_60x60,photo.image_36x36,email");
			args.Client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator (AccessToken, "Bearer");
			}

		/// <summary>
		/// Should return parsed <see cref="UserInfo"/> from content received from third-party service.
		/// </summary>
		/// <param name="content">The content which is received from third-party service.</param>
		protected override UserInfo ParseUserInfo (string content)
			{
			JToken dataExists;
			var response = JObject.Parse (content);
			if (!response.TryGetValue ("data", out dataExists))
			return new UserInfo ();

			//const string avatarUriTemplate = "{0}?type={1}";
			var avatarSmallUri = response["data"]["photo"]["image_36x36"].Value<string> ();
			var avatarNormalUri = response["data"]["photo"]["image_60x60"].Value<string> ();
			var avatarLargeUri = response["data"]["photo"]["image_128x128"].Value<string> ();
			var splitName = new List<string> (response["data"]["name"].Value<string> ().Split (' '));
			var firstName = splitName.FirstOrDefault ();
			splitName.RemoveAt (0);
			var lastName = splitName.Join (" ");

			return new UserInfo
				{
				Id = response["data"]["id"].Value<string> (),
				FirstName = firstName,
				LastName = lastName,
				Email = response["data"]["email"].SafeGet (x => x.Value<string> ()),
				AvatarUri =
					{
					Small = !StringEx.IsNullOrWhiteSpace (avatarSmallUri) ? avatarSmallUri : string.Empty,
					Normal = !StringEx.IsNullOrWhiteSpace (avatarNormalUri) ? avatarNormalUri : string.Empty,
					Large = !StringEx.IsNullOrWhiteSpace (avatarLargeUri) ? avatarLargeUri : string.Empty
					}
				};
			}

		/// <summary>
		/// Friendly name of provider (OAuth2 service).
		/// </summary>
		public override string Name
			{
			get { return "Asana"; }
			}
		}
	}