#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// VkClient.cs
// 
// Copyright (c) 2012-2013 Constantin Titarenko, Andrew Semack and others
// 
// Copyright � 2019 Nivloc Enterprises Ltd.  All rights reserved.
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
	/// VK (Vkontakte) authentication client.
	/// </summary>
	public class VkClient : OAuth2Client
		{
		private string _userId;
		private string _email;

		/// <summary>
		/// Initializes a new instance of the <see cref="VkClient"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="configuration">The configuration.</param>
		public VkClient (IRequestFactory factory, IClientConfiguration configuration)
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
					BaseUri = "http://oauth.vk.com",
					Resource = "/authorize"
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
					BaseUri = "https://oauth.vk.com",
					Resource = "/access_token"
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
					BaseUri = "https://api.vk.com",
					Resource = "/method/users.get"
					};
				}
			}

		public override string Name
			{
			get { return "Vkontakte"; }
			}

		/// <summary>
		/// Called just after obtaining response with access token from third-party service.
		/// Allows to read extra data returned along with access token.
		/// </summary>
		protected override void AfterGetAccessToken (BeforeAfterRequestArgs args)
			{
			var instance = JObject.Parse (args.Response.Content);
			_userId = instance["user_id"].Value<string> ();
			var email = instance["email"];
			if (email != null)
				_email = email.Value<string> ();
			}

		/// <summary>
		/// Called just before issuing request to third-party service when everything is ready.
		/// Allows to add extra parameters to request or do any other needed preparations.
		/// </summary>
		protected override void BeforeGetUserInfo (BeforeAfterRequestArgs args)
			{
			args.Request.AddParameter ("v", "5.74");
			args.Request.AddParameter ("user_ids", _userId);
			args.Request.AddParameter ("fields", "first_name,last_name,has_photo,photo_max_orig");
			}

		/// <summary>
		/// Should return parsed <see cref="UserInfo"/> from content received from third-party service.
		/// </summary>
		/// <param name="content">The content which is received from third-party service.</param>
		protected override UserInfo ParseUserInfo (string content)
			{
			var response = JObject.Parse (content)["response"][0];
			var hasPhoto = response["has_photo"].Value<bool> ();
			var avatarUri = hasPhoto ? response["photo_max_orig"].Value<string> () : null;
			return new UserInfo
				{
				Email = _email,
				FirstName = response["first_name"].Value<string> (),
				LastName = response["last_name"].Value<string> (),
				Id = response["id"].Value<string> (),
				AvatarUri =
					{
					Small = null,
					Normal = avatarUri,
					Large = null
					}
				};
			}
		}
	}