using CrispTv.Dtos;
using CrispTv.Interfaces;
using Flutterwave.Net;
using Microsoft.Extensions.Configuration;
using PayStack.Net;
using RestSharp;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Transactions;

namespace CrispTv.Services;

public class FlutterWaveService : IFlutterWaveService
{
    private readonly IConfiguration _configuration;
    private readonly string flutterwaveSecretKey;
    private readonly string baseUrl;
    private readonly string RedirectUrl;
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string ApiBaseUrl = "https://api.flutterwave.com/";
    private readonly string ApiSecretKey;

    private FlutterwaveApi Flutterwave { get; set; }
 
    public FlutterWaveService(IConfiguration configuration)
	{
        _configuration = configuration;
        flutterwaveSecretKey = _configuration["FlutterWave:SecretKey"];
        ApiSecretKey = _configuration["FlutterWave:SecretKey"];
        baseUrl = _configuration["FlutterWave:BaseUrl"];
        RedirectUrl = _configuration["FlutterWave:RedirectUrl"];
        Flutterwave = new FlutterwaveApi(flutterwaveSecretKey);
    }
    public string InitiatePayment(FlutterWavePaymentDto paymentDto)
    {
        var tnx_ref = GenerateRandomString(16);
        InitiatePaymentResponse response =  Flutterwave.Payments.InitiatePayment(tnx_ref, paymentDto.amount, RedirectUrl, paymentDto.name, paymentDto.emailAddress,
                                                                 paymentDto.phoneNumber, "Order", "This is coming from test app", "incoming.com");
   
        if (response.Status == "success")
        {
            // Get payment hosted link 
            string hostedLink = response.Data.Link;
            
            return  hostedLink;
        }
        throw new NotImplementedException(response.Message);
    }

    public  string PaymentWebhookAsync(decimal amount, string tx_ref, int transactionId)
    {
        /* using (var client = new HttpClient())
         {
             var apiUrl = baseUrl + $"{transaction_id}/verify";
             client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", flutterwaveSecretKey);
             var result = await client.GetFromJsonAsync<ValidationResponseDto>(apiUrl);
             if (result.data.tx_ref == tx_ref && result.data.status == "successful" && result.status == "success" && result.data.amount == amount)
             {
                 return new SuccessResponse()
                 {
                     IsSuccessful = true,
                     Message = "Payment Verified"
                 };
             }
             throw new NotImplementedException("Unable to verify");
         }*/

        /* var client = new RestClient("https://api.flutterwave.com");
         var request = new RestRequest($"/v3/transactions/{tx_ref}/verify", Method.Get);
         request.AddHeader("Content-Type", "application/json");
         request.AddHeader($"Authorization", $"Bearer {flutterwaveSecretKey}");
         var response = await client.ExecuteAsync(request);
         return response;*/

        int transaction_Id = 12345;
        VerifyTransactionResponse response = Flutterwave.Transactions.VerifyTransaction(transaction_Id);

        // success
        if (response.Status == "success")
        {
            // Get the transaction
            Flutterwave.Net.Transaction transaction = response.Data;

            // Verify transaction status
            bool isSuccess = transaction.Status == "successful";

            // Verify that the transaction reference, currency and charged amount i.e
            // transaction.TxRef, transaction.Currency and transaction.ChargedAmount
            // are what you expect them to be

        }
        // error
        throw new NotImplementedException("No Transaction Id Found");
        
    }

    public async Task<bool> VerifyPayment(string transactionId)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiSecretKey}");

            try
            {
                string apiUrl = $"{ApiBaseUrl}transactions/{transactionId}/verify";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Parse the API response to check the payment status
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Handle the response and extract the payment status information
                    // Example: {"status": "success", "message": "Transaction was successful"}
                    // Based on the status, you can determine whether the transaction was successful or not
                    // Return true if the transaction was successful, otherwise return false
                    return true;
                }
                else
                {
                    // Handle the case when the API request was not successful
                    // Return false or throw an exception based on your application's requirements
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any exceptions that occurred during the API call
                // Return false or throw an exception based on your application's requirements
                return false;
            }
        }
    }


    public static string GenerateRandomString(int length)
    {
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

