using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace moneygram_api.Models
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            // Remove 'NZ' part before parsing
            base.DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.f";
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var dateString = reader.Value.ToString();
                // Remove 'NZ' if present
                dateString = dateString.Replace("NZ", "");
                return DateTime.ParseExact(dateString, base.DateTimeFormat, null);
            }
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }

    public class TransactionStatusEventPayload
    {
        [JsonProperty("eventId")]
        public string EventId { get; set; }

        [JsonProperty("eventDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EventDate { get; set; }

        [JsonProperty("subscriptionId")]
        public string SubscriptionId { get; set; }

        [JsonProperty("subscriptionType")]
        public string SubscriptionType { get; set; }

        [JsonProperty("eventPayload")]
        public TransactionStatusEvent EventPayload { get; set; }
    }

    public class TransactionStatusEvent
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("agentPartnerId")]
        public string AgentPartnerId { get; set; }

        [JsonProperty("referenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("partnerTransactionId")]
        public string PartnerTransactionId { get; set; }

        [JsonProperty("transactionSendDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? TransactionSendDate { get; set; }

        [JsonProperty("transactionStatusDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime TransactionStatusDate { get; set; }

        [JsonProperty("expectedPayoutDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? ExpectedPayoutDate { get; set; }

        [JsonProperty("transactionStatus")]
        public string TransactionStatus { get; set; }

        [JsonProperty("transactionSubStatus")]
        public TransactionSubStatus[] TransactionSubStatus { get; set; }

        // Optional financial details
        [JsonProperty("sendAmount")]
        public string SendAmount { get; set; }

        [JsonProperty("sendFee")]
        public string SendFee { get; set; }

        [JsonProperty("sendCurrency")]
        public string SendCurrency { get; set; }

        [JsonProperty("receiveAmount")]
        public string ReceiveAmount { get; set; }

        [JsonProperty("receiveCurrency")]
        public string ReceiveCurrency { get; set; }

        [JsonProperty("fxRate")]
        public string FxRate { get; set; }

        // Sender details
        [JsonProperty("sender")]
        public SenderDetails Sender { get; set; }

        // Receiver details
        [JsonProperty("receiver")]
        public ReceiverDetails Receiver { get; set; }
    }

    public class SenderDetails
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("secondLastName")]
        public string SecondLastName { get; set; }

        [JsonProperty("senderNationality")]
        public string SenderNationality { get; set; }

        [JsonProperty("address")]
        public SenderAddress Address { get; set; }

        [JsonProperty("dateOfBirth")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("personalId1Type")]
        public string PersonalId1Type { get; set; }

        [JsonProperty("personalId1Number")]
        public string PersonalId1Number { get; set; }
    }

    public class SenderAddress
    {
        [JsonProperty("line1")]
        public string Line1 { get; set; }

        [JsonProperty("line2")]
        public string Line2 { get; set; }

        [JsonProperty("line3")]
        public string Line3 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
    }

    public class ReceiverDetails
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("secondLastName")]
        public string SecondLastName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class TransactionSubStatus
    {
        [JsonProperty("subStatus")]
        public string SubStatus { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("targetCustomer")]
        public string TargetCustomer { get; set; }

        [JsonProperty("dataToCollect")]
        public DataToCollect[] DataToCollect { get; set; }
    }

    public class DataToCollect
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("dataCollection")]
        public string DataCollection { get; set; }
    }
}