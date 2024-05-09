using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using Stripe;
using Stripe.Checkout;

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

        public Transaction CreateTransactionForBusiness(Session session, string userId)
        {

            var createdTransaction = new Transaction
            {
                PkTransactionId = session.Id,
                FkBusinessId = userId,
                BaseAmount = Convert.ToDecimal(session.AmountTotal),
                TotalAmount = Convert.ToDecimal(session.AmountTotal),
                TransactionDate = DateTime.Now,
                Description = "Business VIP upgrade fee",
                TransactionTitle ="Business VIP upgrade fee",
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

        public Transaction CreateTransactionForCustomer(Session session, string userId)
        {

            var createdTransaction = new Transaction
            {
                PkTransactionId = session.Id,
                FkCustomerId = userId,
                BaseAmount = Convert.ToDecimal(session.AmountTotal),
                TotalAmount = Convert.ToDecimal(session.AmountTotal),
                TransactionDate = DateTime.Now,
                Description = "Customer VIP upgrade fee",
                TransactionTitle = "Customer VIP upgrade fee",
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
