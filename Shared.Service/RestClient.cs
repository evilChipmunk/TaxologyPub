using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace Shared.Service
{
    public interface IRestClient
    {
        Task<TResultContent> Get<TResultContent>(string baseUrl, string contextUrl, HeaderAccept headerAccept = null);

        Task Get<TResultContent>(
            string baseUrl
            , string contextUrl
            , Action<TResultContent> success
            , Action<RestException<TResultContent>> error
            , HeaderAccept headerAccept = null);

        Task Get<TResultContent>(
            string baseUrl
            , string contextUrl
            , object identifier
            , Action<TResultContent> success
            , Action<RestException<TResultContent>> error
            , HeaderAccept headerAccept = null);
        Task<TResultContent> Get<TResultContent>(string baseUrl, string contextUrl, object identifier = null, HeaderAccept headerAccept = null);
        Task<TResultContent> Post<TPostContent, TResultContent>(string baseUrl, string contextUrl, TPostContent content, HeaderAccept headerAccept = null);
        Task<TResultContent> Put<TPostContent, TResultContent>(string baseUrl, string contextUrl, TPostContent content, HeaderAccept headerAccept = null);
        Task<TResultContent> Delete<TPostContent, TResultContent>(string baseUrl, string contextUrl, TPostContent content, HeaderAccept json = null);
    }


    public class RestException<T> : Exception
    {
        public RestException(RestResponse<T> response)
        {
            this.Response = response;
        }

       public RestResponse<T> Response { get; set; }
    }

    public class RestClient : IRestClient
    {
 
        public async Task<TResultContent> Get<TResultContent>(string baseUrl, string contextUrl, HeaderAccept headerAccept = null)
        { 
            Uri requestUri = new Uri(new Uri(baseUrl), contextUrl);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                var message = await CreateRequestMessage(HttpMethod.Get, headerAccept, requestUri);
                 

                var restResponse = await GetResponse<TResultContent>(client, message);

                if (restResponse.IsSuccess)
                {
                    return restResponse.Result;
                }
                else
                {
                    throw new RestException<TResultContent>(restResponse);
                }
            } 
        }

        public Task<TResultContent> Get<TResultContent>(string baseUrl, string contextUrl, object identifier, HeaderAccept headerAccept = null)
        {
            string request = contextUrl;
            if (identifier != null)
            {
                string idPart = identifier.ToString();
                request = Path.Combine(request, idPart);
            }

            return Get<TResultContent>(baseUrl, request, headerAccept);
        }
 
        public async Task Get<TResultContent>(string baseUrl
            , string contextUrl
            , Action<TResultContent> success
            , Action<RestException<TResultContent>> error
            , HeaderAccept headerAccept = null
            )
        {
            Uri requestUri = new Uri(new Uri(baseUrl), contextUrl);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                var message = await CreateRequestMessage(HttpMethod.Get, headerAccept, requestUri);
                 
                var restResponse = await GetResponse<TResultContent>(client, message);

                if (restResponse.IsSuccess)
                {
                    success(restResponse.Result); 
                }
                else
                {
                    error(new RestException<TResultContent>(restResponse));
                }
            }
        }

        public Task Get<TResultContent>(
            string baseUrl, 
            string contextUrl, 
            object identifier,
            Action<TResultContent> success,
            Action<RestException<TResultContent>> error,
            HeaderAccept headerAccept = null)
        {
            string request = contextUrl;
            if (identifier != null)
            {
                string idPart = identifier.ToString();
                request = Path.Combine(request, idPart);
            }

            return Get<TResultContent>(baseUrl, request, success, error, headerAccept);
        }

        public async Task<TResultContent> Post<TPostContent, TResultContent>(string baseUrl, string contextUrl, TPostContent content, HeaderAccept headerAccept = null)
        {
            HttpMethod method = HttpMethod.Post;

            return await Execute<TPostContent, TResultContent>(baseUrl, contextUrl, content,method, headerAccept);
        }

        public async Task<TResultContent> Put<TPostContent, TResultContent>(string baseUrl, string contextUrl, TPostContent content, HeaderAccept headerAccept = null)
        {
            HttpMethod method = HttpMethod.Put;

            return await Execute<TPostContent, TResultContent>(baseUrl, contextUrl, content, method, headerAccept);
        }

        public async Task<TResultContent> Delete<TPostContent, TResultContent>(string baseUrl, string contextUrl, TPostContent content,
            HeaderAccept headerAccept = null)
        {
            HttpMethod method = HttpMethod.Delete;

            return await Execute<TPostContent, TResultContent>(baseUrl, contextUrl, content, method, headerAccept);
        }

        private async Task<TResult> Execute<TPost, TResult>(string baseUrl, string contextUrl, TPost content, HttpMethod method,
            HeaderAccept headerAccept = null)
        {
            Uri requestUri = new Uri(new Uri(baseUrl), contextUrl);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                

                var message = await CreateRequestMessage(method, headerAccept, requestUri);

                CreatePostContent(content, message);

                var restResponse = await GetResponse<TResult>(client, message);

                if (restResponse.IsSuccess)
                {
                    return restResponse.Result;
                }
                else
                {
                    throw new RestException<TResult>(restResponse);
                }
            }
        }


        private static async Task<RestResponse<TResultContent>> GetResponse<TResultContent>(
            HttpClient client, HttpRequestMessage message)
        {
            RestResponse<TResultContent> restResponse = new RestResponse<TResultContent>();
            try
            {
                var response = await client.SendAsync(message);
                restResponse.StatusCode = response.StatusCode;
                restResponse.ReasonPhrase = response.ReasonPhrase;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    restResponse.Result = JsonConvert.DeserializeObject<TResultContent>(responseString);
                    restResponse.IsSuccess = true;
                }
                else
                {
                    if (restResponse.ReasonPhrase != "Not Found")
                    {
                        restResponse.Error = "Response was not successful";
                    }
                } 
            }
            catch (HttpRequestException ex)
            {
                restResponse.Error = $"Message: {ex.Message} Exception: { ex.InnerException }"; 
            }

            return restResponse;
        }


        private async Task<HttpRequestMessage> CreateRequestMessage(HttpMethod method, HeaderAccept headerAccept,
            Uri requestUri)
        {
            var message = new HttpRequestMessage(method, requestUri);
            string authtoken = await GetAuthToken();
            if (!string.IsNullOrEmpty(authtoken))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
            }

            if (headerAccept != null && headerAccept != HeaderAccept.None)
            {
                message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(headerAccept.Type));
            } 

            return message;
        }
 
        private void CreatePostContent<TPostContent>(TPostContent content, HttpRequestMessage message)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            StringContent stringContent = new StringContent(jsonContent, Encoding.UTF8, HeaderAccept.Json.Type);
            message.Content = stringContent;
        }


        private async Task<string> GetAuthToken()
        {
            return null;
        }
    }
        public class RestResponse<T>
        {
            public T Result { get; set; }
            public HttpStatusCode StatusCode { get; set; }
            public string ReasonPhrase { get; set; }
            public string Error { get; set; }
            public bool IsSuccess { get; set; }
        }
}
