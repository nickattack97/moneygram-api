namespace moneygram_api.Exceptions;

using System;

public class BaseCustomException : Exception
{
    public int ErrorCode { get; }
    public string ErrorMessage { get; }
    public string OffendingField { get; }
    public DateTime TimeStamp { get; }

    public BaseCustomException(int errorCode, string errorMessage, string offendingField, DateTime timeStamp)
        : base($"Error Code: {errorCode}, Error Message: {errorMessage}, Offending Field: {offendingField}, TimeStamp: {timeStamp}")
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        OffendingField = offendingField;
        TimeStamp = timeStamp;
    }
}