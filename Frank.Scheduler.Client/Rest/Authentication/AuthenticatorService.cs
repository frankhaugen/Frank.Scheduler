using Frank.Scheduler.Client.Configuration;
using RestSharp;
using System;
using System.Text;

namespace Frank.Scheduler.Client.Rest
{
    public class AuthenticatorService
    {
        public AccessToken GetToken(SchedulerRestConfiguration restConfiguration, AuthenticationOptions authenticationOptions)
        {
            var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationOptions.ClientId + ":" + authenticationOptions.ClientSecret));
            var client = new RestClient(authenticationOptions.BaseUrl);
            client.Timeout = 30;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Basic {base64String}");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("grant_type", "password");
            request.AddParameter("scope", authenticationOptions.Scope);
            request.AddParameter("username", authenticationOptions.UserName);
            request.AddParameter("password", authenticationOptions.Password);
            var response = client.Execute<AccessToken>(request);

            return response.Data;
        }
    }

    public class AccessToken
    {
        public string? Token { get; set; }

        public string? TokenType { get; set; }

        public string? Scope { get; set; }
    }

    public class AuthenticationOptions
    {
        public AuthenticationOptions(string baseUrl, string userName, string password, string companyId, string clientId, string clientSecret, string scope)
        {
            BaseUrl = baseUrl;
            UserName = userName;
            Password = password;
            CompanyId = companyId;
            ClientId = clientId;
            ClientSecret = clientSecret;
            Scope = scope;
        }

        public string? BaseUrl;
        public string? UserName;
        public string? Password;
        public string? CompanyId;
        public string? ClientId;
        public string? ClientSecret;
        public string? Scope;
    }
}
