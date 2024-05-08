using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using Stripe;
using Stripe.BillingPortal;

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

        public Transaction CreateTransactionForBusiness(Charge charge, string userId)
        {

            var createdTransaction = new Transaction
            {
                PkTransactionId = charge.Id,
                FkBusinessId = userId,
                BaseAmount = charge.Amount,
                TotalAmount = charge.AmountCaptured,
                TransactionDate = DateTime.UtcNow,
                Description = "VIP upgrade fee",
                TransactionTitle ="VIP upgrade fee",
            };
            try
            {
                _obrnContext.Transactions.Add(createdTransaction);
                _obrnContext.SaveChanges();
                return createdTransaction;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public Transaction CreateTransactionForCustomer(Charge charge, string userId)
        {

            var createdTransaction = new Transaction
            {
                PkTransactionId = charge.Id,
                FkCustomerId = userId,
                BaseAmount = charge.Amount,
                TotalAmount = charge.AmountCaptured,
                TransactionDate = DateTime.UtcNow,
                Description = "VIP upgrade fee",
                TransactionTitle = "VIP upgrade fee",
            };
            try
            {
                _obrnContext.Transactions.Add(createdTransaction);
                _obrnContext.SaveChanges();
                return createdTransaction;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
