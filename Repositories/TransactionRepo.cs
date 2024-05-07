using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class TransactionRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public TransactionRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Transaction> GetAllTransactionsBase()
        {
            return _obrnContext.Transactions.ToList();
        }

        //public Transaction CreateTransactionForBusiness(string session_id, decimal tax)
        //{
        //    var createdTransaction = new Transaction
        //    {
        //        PkTransactionId = session_id,
        //        FkSubscriptionId = transaction.FkSubscriptionId,
        //        FkBusinessId = transaction.FkBusinessId,
        //        FkCustomerId = transaction.FkCustomerId,
        //        BaseAmount = transaction.BaseAmount,
        //        Tax = tax,
        //        TotalAmount = transaction.TotalAmount,
        //        TransactionDate = DateTime.UtcNow,
        //        Description = transaction.Description,
        //        TransactionTitle = transaction.TransactionTitle,
        //    };
        //    try
        //    {
        //        _obrnContext.Transactions.Add(createdTransaction);
        //        _obrnContext.SaveChanges();
        //        return createdTransaction;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return null;
        //    }
        //}
    }
}
