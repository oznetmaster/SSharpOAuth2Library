#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// IClient.cs
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
using System.Collections.Specialized;
#if SSHARP
#if ASYNC
using SSMono.Threading;
using SSMono.Threading.Tasks;
#endif
using SSMono.Web;
#else
#if ASYNC
using System.Threading;
using System.Threading.Tasks;
#endif
using System.Web;
#endif
using OAuth2.Configuration;
using OAuth2.Models;

namespace OAuth2.Client
	{
	/// <summary>
	/// Defines API for doing user authentication using certain third-party service.
	/// </summary>
	/// <remarks>
	/// Standard flow is:
	/// - client is used to generate login link (<see cref="GetLoginLinkUriAsync"/>)
	/// - hosting app renders page with generated login link
	/// - user clicks login link - this leads to redirect to third-party service site
	/// - user authenticates and allows app access their basic information
	/// - third-party service redirects user to hosting app
	/// - hosting app reads user information using <see cref="GetUserInfoAsync"/> method
	/// </remarks>
	public interface IClient
		{
		/// <summary>
		/// Friendly name of provider (third-party authentication service). 
		/// Defined by client implementation developer and supposed to be unique.
		/// </summary>
		string Name { get; }

#if NETCF
		/// <summary>
		/// Returns URI of service which should be called in order to start authentication process. 
		/// You should use this URI when rendering login link.
		/// </summary>
		string GetLoginLinkUri ();

		/// <summary>
		/// Returns URI of service which should be called in order to start authentication process. 
		/// You should use this URI when rendering login link.
		/// </summary>
		string GetLoginLinkUri (string state);
#else
		/// <summary>
		/// Returns URI of service which should be called in order to start authentication process. 
		/// You should use this URI when rendering login link.
		/// </summary>
		string GetLoginLinkUri (string state = null);
#endif
		/// <summary>
		/// State which was posted as additional parameter 
		/// to service and then received along with main answer.
		/// </summary>
		string State { get; }

		/// <summary>
		/// Obtains user information using third-party authentication service 
		/// using data provided via callback request.
		/// </summary>
		/// <param name="parameters">
		/// Callback request payload (parameters).
		/// <example>Request.QueryString</example>
		/// </param>
		UserInfo GetUserInfo (NameValueCollection parameters);

#if ASYNC
		/// <summary>
		/// Obtains user information using third-party authentication service using data provided via callback request.
		/// </summary>
		/// <param name="parameters">Callback request payload (parameters).</param>
		/// <param name="cancellationToken">Optional cancellation token</param>
		/// <returns></returns>
		Task<UserInfo> GetUserInfoAsync (NameValueCollection parameters,
			CancellationToken cancellationToken = default);
#endif

		/// <summary>
		/// Client configuration object.
		/// </summary>
		IClientConfiguration Configuration { get; }
		}
	}