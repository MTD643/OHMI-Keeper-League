using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OHMI_Keeper_League.Models
{
    [Serializable]
    public class OAuthTokenRequestModel
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectURI { get; set; }
        
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public OAuthTokenRequestModel()
        {

        }

        public OAuthTokenRequestModel(string grantType, string code, string refreshToken)
        {
            this.GrantType = grantType;
            this.RedirectURI = "oob";
            this.Code = code;
            this.RefreshToken = refreshToken;
        }
    }
}
