using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class AccountService
    {
        private const string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();
        public AccountService() {
            client.Authenticator = new JwtAuthenticator(UserService.Token);
        }
        /// <summary>
        /// Return account details for this user
        /// </summary>
        public API_Account GetAccount()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account");
            IRestResponse<API_Account> response = client.Get<API_Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not communicate with server.");
                return null;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("Failed to retrieve balance.");
                return null;
            }
            return response.Data;
        }

        /// <summary>
        /// Return a list of all transfers associated with this user
        /// </summary>
        public List<API_Transfer> GetTransfers()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account/transfer");
            IRestResponse<List<API_Transfer>> response = client.Get<List<API_Transfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not communicate with server.");
                return null;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("Failed to retrieve transfers.");
                return null;
            }
            return response.Data;
        }

        /// <summary>
        /// Return a list of all TEnmo users
        /// </summary>
        public List<API_Member> GetMembers()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account/member");
            IRestResponse<List<API_Member>> response = client.Get<List<API_Member>>(request);

            return response.Data;
        }

        /// <summary>
        /// Submit a transfer to the server for validation & execution.
        /// </summary>
        public API_Transfer SubmitTransfer(API_Transfer transferRequest)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account/transfer");
            request.AddJsonBody(transferRequest);
            IRestResponse<API_Transfer> response = client.Post<API_Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not communicate with server.");
                return null;
            }
            if (!response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    JsonDeserializer deserial = new JsonDeserializer();
                    API_Error e = deserial.Deserialize<API_Error>(response);
                    Console.WriteLine($"Error: {e.Message}");
                }
                return null;
            }
            return response.Data;
        }
        public void FinalizePendingTransfer(API_Transfer transfer)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account/transfer");
            request.AddJsonBody(transfer);
            IRestResponse response = client.Put<API_Transfer>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not communicate with server.");
                return;
            }
            if (!response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    JsonDeserializer deserial = new JsonDeserializer();
                    API_Error e = deserial.Deserialize<API_Error>(response);
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
        }
    }
}
