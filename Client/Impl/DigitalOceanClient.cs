#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// DigitalOceanClient.cs
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
using System.Threading;
#if ASYNC
using System.Threading.Tasks;
#endif
using Newtonsoft.Json.Linq;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace OAuth2.Client.Impl
	{
	public class DigitalOceanClient : OAuth2Client
		{
		private string _accessToken;

		public DigitalOceanClient (IRequestFactory factory, IClientConfiguration configuration)
			: base (factory, configuration)
			{
			}

		public override string Name
			{
			get { return "DigitalOcean"; }
			}

		protected override Endpoint AccessCodeServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://cloud.digitalocean.com",
					Resource = "/v1/oauth/authorize"
					};
				}
			}

		protected override void AfterGetAccessToken (BeforeAfterRequestArgs args)
			{
			_accessToken = args.Response.Content;
			}

		protected override Endpoint AccessTokenServiceEndpoint
			{
			get
				{
				return new Endpoint
					{
					BaseUri = "https://cloud.digitalocean.com",
					Resource = "/v1/oauth/token"
					};
				}
			}

		protected override Endpoint UserInfoServiceEndpoint
			{
			get { throw new NotImplementedException (); }
			}

		protected override UserInfo GetUserInfo ()
			{
			return ParseUserInfo (_accessToken);
			}

#if ASYNC
		/// <inheritdoc />
		protected override Task<UserInfo> GetUserInfoAsync (CancellationToken cancellationToken = default)
			{
			return Task.FromResult (ParseUserInfo (_accessToken));
			}
#endif

		protected override UserInfo ParseUserInfo (string content)
			{
			var response = JObject.Parse (content);
			return new UserInfo
				{
				Id = response["uid"].Value<string> (),
				FirstName = response["info"]["name"].Value<string> (),
				LastName = "",
				Email = response["info"]["email"].SafeGet (x => x.Value<string> ())
				};
			}
		}
	}