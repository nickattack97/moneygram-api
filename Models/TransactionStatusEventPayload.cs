
using Newtonsoft.Json;

namespace moneygram_api.Models
{
    public class TransactionStatusEventPayload
    {
        [JsonProperty("eventId")]
        public string EventId { get; set; }

        [JsonProperty("eventDate")]
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
        public DateTime TransactionSendDate { get; set; }

        [JsonProperty("transactionStatusDate")]
        public DateTime TransactionStatusDate { get; set; }

        [JsonProperty("transactionStatus")]
        public string TransactionStatus { get; set; }

        [JsonProperty("transactionSubStatus")]
        public TransactionSubStatus[] TransactionSubStatus { get; set; }
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