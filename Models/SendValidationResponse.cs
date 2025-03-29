namespace moneygram_api.Models.SendValidationResponse;

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
    [XmlElement(ElementName = "sendValidationResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public SendValidationResponse SendValidationResponse { get; set; }
}

public class SendValidationResponse
{
    [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool DoCheckIn { get; set; }

    [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime TimeStamp { get; set; }

    [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int Flags { get; set; }

    [XmlElement(ElementName = "mgiTransactionSessionID", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string MgiTransactionSessionID { get; set; }

    [XmlElement(ElementName = "customerReceiveNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string CustomerReceiveNumber { get; set; }

    [XmlElement(ElementName = "displayAccountID", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DisplayAccountID { get; set; }

    [XmlElement(ElementName = "promotionInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public PromotionInfo PromotionInfo { get; set; }

    [XmlElement(ElementName = "readyForCommit", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool ReadyForCommit { get; set; }

    [XmlElement(ElementName = "receiveAgentName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveAgentName { get; set; }

    [XmlElement(ElementName = "receiveAgentAddress", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveAgentAddress { get; set; }

    [XmlElement(ElementName = "sendAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public SendAmounts SendAmounts { get; set; }

    [XmlElement(ElementName = "receiveAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public ReceiveAmounts ReceiveAmounts { get; set; }

    [XmlElement(ElementName = "exchangeRateApplied", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal ExchangeRateApplied { get; set; }

    [XmlElement(ElementName = "receiveFeeDisclosureText", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool ReceiveFeeDisclosureText { get; set; }

    [XmlElement(ElementName = "receiveTaxDisclosureText", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool ReceiveTaxDisclosureText { get; set; }
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

    [XmlElement(ElementName = "detailReceiveAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<DetailReceiveAmounts> DetailReceiveAmounts { get; set; }
}

public class DetailReceiveAmounts
{
    [XmlElement(ElementName = "amountType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string AmountType { get; set; }

    [XmlElement(ElementName = "amount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal Amount { get; set; }

    [XmlElement(ElementName = "amountCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string AmountCurrency { get; set; }
}

public class PromotionInfo
{
    [XmlElement("promotionCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string PromotionCode { get; set; }

    [XmlElement("promotionDiscountId", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int PromotionDiscountId { get; set; }

    [XmlElement("promotionCategoryId", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int PromotionCategoryId { get; set; }

    [XmlElement("promotionDiscountAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal PromotionDiscountAmount { get; set; }

    [XmlElement("promotionErrorCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public long PromotionErrorCode { get; set; }

    [XmlElement("promotionErrorMessage", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public PromotionErrorMessage PromotionErrorMessage { get; set; }
}

public class PromotionErrorMessage
{
    [XmlElement("longLanguageCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string LongLanguageCode { get; set; }

    [XmlElement("textTranslation", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string TextTranslation { get; set; }
}