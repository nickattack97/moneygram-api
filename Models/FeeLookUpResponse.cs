namespace moneygram_api.Models.FeeLookUpResponse;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using moneygram_api.Models;

[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class Envelope : ResponsesBaseEnvelope
{
    [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public Body Body { get; set; }
}

public class Body
{
    [XmlElement(ElementName = "feeLookupResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public FeeLookUpResponse FeeLookUpResponse { get; set; }
}

public class FeeLookUpResponse
{
    [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool DoCheckIn { get; set; }

    [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime TimeStamp { get; set; }

    [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int Flags { get; set; }

    [XmlElement(ElementName = "feeInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<FeeInfo> FeeInfo { get; set; }

    [XmlElement(ElementName = "errorCode", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string ErrorCode { get; set; }

    [XmlElement(ElementName = "errorMessage", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string ErrorMessage { get; set; }

    [XmlElement(ElementName = "offendingField", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string OffendingField { get; set; }
}

public class FeeInfo
{
    [XmlElement(ElementName = "validReceiveAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal ValidReceiveAmount { get; set; }

    [XmlElement(ElementName = "validReceiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ValidReceiveCurrency { get; set; }

    [XmlElement(ElementName = "validExchangeRate", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal ValidExchangeRate { get; set; }

    [XmlElement(ElementName = "estimatedReceiveAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal EstimatedReceiveAmount { get; set; }

    [XmlElement(ElementName = "estimatedReceiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string EstimatedReceiveCurrency { get; set; }

    [XmlElement(ElementName = "estimatedExchangeRate", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal EstimatedExchangeRate { get; set; }

    [XmlElement(ElementName = "totalAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal TotalAmount { get; set; }

    [XmlElement(ElementName = "receiveCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCountry { get; set; }

    [XmlElement(ElementName = "deliveryOption", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DeliveryOption { get; set; }

    [XmlElement(ElementName = "receiveAmountAltered", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool ReceiveAmountAltered { get; set; }

    [XmlElement(ElementName = "revisedInformationalFee", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool RevisedInformationalFee { get; set; }

    [XmlElement(ElementName = "deliveryOptId", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int DeliveryOptId { get; set; }

    [XmlElement(ElementName = "deliveryOptDisplayName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DeliveryOptDisplayName { get; set; }

    [XmlElement(ElementName = "mgiTransactionSessionID", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string MgiTransactionSessionID { get; set; }

    [XmlElement(ElementName = "sendAmountAltered", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool SendAmountAltered { get; set; }

    [XmlElement(ElementName = "sendAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public SendAmounts SendAmounts { get; set; }

    [XmlElement(ElementName = "receiveAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public ReceiveAmounts ReceiveAmounts { get; set; }

    [XmlElement(ElementName = "exchangeRateApplied", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal ExchangeRateApplied { get; set; }

    [XmlElement(ElementName = "exchangeRateExpiration", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime ExchangeRateExpiration { get; set; }

    [XmlElement(ElementName = "exchangeRateSource", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ExchangeRateSource { get; set; }
}

public class SendAmounts
{
    [XmlElement(ElementName = "sendAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal SendAmount { get; set; }

    [XmlElement(ElementName = "sendCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SendCurrency { get; set; }

    [XmlElement(ElementName = "totalSendFees", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal TotalSendFees { get; set; }

    [XmlElement(ElementName = "totalDiscountAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal TotalDiscountAmount { get; set; }

    [XmlElement(ElementName = "totalSendTaxes", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal TotalSendTaxes { get; set; }

    [XmlElement(ElementName = "totalAmountToCollect", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal TotalAmountToCollect { get; set; }

    [XmlElement(ElementName = "detailSendAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<DetailSendAmounts> DetailSendAmounts { get; set; }
}

public class DetailSendAmounts
{
    [XmlElement(ElementName = "amountType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string AmountType { get; set; }

    [XmlElement(ElementName = "amount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal Amount { get; set; }

    [XmlElement(ElementName = "amountCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string AmountCurrency { get; set; }
}

public class ReceiveAmounts
{
    [XmlElement(ElementName = "receiveAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal ReceiveAmount { get; set; }

    [XmlElement(ElementName = "receiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCurrency { get; set; }

    [XmlElement(ElementName = "validCurrencyIndicator", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool ValidCurrencyIndicator { get; set; }

    [XmlElement(ElementName = "payoutCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string PayoutCurrency { get; set; }

    [XmlElement(ElementName = "totalReceiveFees", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal TotalReceiveFees { get; set; }

    [XmlElement(ElementName = "totalReceiveTaxes", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal TotalReceiveTaxes { get; set; }

    [XmlElement(ElementName = "totalReceiveAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal TotalReceiveAmount { get; set; }

    [XmlElement(ElementName = "receiveFeesAreEstimated", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool ReceiveFeesAreEstimated { get; set; }

    [XmlElement(ElementName = "receiveTaxesAreEstimated", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool ReceiveTaxesAreEstimated { get; set; }

    [XmlElement(ElementName = "detailEstimatedReceiveAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<DetailEstimatedReceiveAmounts> DetailEstimatedReceiveAmounts { get; set; }
}

public class DetailEstimatedReceiveAmounts
{
    [XmlElement(ElementName = "amountType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string AmountType { get; set; }

    [XmlElement(ElementName = "amount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal Amount { get; set; }

    [XmlElement(ElementName = "amountCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string AmountCurrency { get; set; }
}