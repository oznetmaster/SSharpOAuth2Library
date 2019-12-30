using SSMono.Net;
using System.Threading;
#if ASYNC
using System.Threading.Tasks;
#endif
using OAuth2.Client;
using RestSharp;

namespace OAuth2.Infrastructure
	{
	public static class RestClientExtensions
		{
		public static IRestResponse ExecuteAndVerify (this IRestClient client, IRestRequest request)
			{
			return VerifyResponse (client.Execute (request));
			}

		private static IRestResponse VerifyResponse (IRestResponse response)
			{
			if (response.Content.IsEmpty () ||
			    (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created))
				{
				throw new UnexpectedResponseException (response);
				}

			return response;
			}

#if ASYNC
		public static async Task<IRestResponse> ExecuteAndVerifyAsync (this IRestClient client, IRestRequest request, CancellationToken cancellationToken = default)
			{
			return VerifyResponse (await client.ExecuteTaskAsync (request, cancellationToken).ConfigureAwait (false));
			}
#endif
		}
	}