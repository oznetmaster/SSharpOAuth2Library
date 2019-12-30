#region Copyright and License
// -----------------------------------------------------------------------------------------------------------------
// 
// RequestFactoryExtensions.cs
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
using OAuth2.Client;
using RestSharp;

namespace OAuth2.Infrastructure
	{
	public static class RequestFactoryExtensions
		{
		public static IRestClient CreateClient (this IRequestFactory factory, Endpoint endpoint)
			{
			var client = factory.CreateClient ();
			client.BaseUrl = new Uri (endpoint.BaseUri);
			return client;
			}

		public static IRestRequest CreateRequest (this IRequestFactory factory, Endpoint endpoint)
			{
			return CreateRequest (factory, endpoint, Method.GET);
			}

		public static IRestRequest CreateRequest (this IRequestFactory factory, Endpoint endpoint, Method method)
			{
			var request = factory.CreateRequest ();
			request.Resource = endpoint.Resource;
			request.Method = method;
			return request;
			}
		}
	}