using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OHMI_Keeper_League.Models
{
    [Serializable]
    public class OAuthTokenResponseModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("xoauth_yahoo_guid")]
        public string XOAuthYahooGUID { get; set; }

        public OAuthTokenResponseModel()
        {

        }
    }
}