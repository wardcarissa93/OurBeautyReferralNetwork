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

        public IEnumerable<Transaction> GetAllTransactionsFromUser(string userId) 
        {
            var allTransactions = GetAllTransactionsBase();

            // Filter transactions by UserId
            var transactionsFromUser = allTransactions.Where(t => t.FkCustomerId == userId);

            return transactionsFromUser;
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

        public string DeleteTransactionsFromUser(string userId)
        {
            try
            {
                IEnumerable<Transaction> transactions = GetAllTransactionsFromUser(userId);
                if (transactions == null)
                {
                    return "Transaction does not exist";
                }
                _obrnContext.Transactions.RemoveRange(transactions);
                _obrnContext.SaveChanges();
                return "Deleted successfully";
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // You can also return a custom error message if needed
                Console.WriteLine($"Error occurred during delete: {ex.Message}");
                return "An error occurred during delete";
            }
        }
    }
}
