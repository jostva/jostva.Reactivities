using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.application.User;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace jostva.Reactivities.Infrastructure.Security
{
    public class FacebookAccessor : IFacebookAccesor
    {
        private readonly HttpClient httpClient;
        private readonly IOptions<FacebookAppSettings> config;


        public FacebookAccessor(IOptions<FacebookAppSettings> config)
        {
            this.config = config;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/")
            };

            httpClient.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        
        public async Task<FacebookUserInfo> FacebookLogin(string accessToken)
        {
            // Verify token is valid.
            HttpResponseMessage verifyToken = await httpClient.GetAsync($"debug_token?input_token={accessToken}&access_token={config.Value.AppId}|{config.Value.AppSecret}");

            if (!verifyToken.IsSuccessStatusCode)
            {
                return null;
            }

            FacebookUserInfo result = await GetAsync<FacebookUserInfo>(accessToken, "me", "fields=name,email,picture.width(100).height(100)");

            return result;
        }


        private async Task<T> GetAsync<T>(string accessToken, string endpoint, string args)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");

            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }

            string result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}