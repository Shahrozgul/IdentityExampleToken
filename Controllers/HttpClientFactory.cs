﻿using System;
using System.Net.Http;

namespace IdentityExampleToken.Controllers
{
    public class HttpClientFactory
    {
        private string webServiceUrl = "https://api.blockchair.com/bitcoin/";

        public HttpClient CreateClient()
        {
            var client = new HttpClient();
            SetupClientDefaults(client);
            return client;
        }

        protected virtual void SetupClientDefaults(HttpClient client)
        {
            //This is global for all REST web service calls
            client.Timeout = TimeSpan.FromSeconds(60);
            client.BaseAddress = new Uri(webServiceUrl);
        }
    }
}