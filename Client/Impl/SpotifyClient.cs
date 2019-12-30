#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// SpotifyClient.cs
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
using RestSharp.Authenticators;

namespace OAuth2.Client.Impl
	{
	/// <summary>
	/// Spotify client 
	/// https://developer.spotify.com/web-api/authorization-guide/
	/// https://developer.spotify.com/web-api/endpoint-reference/
	/// </summary>
	public class SpotifyClient : OAuth2Client
		{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpotifyClient"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="configuration">The configuration.</param>
		public SpotifyClient (IRequestFactory factory, IClientConfiguration configuration)
			: base (factory, configuration)
			{
			}

		/// <summary>
		/// Spotify client name
		/// </summary>
		public override string Name
			{
			get { return "Spotify"; }
			}

		/// <summary>
		/// The access code service endpoint
		/// </summary>
		protected override Endpoint AccessCodeServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://accounts.spotify.com",
					Resource = "/authorize"
					};
				}
			}

		/// <summary>
		/// The acess token service endpoint
		/// </summary>
		protected override Endpoint AccessTokenServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://accounts.spotify.com",
					Resource = "/api/token"
					};
				}
			}

		/// <summary>
		/// Called just before issuing request to third-party service when everything is ready.
		/// Allows to add extra parameters to request or do any other needed preparations.
		/// </summary>
		protected override void BeforeGetUserInfo (BeforeAfterRequestArgs args)
			{
			args.Client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator (AccessToken, "Bearer");
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
					BaseUri = "https://api.spotify.com",
					Resource = "/v1/me"
					};
				}
			}

		/// <summary>
		/// Should return parsed <see cref="UserInfo"/> from content received from third-party service.
		/// </summary>
		/// <param name="content">The content which is received from third-party service.</param>
		protected override UserInfo ParseUserInfo (string content)
			{
			var response = JObject.Parse (content);
			var userInfo = new UserInfo ();
			var tok = response.SelectToken ("images[0].url");
			userInfo.AvatarUri.Normal =
				userInfo.AvatarUri.Large =
				userInfo.AvatarUri.Small = tok == null ? null : tok.ToString ();

			tok = response.SelectToken ("display_name");
			userInfo.FirstName = tok == null ? null : tok.ToString ();

			tok = response.SelectToken ("id");
			userInfo.Id = tok == null ? null : tok.ToString ();

			tok = response.SelectToken ("email");
			userInfo.Email = tok == null ? null : tok.ToString ();

			userInfo.ProviderName = this.Name;
			return userInfo;
			}
		}
	}