using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.Commonlibs
{
    public static class HttpHelper
    {
        public static async Task<string> HttpSendGet(string baseUrl, string api)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync(api);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static async Task<string> HttpSendPost(string baseUrl, string api, string? jsonData)
        {
            try
            {
                using var client = new HttpClient();
                var url = baseUrl + api;
                HttpResponseMessage response = new();

                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var queryString = jsonData != null ? new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json") : null;
                response = await client.PostAsync(new Uri(url), queryString);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                    return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static async Task<string> HttpSenPostWithToken(string baseUrl, string api, string jsonData, string token)
        {
            try
            {
                using var client = new HttpClient();
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var queryString = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");
                var url = baseUrl + api;
                HttpResponseMessage response = await client.PostAsync(new Uri(url), queryString);

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
