namespace moneygram_api.Exceptions;

using System;

public class SoapFaultException : BaseCustomException
{
    public SoapFaultException(int errorCode, string errorMessage, string offendingField, DateTime timeStamp)
        : base(errorCode, errorMessage, offendingField, timeStamp)
    {
    }
}