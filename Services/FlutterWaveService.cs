using CrispTv.Dtos;
using CrispTv.Interfaces;
using Flutterwave.Net;
using Newtonsoft.Json;
using Rave;
using Rave.Models;
/*using Flutterwave.Net;*/
using Rave.Models.Charge;
using RestSharp;

namespace CrispTv.Services;

public class FlutterWaveService : IFlutterWaveService
{
    private readonly IConfiguration _configuration;
    private readonly string flutterwaveSecretKey;
    private readonly string RedirectUrl;
    private readonly string PbKey;
    private readonly string ScKey;

    private static RaveConfig raveConfig { get; set; } = null!;
    private static ChargeCard cardCharge { get; set; } = null!;

    private FlutterwaveApi Flutterwave { get; set; }

    public FlutterWaveService(IConfiguration configuration)
    {
        _configuration = configuration;
        flutterwaveSecretKey = _configuration["FlutterWave:SecretKey"];
        ScKey = _configuration["FlutterWave:SecretKey"];
        PbKey = _configuration["FlutterWave:PublicKey"];
        RedirectUrl = _configuration["FlutterWave:RedirectUrl"];

        raveConfig = new RaveConfig(PbKey, ScKey, false);
        /*cardCharge = new ChargeCard(raveConfig);*/
        Flutterwave = new FlutterwaveApi(flutterwaveSecretKey);

    }
    public ResultResponse InitiatePayment(FlutterWavePaymentDto paymentDto)
    {
        var tnx_ref = GenerateRandomString(20);
        InitiatePaymentResponse response = Flutterwave.Payments.InitiatePayment(tnx_ref, paymentDto.amount, RedirectUrl, paymentDto.name, paymentDto.emailAddress,
                                                                 paymentDto.phoneNumber, "Order", "This is coming from test app", "incoming.com");
        if (response.Status == "success")
        {
            string hostedLink = response.Data.Link;

            return new ResultResponse()
            {
                Message = "Successful",
                IsSuccessful = true,
                Data = hostedLink
            };
        }
        return new ResultResponse()
        {
            IsSuccessful = false,
            Message = response.Message
        };
    }

    public ResultResponse PaymentWebhookAsync(decimal amount, string tx_ref, int transactionId)
    {

        VerifyTransactionResponse response = Flutterwave.Transactions.VerifyTransaction(transactionId);
        Transaction transaction = response.Data;

        if (transaction.Status == "successful" && response.Status == "success" && transaction.Amount == amount && transaction.TxRef == tx_ref)
        {
            return new ResultResponse()
            {
                Message = "Tarnsaction was successful",
                IsSuccessful = true
            };
        }

        return new ResultResponse()
        {
            Message = "Transaction Failed",
            IsSuccessful = false
        };
    }




    private static string GenerateRandomString(int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public async Task<BankResponse> BankAccount()
    {
        /*  var accountCharge = new ChargeAccount(raveConfig);
          var Payload = new AccountParams(PbKey, ScKey, "firstname","lastname","user@example.com", "0690000031", 1000.00m, "440", "NGN", "MC-0292920");
          var chargeResponse = await accountCharge.Charge(Payload);

          if (chargeResponse.Data.Status == "success-pending-validation")
          {
              // This usually means the user needs to validate the transaction with an OTP
              Payload.Otp = "12345";
              chargeResponse = accountCharge.Charge(Payload).Result;
              return new BankResponse()
              {
                  Instruction = chargeResponse.Data.ValidateInstructions.Instruction,
                  ValidateInstruction = chargeResponse.Data.ValidateInstruction,
                  Valparams = chargeResponse.Data.ValidateInstructions.Valparams
              };
          }
          return new BankResponse()
          {
              Instruction = chargeResponse.Data.ValidateInstructions.Instruction,
              ValidateInstruction = chargeResponse.Data.ValidateInstruction,
              Valparams = chargeResponse.Data.ValidateInstructions.Valparams
          };*/

        var client = new RestClient("https://api.flutterwave.com");
        var request = new RestRequest($"/v3/virtual-account-numbers", Method.Post);
        request.AddHeader($"Authorization", $"Bearer {flutterwaveSecretKey}");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("verif-hash","hdhdhjjndsjkndjk");
        var jsonData = new {
            email = "Henry@domain.com",
            amount = 200,
            narration = "Henry Test account",
            tx_ref = GenerateRandomString(20),
            currency = "NGN"


        };
        request.AddJsonBody(jsonData);
        var response = await client.ExecuteAsync(request);
        BankResponse myClass = JsonConvert.DeserializeObject<BankResponse>(response.Content);
        if (!response.IsSuccessful)
        {
           return myClass;
        }

        return myClass;
        // var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        /* string jsonObject = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
         request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);*/
    }

    /*  public string Initiate()
      {
          string tranxRef = GenerateRandomString(26);
          var Payload = new CardParams(PbKey, ScKey, "Customer", "LastName", "tester@example.com", 1100, "NGN") {  CardNo = "5531886652142950", Cvv = "564", Expirymonth = "09", Expiryyear = "32", TxRef = tranxRef };
          var cha = cardCharge.Charge(Payload).Result;
          if (cha.Message == "AUTH_SUGGESTION" && cha.Data.SuggestedAuth == "PIN" && cha.Data.Amount == (decimal)1100.00)
          {
              *//* cardParams.Pin = "3310";
               cardParams.Otp = "12345";
               cardParams.SuggestedAuth = "PIN";*//*

              // cha = cardCharge.Charge(Payload).Result;

              return cha.Message;
          }
          throw new NotImplementedException(cha.Message);
      }*/
}

