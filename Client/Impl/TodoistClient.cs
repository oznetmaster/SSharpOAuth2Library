#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// TodoistClient.cs
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
using RestSharp;

namespace OAuth2.Client.Impl
	{
	/// <summary>
	/// Todoist authentication client.
	/// </summary>
	public class TodoistClient : OAuth2Client
		{
		/// <summary>
		/// Initializes a new instance of the <see cref="TodoistClient"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="configuration">The configuration.</param>
		public TodoistClient (IRequestFactory factory, IClientConfiguration configuration)
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
					BaseUri = "https://todoist.com/",
					Resource = "oauth/authorize"
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
					BaseUri = "https://todoist.com/",
					Resource = "oauth/access_token"
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
					BaseUri = "https://todoist.com/",
					Resource = "API/v6/sync"
					};
				}
			}

		/// <summary>
		/// Friendly name of provider (OAuth2 service).
		/// </summary>
		public override string Name
			{
			get { return "Todoist"; }
			}


		protected override void BeforeGetUserInfo (BeforeAfterRequestArgs args)
			{
			args.Client.Authenticator = null;
			args.Request.Parameters.Add (new Parameter ("token", AccessToken, ParameterType.GetOrPost));
			args.Request.AddParameter ("resource_types", "[\"all\"]");
			base.BeforeGetUserInfo (args);
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
				Id = response["User"]["id"].Value<string> (),
				Email = response["User"]["email"].SafeGet (x => x.Value<string> ()),
				LastName = response["User"]["full_name"].Value<string> (),
				AvatarUri =
					{
					Small = response["User"]["avatar_small"].Value<string> (),
					Normal = response["User"]["avatar_medium"].Value<string> (),
					Large = response["User"]["avatar_big"].Value<string> (),
					}
				};
			}
		}
	}