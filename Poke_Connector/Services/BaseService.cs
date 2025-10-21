using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Poke_Connector.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Poke_Connector.Services
{
    public class BaseService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private string? _clientName;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        private HttpClient GetClient()
        {
            if (string.IsNullOrEmpty(_clientName))
                return httpClientFactory.CreateClient(Config.Constants.DefaultClientName);
            else
                return httpClientFactory.CreateClient(_clientName);
        }
        protected async Task<TResponse> GetAsync<TResponse>(string requestUri, ILogger log = null) where TResponse : class
        {
            if (log != null)
            {
                log.LogInformation($"RequestUri: {requestUri}");
            }

            var client = GetClient();

            if (log != null)
            {
                log.LogInformation($"Client baseaddress: {client.BaseAddress}");

            }

            var response = await client.GetAsync(requestUri);

            if (log != null)
            {
                log.LogInformation($"RequestMessage: {response.RequestMessage}");
            }

            await ThrowIfNotSuccessStatusCode(response, requestUri, "GET");
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<TResponse>(stringResponse)
                ?? throw new InvalidOperationException("Failed to deserialize the response.");

            return result;
        }
        private static async Task ThrowIfNotSuccessStatusCode(HttpResponseMessage response, string requestUri, string method, string? requestBody = null)
        {
            if (response.IsSuccessStatusCode)
                return;

            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception(content);
            }

            var message = new StringBuilder();
            message.AppendLine(response.ReasonPhrase ?? "Unexpected Error:");
            message.AppendLine($"Request URI: {requestUri}");
            message.AppendLine($"Request HTTP Method: {method}");
            if (requestBody != null)
                message.AppendLine($"Request Body: {requestBody}");

            message.AppendLine($"Response Status Code: {response.StatusCode}");
            message.AppendLine($"Response Content: {content}");

            var jsonResponse = JsonConvert.SerializeObject(response);
            message.AppendLine($"JSON Response: {jsonResponse}");

            throw new Exception(message.ToString());
        }
    }
}
