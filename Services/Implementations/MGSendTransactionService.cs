using moneygram_api.Data;
using moneygram_api.Models;
using moneygram_api.Services.Interfaces;
using System.Threading.Tasks;
using moneygram_api.DTOs;
using System;

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
            var transactionModel = new SendTransaction
            {
                SessionID = transaction.SessionID,
                ReferenceNumber = transaction.ReferenceNumber,
                Sender = $"{transaction.SenderFirstName} {transaction.SenderMiddleName} {transaction.SenderLastName}".Trim(),
                SenderFirstName = transaction.SenderFirstName,
                SenderMiddleName = transaction.SenderMiddleName,
                SenderLastName = transaction.SenderLastName,
                SenderAddress1 = transaction.SenderAddress1,
                SenderAddress2 = transaction.SenderAddress2,
                SenderAddress3 = transaction.SenderAddress3,
                OriginatingCountry = transaction.SenderCountry,
                SendCurrency = "USD",
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
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new Response { Success = true, TimeStamp = DateTime.Now, RecordId = transactionModel.ID, Message = "Transaction logged successfully." };
            }
            else
            {
                return new Response { Success = false, TimeStamp = DateTime.Now, Message = "Failed to log transaction." };
            }
        }
    }
}