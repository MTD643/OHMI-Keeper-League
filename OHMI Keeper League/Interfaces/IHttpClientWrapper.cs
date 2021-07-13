using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static OHMI_Keeper_League.Enums.Enums;

namespace OHMI_Keeper_League.Interfaces
{
    public interface IHttpClientWrapper
    {
        public Task<string> RefreshAuthorization();
        public Task<T> CallAPI<T>(string uri, HttpContent content, ApiType apiType);
        public Task<bool> DeleteAsync(string uri);
    }
}
