﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mailjet.Client
{
    /// <summary>
    /// Mailjet API wrapper
    /// </summary>
    public class MailjetClient
    {
        private const string BaseAdress = "https://api.mailjet.com";
        private const string UserAgent = "mailjet-api-v3-net/1.0";
        private const string JsonMediaType = "application/json";
        private const string ApiVersion = "v3";

        private readonly HttpClient _httpClient;
        private readonly HttpClientHandler _httpClientHandler;

        public MailjetClient(string apiKey, string apiSecret, string baseAdress = BaseAdress)
        {
            // Create HttpClient
            _httpClientHandler = new HttpClientHandler();
            _httpClient = new HttpClient(_httpClientHandler);

            // Set base URI
            _httpClient.BaseAddress = new Uri(baseAdress);

            // Set accepted media type
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

            // Set user-agent
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);

            // Set basic authentification
            var byteArray = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", apiKey, apiSecret));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<MailjetResponse> GetAsync(MailjetRequest request)
        {
            string url = UrlHelper.CombineUrl(ApiVersion, request.BuildUrl());
            url = UrlHelper.AddQuerryString(url, request.Filters);

            var responseMessage = await _httpClient.GetAsync(url);

            JObject content = await GetContent(responseMessage);
            return new MailjetResponse(responseMessage.IsSuccessStatusCode, (int)responseMessage.StatusCode, content);
        }

        public async Task<MailjetResponse> PostAsync(MailjetRequest request)
        {
            string url = UrlHelper.CombineUrl(ApiVersion, request.BuildUrl());

            var responseMessage = await _httpClient.PostAsJsonAsync(url, request.Body);

            JObject content = await GetContent(responseMessage);
            return new MailjetResponse(responseMessage.IsSuccessStatusCode, (int)responseMessage.StatusCode, content);
        }

        public async Task<MailjetResponse> PutAsync(MailjetRequest request)
        {
            string url = UrlHelper.CombineUrl(ApiVersion, request.BuildUrl());

            var responseMessage = await _httpClient.PutAsJsonAsync(url, request.Body);

            JObject content = await GetContent(responseMessage);
            MailjetResponse mailjetResponse = new MailjetResponse(responseMessage.IsSuccessStatusCode, (int)responseMessage.StatusCode, content);
            return mailjetResponse;
        }

        public async Task<MailjetResponse> DeleteAsync(MailjetRequest request)
        {
            string url = UrlHelper.CombineUrl(ApiVersion, request.BuildUrl());
            url = UrlHelper.AddQuerryString(url, request.Filters);

            var responseMessage = await _httpClient.DeleteAsync(url);

            JObject content = await GetContent(responseMessage);
            return new MailjetResponse(responseMessage.IsSuccessStatusCode, (int)responseMessage.StatusCode, content);
        }

        private async Task<JObject> GetContent(HttpResponseMessage responseMessage)
        {
            JObject content;

            if (responseMessage.Content != null && responseMessage.Content.Headers.ContentType.MediaType == JsonMediaType)
            {
                content = await responseMessage.Content.ReadAsAsync<JObject>();
            }
            else
            {
                content = new JObject();
                content.Add("StatusCode", new JValue((int)responseMessage.StatusCode));

                if (!responseMessage.IsSuccessStatusCode)
                {
                    content.Add("ErrorInfo", new JValue(responseMessage.ReasonPhrase));
                }
            }

            return content;
        }
    }
}
