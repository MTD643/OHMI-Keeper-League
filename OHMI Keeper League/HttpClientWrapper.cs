using OHMI_Keeper_League.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OHMI_Keeper_League.Models;
using static OHMI_Keeper_League.Enums.Enums;
using System.Text;

namespace OHMI_Keeper_League
{
    public sealed class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _client;

        public HttpClientWrapper(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Environment.GetEnvironmentVariable("ENCODED_AUTHORIZATION"));
        }

        public async Task<string> RefreshAuthorization()
        {
            string refreshToken = Environment.GetEnvironmentVariable("REFRESH_TOKEN");
            string code = Environment.GetEnvironmentVariable("CODE");
            string grantType = "refresh_token";

            OAuthTokenRequestModel refreshTokenRequest = new OAuthTokenRequestModel(grantType, code, refreshToken);
            string json = JsonConvert.SerializeObject(refreshTokenRequest);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                OAuthTokenResponseModel tokenResponse = await CallAPI<OAuthTokenResponseModel>("https://api.login.yahoo.com/oauth2/get_token", content, ApiType.Post);

                if (tokenResponse != null && tokenResponse.AccessToken.HasValue())
                {
                    return tokenResponse.AccessToken;
                }
                else
                {
                    throw new Exception("Error refreshing auth token.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> CallAPI<T>(string uri, HttpContent content, ApiType apiType)
        {
            if (uri.HasValue())
            {
                try
                {
                    HttpResponseMessage response = null;

                    switch (apiType)
                    {
                        case ApiType.Get:
                            response = await _client.GetAsync(uri);
                            break;
                        case ApiType.Post:
                            if (content != null)
                                response = await _client.PostAsync(uri, content);
                            else
                                throw new ArgumentException("Content cannot be null when using a POST request");
                            break;
                        case ApiType.Put:
                            if (content != null)
                                response = await _client.PutAsync(uri, content);
                            else
                                throw new ArgumentException("Content cannot be null when using a PUT request");
                            break;
                        case ApiType.Patch:
                            if (content != null)
                                response = await _client.PatchAsync(uri, content);
                            else
                                throw new ArgumentException("Content cannot be null when using a PATCH request");
                            break;
                    }

                    return await ValidateResponse<T>(response);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("Parameters cannot be null.");
            }
        }

        public async Task<bool> DeleteAsync(string uri)
        {
            if (uri.HasValue())
            {
                bool success;

                try
                {
                    HttpResponseMessage response = await _client.DeleteAsync(uri);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = "Unknown Error";

                        if (response != null && response.Content != null)
                        {
                            errorMessage = response.Content.ReadAsStringAsync().Result;
                        }

                        throw new Exception(errorMessage);
                    }
                    else
                    {
                        success = true;
                    }

                    return success;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("Parameter cannot be null.");
            };
        }

        async Task<T> ValidateResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();

                if (!jsonResult.HasValue())
                {
                    return default(T);
                }
                else if (typeof(T) == typeof(string))
                {
                    return (T)(object)jsonResult;
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(jsonResult);
                }
            }
            else
            {
                string errorMessage = "Unknown Error";

                if (response != null && response.Content != null)
                {
                    errorMessage = response.Content.ReadAsStringAsync().Result;
                }

                throw new ApplicationException(errorMessage);
            }
        }
    }
}