using moneygram_api.Data;
using moneygram_api.Models;
using moneygram_api.Services.Interfaces;
using System.Threading.Tasks;
using moneygram_api.DTOs;
using System;
using Microsoft.EntityFrameworkCore;

namespace moneygram_api.Services.Implementations
{
    public class MGSendTransactionService : IMGSendTransactionService
    {
        private readonly AppDbContext _context;

        public MGSendTransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response> LogTransactionAsync(MGSendTransactionDTO transaction)
        {
            // Check if the record already exists
            var existingTransaction = await _context.SendTransactions
                .FirstOrDefaultAsync(t => t.ID == transaction.recordId);

            if (existingTransaction != null)
            {
                // Update the existing record
                existingTransaction.SessionID = transaction.SessionID;
                existingTransaction.ReferenceNumber = transaction.ReferenceNumber;
                existingTransaction.Sender = $"{transaction.SenderFirstName} {transaction.SenderMiddleName} {transaction.SenderLastName}".Trim();
                existingTransaction.SenderFirstName = transaction.SenderFirstName;
                existingTransaction.SenderMiddleName = transaction.SenderMiddleName;
                existingTransaction.SenderLastName = transaction.SenderLastName;
                existingTransaction.SenderGender = transaction.SenderGender;
                existingTransaction.SenderDOB = transaction.SenderDOB;
                existingTransaction.SenderPhotoIDType = transaction.SenderPhotoIdType;
                existingTransaction.SenderPhotoIDNumber = transaction.SenderPhotoIdNumber;
                existingTransaction.SenderAddress1 = transaction.SenderAddress1;
                existingTransaction.SenderAddress2 = transaction.SenderAddress2;
                existingTransaction.SenderAddress3 = transaction.SenderAddress3;
                existingTransaction.OriginatingCountry = transaction.SenderCountry;
                existingTransaction.SendCurrency = "USD";
                existingTransaction.ReceiveCurrency = transaction.ReceiveCurrency;
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Receiver = $"{transaction.ReceiverFirstName} {transaction.ReceiverMiddleName} {transaction.ReceiverLastName}".Trim();
                existingTransaction.ReceiverFirstName = transaction.ReceiverFirstName;
                existingTransaction.ReceiverMiddleName = transaction.ReceiverMiddleName;
                existingTransaction.ReceiverLastName = transaction.ReceiverLastName;
                existingTransaction.ReceiverAddress1 = transaction.ReceiverAddress1;
                existingTransaction.ReceiverAddress2 = transaction.ReceiverAddress2;
                existingTransaction.ReceiverAddress3 = transaction.ReceiverAddress3;
                existingTransaction.ReceiverCity = transaction.ReceiverCity;
                existingTransaction.ReceiverCountry = transaction.ReceiverCountry;
                existingTransaction.ReceiverState = transaction.ReceiverState;
                existingTransaction.ReceiverZipCode = transaction.ReceiverZipCode;
                existingTransaction.ReceiverPhotoIDType = transaction.ReceiverPhotoIDType;
                existingTransaction.ReceiverPhotoIDNumber = transaction.ReceiverPhotoIDNumber;

                _context.SendTransactions.Update(existingTransaction);
            }
            else
            {
                // Create a new record
                var transactionModel = new SendTransaction
                {
                    SessionID = transaction.SessionID,
                    ReferenceNumber = transaction.ReferenceNumber,
                    Sender = $"{transaction.SenderFirstName} {transaction.SenderMiddleName} {transaction.SenderLastName}".Trim(),
                    SenderFirstName = transaction.SenderFirstName,
                    SenderMiddleName = transaction.SenderMiddleName,
                    SenderLastName = transaction.SenderLastName,
                    SenderGender = transaction.SenderGender,
                    SenderDOB = transaction.SenderDOB,
                    SenderPhotoIDType = transaction.SenderPhotoIdType,
                    SenderPhotoIDNumber = transaction.SenderPhotoIdNumber,
                    SenderAddress1 = transaction.SenderAddress1,
                    SenderAddress2 = transaction.SenderAddress2,
                    SenderAddress3 = transaction.SenderAddress3,
                    OriginatingCountry = transaction.SenderCountry,
                    SendCurrency = "USD",
                    ReceiveCurrency = transaction.ReceiveCurrency,
                    Amount = transaction.Amount,
                    Receiver = $"{transaction.ReceiverFirstName} {transaction.ReceiverMiddleName} {transaction.ReceiverLastName}".Trim(),
                    ReceiverFirstName = transaction.ReceiverFirstName,
                    ReceiverMiddleName = transaction.ReceiverMiddleName,
                    ReceiverLastName = transaction.ReceiverLastName,
                    ReceiverAddress1 = transaction.ReceiverAddress1,
                    ReceiverAddress2 = transaction.ReceiverAddress2,
                    ReceiverAddress3 = transaction.ReceiverAddress3,
                    ReceiverCity = transaction.ReceiverCity,
                    ReceiverCountry = transaction.ReceiverCountry,
                    ReceiverState = transaction.ReceiverState,
                    ReceiverZipCode = transaction.ReceiverZipCode,
                    ReceiverPhotoIDType = transaction.ReceiverPhotoIDType,
                    ReceiverPhotoIDNumber = transaction.ReceiverPhotoIDNumber,
                };

                _context.SendTransactions.Add(transactionModel);
            }

            // Save changes to the database
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new Response
                {
                    Success = true,
                    TimeStamp = DateTime.Now,
                    RecordId = (existingTransaction?.ID ?? _context.SendTransactions.Local.FirstOrDefault()?.ID) ?? 0,
                    Message = existingTransaction != null ? "Transaction updated successfully." : "Transaction logged successfully."
                };
            }
            else
            {
                return new Response
                {
                    Success = false,
                    TimeStamp = DateTime.Now,
                    Message = "Failed to log/update transaction."
                };
            }
        }
    }
}