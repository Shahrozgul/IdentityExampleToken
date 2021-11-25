using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExampleToken.Controllers
{
    [Route("[controller]/[action]")]
    public class ChatSystemController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        public ChatSystemController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            #region outlook contact
            //var scopes = new[] { "User.Read.All" };

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            //var tenantId = "0b03cb40-4ced-44d3-b308-f737d88829b2";
            var tenantId = "6c92f381-7902-4245-a516-fa6bf927ffe9"; //prod

            // Values from app registration
            //var clientId = "c0d4882a-8c96-4d6d-900a-926117200b4d";
            var clientId = "879bb5ae-fa04-492e-b2bf-cc5e650bc009";//prod
            //var clientSecret = "8Vd7Q~BpeQAhGBenTRo4SdebAxRzskZhlnU.F";
            var clientSecret = "gVg7Q~xKqQW4CXo_ci691bKLQAY5pcIX7~sia";//prod

            IEnumerable<string> scopes = new List<string> { "User.Read.All", "Contacts.Read", "Directory.Read.All" };
            string authority = "https://login.microsoftonline.com/organizations/";

            // Create a client application.
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                            .Create(clientId)
                            .WithTenantId(tenantId)
                            .WithClientSecret(clientSecret)
                            .Build();
            // Create an authentication provider.
            ClientCredentialProvider authenticationProvider = new ClientCredentialProvider(confidentialClientApplication);
            // Configure GraphServiceClient with provider.
            GraphServiceClient graphServiceClient = new GraphServiceClient(authenticationProvider);
            // Make a request

            var mc = await graphServiceClient.Users.Request().GetAsync();
            var c = await graphServiceClient.Contacts.Request().GetAsync();
            foreach (var item in mc)
            {
                string a = "";
                string b = "";
                if (a == b)
                {
                    try
                    {
                        var me = await graphServiceClient.Users[item.Id].Contacts.Request().GetAsync();
                        foreach (var item2 in me)
                        {
                            string displayName = item2.DisplayName;
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }


            // Use the API reference to determine which scopes are appropriate for your API request.
            #endregion


























            return View();
        }
    }
}
