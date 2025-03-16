using moneygram_api.Data;
using moneygram_api.Models;
using moneygram_api.Services.Interfaces;
using moneygram_api.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace moneygram_api.Services.Implementations
{
    public class MGSendTransactionService : IMGSendTransactionService
    {
        private readonly AppDbContext _context;
        private readonly KycDbContext _kycContext; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MGSendTransactionService(AppDbContext context, KycDbContext kycContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _kycContext = kycContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<SendTransaction>> GetSendTransactionsAsync()
        {
            return await _context.SendTransactions.ToListAsync();
        }

         // New method to fetch transactions by a specific user (teller)
        public async Task<List<SendTransaction>> GetTransactionsByUserAsync(string username = null)
        {
            // If no username is provided, get the current user from the context
            if (string.IsNullOrEmpty(username))
            {
                username = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString();
                
                if (string.IsNullOrEmpty(username))
                {
                    throw new UnauthorizedAccessException("Username not found in token. Re-authenticate.");
                }
            }
            
            // Query transactions where TellerId matches the specified username
            return await _context.SendTransactions
                .Where(t => t.TellerId == username)
                .OrderByDescending(t => t.AddDate)
                .ToListAsync();
        }

        // New method to get transactions by the current authenticated user
        public async Task<List<SendTransaction>> GetMyTransactionsAsync()
        {
            string username = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString();
            
            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedAccessException("Username not found in token. Re-authenticate.");
            }
            
            return await GetTransactionsByUserAsync(username);
        }

             public async Task<SendTransaction> GetTransactionByReferenceNumberAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");
            }
            return await _context.SendTransactions
                .FirstOrDefaultAsync(t => t.ReferenceNumber == referenceNumber);
        }

        public async Task<Response> LogTransactionAsync(MGSendTransactionDTO transaction)
        {
            // Log the transaction to SendTransactions
            var existingTransaction = await _context.SendTransactions.FirstOrDefaultAsync(t =>
                t.ID == transaction.recordId || (t.SessionID == transaction.SessionID));

            SendTransaction transactionEntity;
            if (existingTransaction != null)
            {
                UpdateTransaction(existingTransaction, transaction);
                _context.SendTransactions.Update(existingTransaction);
                transactionEntity = existingTransaction;
            }
            else
            {
                transactionEntity = CreateNewTransaction(transaction);
                _context.SendTransactions.Add(transactionEntity);
            }

            // Check and log sender to tblClientele if not exists
            await LogSenderToClienteleAsync(transaction);

            var result = await _context.SaveChangesAsync();

            return new Response
            {
                Success = result > 0,
                TimeStamp = DateTime.Now,
                RecordId = transactionEntity.ID,
                Message = result > 0 ? (existingTransaction != null ? "Transaction updated successfully." : "Transaction logged successfully.") : "Failed to log/update transaction."
            };
        }

        private async Task LogSenderToClienteleAsync(MGSendTransactionDTO transaction)
        {
            var existingClient = await _kycContext.tblClientele
                .FirstOrDefaultAsync(c => c.NationalID == transaction.SenderPhotoIdNumber);

            // Extract pure Base64 from data URI if present
            string base64String = transaction.idImage ?? "";
            if (base64String.StartsWith("data:image/"))
            {
                // Split on comma and take the part after it
                var parts = base64String.Split(',');
                if (parts.Length > 1)
                {
                    base64String = parts[1];
                }
            }

            if (existingClient == null)
            {
                var newClient = new Clientele
                {
                    FirstName = transaction.SenderFirstName ?? "",
                    MiddleName = transaction.SenderMiddleName,
                    Surname = transaction.SenderLastName ?? "",
                    Gender = transaction.SenderGender ?? "",
                    NationalID = transaction.SenderPhotoIdNumber ?? "",
                    Address = transaction.SenderAddress1 ?? "",
                    City = transaction.SenderCity ?? "",
                    District = "",
                    Suburb = transaction.SenderAddress2 ?? "",
                    NatID_Image = string.IsNullOrEmpty(base64String) 
                        ? Array.Empty<byte>() 
                        : Convert.FromBase64String(base64String),
                    Img_Format = MimeToExtension.TryGetValue(transaction.contentType ?? "image/jpeg", out var extension) 
                        ? extension 
                        : ".jpeg",
                    ContentType = transaction.contentType ?? "image/jpeg",
                    AddDate = transaction.AddDate ?? DateTime.Now,
                    ModifiedDate = null,
                    Modification_Reason = null,
                    ModifiedBy = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString()
                };
                _kycContext.tblClientele.Add(newClient);
                await _kycContext.SaveChangesAsync();
            }
            else if (!string.IsNullOrEmpty(base64String) && (existingClient.NatID_Image == null || existingClient.NatID_Image.Length == 0))
            {
                // Update KYC image if missing
                existingClient.NatID_Image = Convert.FromBase64String(base64String);
                existingClient.Img_Format = MimeToExtension.TryGetValue(transaction.contentType ?? "image/jpeg", out var extension) 
                                            ? extension 
                                            : ".jpeg";
                existingClient.ContentType = transaction.contentType ?? "image/jpeg";
                existingClient.ModifiedDate = DateTime.Now;
                existingClient.ModifiedBy = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString();
                _kycContext.tblClientele.Update(existingClient);
                await _kycContext.SaveChangesAsync();
            }
        }

        private void UpdateTransaction(SendTransaction existing, MGSendTransactionDTO transaction)
        {
            existing.SessionID = UpdateIfNull(existing.SessionID, transaction.SessionID);
            existing.ReferenceNumber = UpdateIfNull(existing.ReferenceNumber, transaction.ReferenceNumber);

            existing.SenderFirstName = UpdateIfNull(existing.SenderFirstName, transaction.SenderFirstName);
            existing.SenderMiddleName = UpdateIfNull(existing.SenderMiddleName, transaction.SenderMiddleName);
            existing.SenderLastName = UpdateIfNull(existing.SenderLastName, transaction.SenderLastName);
            existing.Sender = ConcatenateNames(existing.SenderFirstName, existing.SenderMiddleName, existing.SenderLastName);

            existing.SenderGender = UpdateIfNull(existing.SenderGender, transaction.SenderGender);
            existing.SenderDOB = UpdateIfNull(existing.SenderDOB, transaction.SenderDOB);
            existing.SenderPhotoIDType = UpdateIfNull(existing.SenderPhotoIDType, transaction.SenderPhotoIdType);
            existing.SenderPhotoIDNumber = UpdateIfNull(existing.SenderPhotoIDNumber, transaction.SenderPhotoIdNumber);
            existing.SenderAddress1 = UpdateIfNull(existing.SenderAddress1, transaction.SenderAddress1);
            existing.SenderAddress2 = UpdateIfNull(existing.SenderAddress2, transaction.SenderAddress2);
            existing.SenderAddress3 = UpdateIfNull(existing.SenderAddress3, transaction.SenderAddress3);
            existing.SenderCity = UpdateIfNull(existing.SenderCity, transaction.SenderCity);
            existing.SenderCountry = UpdateIfNull(existing.SenderCountry, transaction.SenderCountry);
            existing.OriginatingCountry = UpdateIfNull(existing.OriginatingCountry, transaction.SenderCountry);
            existing.SenderPhoneNumber = UpdateIfNull(existing.SenderPhoneNumber, transaction.SenderPhoneNumber);

            existing.SendCurrency = existing.SendCurrency ?? "USD";
            existing.ReceiveCurrency = UpdateIfNull(existing.ReceiveCurrency, transaction.ReceiveCurrency);
            existing.Charge = UpdateIfNull(existing.Charge, transaction.Charge);
            existing.ReceiveAmount = UpdateIfNull(existing.ReceiveAmount, transaction.ReceiveAmount);
            existing.SendAmount = UpdateIfNull(existing.SendAmount, transaction.SendAmount);
            existing.ExchangeRate = UpdateIfNull(existing.ExchangeRate, transaction.ExchangeRate);
            existing.TotalAmountCollected = UpdateIfNull(existing.TotalAmountCollected, transaction.TotalAmountCollected);
            
            existing.ReceiverFirstName = UpdateIfNull(existing.ReceiverFirstName, transaction.ReceiverFirstName);
            existing.ReceiverMiddleName = UpdateIfNull(existing.ReceiverMiddleName, transaction.ReceiverMiddleName);
            existing.ReceiverLastName = UpdateIfNull(existing.ReceiverLastName, transaction.ReceiverLastName);
            existing.Receiver = ConcatenateNames(existing.ReceiverFirstName, existing.ReceiverMiddleName, existing.ReceiverLastName);

            existing.ReceiverAddress1 = UpdateIfNull(existing.ReceiverAddress1, transaction.ReceiverAddress1);
            existing.ReceiverAddress2 = UpdateIfNull(existing.ReceiverAddress2, transaction.ReceiverAddress2);
            existing.ReceiverAddress3 = UpdateIfNull(existing.ReceiverAddress3, transaction.ReceiverAddress3);
            existing.ReceiverCity = UpdateIfNull(existing.ReceiverCity, transaction.ReceiverCity);
            existing.ReceiverCountry = UpdateIfNull(existing.ReceiverCountry, transaction.ReceiverCountry);
            existing.ReceiverState = UpdateIfNull(existing.ReceiverState, transaction.ReceiverState);
            existing.ReceiverZipCode = UpdateIfNull(existing.ReceiverZipCode, transaction.ReceiverZipCode);
            existing.ReceiverPhotoIDType = UpdateIfNull(existing.ReceiverPhotoIDType, transaction.ReceiverPhotoIDType);
            existing.ReceiverPhotoIDNumber = UpdateIfNull(existing.ReceiverPhotoIDNumber, transaction.ReceiverPhotoIDNumber);
            existing.ReceiverPhoneNumber = UpdateIfNull(existing.ReceiverPhoneNumber, transaction.ReceiverPhoneNumber);

            existing.Occupation = UpdateIfNull(existing.Occupation, transaction.Occupation);
            existing.TransactionPurpose = UpdateIfNull(existing.TransactionPurpose, transaction.TransactionPurpose);
            existing.SourceOfFunds = UpdateIfNull(existing.SourceOfFunds, transaction.SourceOfFunds);
            existing.ConsumerID = UpdateIfNull(existing.ConsumerID, transaction.ConsumerID);

            existing.FormFree = UpdateIfNull(existing.FormFree, transaction.FormFree);
            existing.AddDate = UpdateIfNull(existing.AddDate, transaction.AddDate);
            existing.Successful = UpdateIfNull(existing.Successful, transaction.Successful);
            existing.Committed = UpdateIfNull(existing.Committed, transaction.Committed);
            existing.CommitDate = UpdateIfNull(existing.CommitDate, transaction.CommitDate);
            existing.Processed = UpdateIfNull(existing.Processed, transaction.Processed);
            existing.ProcessDate = UpdateIfNull(existing.ProcessDate, transaction.ProcessDate);

            existing.Reversed = UpdateIfNull(existing.Reversed, transaction.Reversed);
            existing.ReversalTime = UpdateIfNull(existing.ReversalTime, transaction.ReversalTime);
            existing.ReversalReason = UpdateIfNull(existing.ReversalReason, transaction.ReversalReason);
            existing.ReversalTellerId = UpdateIfNull(existing.ReversalTellerId, transaction.ReversalTellerId);            
        }

        private SendTransaction CreateNewTransaction(MGSendTransactionDTO transaction)
        {
            string operatorName = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString();

            if (string.IsNullOrEmpty(operatorName))
            {
                throw new UnauthorizedAccessException("Username name not found in token. Re-authenticate.");
            }

            return new SendTransaction
            {
                SessionID = transaction.SessionID,
                ReferenceNumber = transaction.ReferenceNumber,
                SenderFirstName = transaction.SenderFirstName,
                SenderMiddleName = transaction.SenderMiddleName,
                SenderLastName = transaction.SenderLastName,
                Sender = ConcatenateNames(transaction.SenderFirstName, transaction.SenderMiddleName, transaction.SenderLastName),
                SenderGender = transaction.SenderGender,
                SenderDOB = transaction.SenderDOB,
                SenderPhotoIDType = transaction.SenderPhotoIdType,
                SenderPhotoIDNumber = transaction.SenderPhotoIdNumber,
                SenderAddress1 = transaction.SenderAddress1,
                SenderAddress2 = transaction.SenderAddress2,
                SenderAddress3 = transaction.SenderAddress3,
                SenderCity = transaction.SenderCity,
                SenderCountry = transaction.SenderCountry,
                OriginatingCountry = transaction.SenderCountry,
                SenderPhoneNumber = transaction.SenderPhoneNumber,
                SendCurrency = "USD",
                ReceiveCurrency = transaction.ReceiveCurrency,
                Charge = transaction.Charge,
                ReceiveAmount = transaction.ReceiveAmount,
                SendAmount = transaction.SendAmount,
                ReceiverFirstName = transaction.ReceiverFirstName,
                ReceiverMiddleName = transaction.ReceiverMiddleName,
                ReceiverLastName = transaction.ReceiverLastName,
                Receiver = ConcatenateNames(transaction.ReceiverFirstName, transaction.ReceiverMiddleName, transaction.ReceiverLastName),
                ReceiverAddress1 = transaction.ReceiverAddress1,
                ReceiverAddress2 = transaction.ReceiverAddress2,
                ReceiverAddress3 = transaction.ReceiverAddress3,
                ReceiverCity = transaction.ReceiverCity,
                ReceiverCountry = transaction.ReceiverCountry,
                ReceiverState = transaction.ReceiverState,
                ReceiverZipCode = transaction.ReceiverZipCode,
                ReceiverPhotoIDType = transaction.ReceiverPhotoIDType,
                ReceiverPhotoIDNumber = transaction.ReceiverPhotoIDNumber,
                ReceiverPhoneNumber = transaction.ReceiverPhoneNumber,
                Occupation = transaction.Occupation,
                TransactionPurpose = transaction.TransactionPurpose,
                SourceOfFunds = transaction.SourceOfFunds,
                ConsumerID = transaction.ConsumerID,
                TellerId = operatorName,
                Reversed = transaction.Reversed ?? false, 
                ReversalTime = transaction.ReversalTime,
                ReversalReason = transaction.ReversalReason,
                ReversalTellerId = transaction.ReversalTellerId
            };
        }

        private static readonly Dictionary<string, string> MimeToExtension = new()
        {
            { "image/jpeg", ".jpeg" },
            { "image/jpg", ".jpg" },
            { "image/png", ".png" },
            { "image/gif", ".gif" },
            { "image/bmp", ".bmp" },
            { "image/webp", ".webp" },
            { "application/pdf", ".pdf" } // Added PDF support
        };

        private static T UpdateIfNull<T>(T existingValue, T newValue)
        {
            return existingValue == null ? newValue : existingValue;
        }

        private string ConcatenateNames(params string[] names)
        {
            return string.Join(" ", names.Where(name => !string.IsNullOrEmpty(name))).Trim();
        }
    }
}
