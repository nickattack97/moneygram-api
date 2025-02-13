using System;
using System.Collections.Generic;

namespace moneygram_api.Utilities
{
    public static class ErrorDictionary
    {
        private static readonly Dictionary<int, ErrorResponse> _errorResponses = new()
        {
            { 616, new ErrorResponse(616, "Customer is not found", "customerPhone") },
            { 404, new ErrorResponse(404, "Resource not found", "resourceId") },
            { 400, new ErrorResponse(400, "Invalid request", "requestBody") },
            { 503, new ErrorResponse(503, "Service Unavailable", "service") },
            { 204, new ErrorResponse(204, "No fee information found for the provided filters", "filteredFeeInfo") } // Added 204 status code
        };

        public static ErrorResponse GetErrorResponse(int errorCode, string offendingField = null)
        {
            if (_errorResponses.TryGetValue(errorCode, out var errorResponse))
            {
                return new ErrorResponse(
                    errorResponse.ErrorCode,
                    errorResponse.ErrorMessage,
                    offendingField ?? errorResponse.OffendingField
                );
            }

            return new ErrorResponse(500, "Unknown error occurred", offendingField);
        }
    }

    public class ErrorResponse
    {
        public int ErrorCode { get; }
        public string ErrorMessage { get; }
        public string OffendingField { get; }
        public string TimeStamp { get; }

        public ErrorResponse(int errorCode, string errorMessage, string offendingField)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            OffendingField = offendingField;
            TimeStamp = DateTime.Now.ToString("o"); // ISO 8601 format
        }
    }
}