#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// LinkedinClient.cs
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
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;
using RestSharp;
using SSCore.Xml.Linq;
using SSCore.Xml.XPath;

namespace OAuth2.Client.Impl
	{
	/// <summary>
	/// LinkedIn authentication client.
	/// </summary>
	public class LinkedInClient : OAuth2Client
		{
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedInClient"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="configuration">The configuration.</param>
		public LinkedInClient (IRequestFactory factory, IClientConfiguration configuration)
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
					BaseUri = "https://www.linkedin.com",
					Resource = "/uas/oauth2/authorization"
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
					BaseUri = "https://www.linkedin.com",
					Resource = "/uas/oauth2/accessToken"
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
					BaseUri = "https://api.linkedin.com",
					Resource = "/v1/people/~:(id,email-address,first-name,last-name,picture-url)"
					};
				}
			}

#if NETCF
		public override string GetLoginLinkUri ()
			{
			return GetLoginLinkUri (null);
			}
#endif

		public override string GetLoginLinkUri (string state)
			{
			return base.GetLoginLinkUri (state ?? Guid.NewGuid ().ToString ("N"));
			}

		protected override void BeforeGetUserInfo (BeforeAfterRequestArgs args)
			{
			args.Client.Authenticator = null;
			args.Request.Parameters.Add (new Parameter ("oauth2_access_token", AccessToken, ParameterType.GetOrPost));
			}

		/// <summary>
		/// Should return parsed <see cref="UserInfo"/> from content received from third-party service.
		/// </summary>
		/// <param name="content">The content which is received from third-party service.</param>
		protected override UserInfo ParseUserInfo (string content)
			{
			var document = XDocument.Parse (content);
			var avatarUri = SafeGet (document, "/person/picture-url");
			var avatarSizeTemplate = "{0}_{0}";
			if (string.IsNullOrEmpty (avatarUri))
				{
				avatarUri = "https://www.linkedin.com/scds/common/u/images/themes/katy/ghosts/person/ghost_person_80x80_v1.png";
				avatarSizeTemplate = "{0}x{0}";
				}
			var avatarDefaultSize = string.Format (avatarSizeTemplate, 80);

			return new UserInfo
				{
				Id = document.XPathSelectElement ("/person/id").Value,
				Email = SafeGet (document, "/person/email-address"),
				FirstName = document.XPathSelectElement ("/person/first-name").Value,
				LastName = document.XPathSelectElement ("/person/last-name").Value,
				AvatarUri =
					{
					Small = avatarUri.Replace (avatarDefaultSize, string.Format (avatarSizeTemplate, AvatarInfo.SmallSize)),
					Normal = avatarUri,
					Large = avatarUri.Replace (avatarDefaultSize, string.Format (avatarSizeTemplate, AvatarInfo.LargeSize))
					}
				};
			}


		private string SafeGet (XDocument document, string path)
			{
			var element = document.XPathSelectElement (path);
			if (element == null)
				return null;

			return element.Value;
			}

		/// <summary>
		/// Friendly name of provider (OAuth service).
		/// </summary>
		public override string Name
			{
			get { return "LinkedIn"; }
			}
		}
	}