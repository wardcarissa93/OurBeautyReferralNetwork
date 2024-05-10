using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using Stripe;
using Stripe.Checkout;

namespace OurBeautyReferralNetwork.Repositories
{
    public class SubscriptionRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;
        private readonly FeeRepo _feeRepo;

        public SubscriptionRepo(ApplicationDbContext context, obrnDbContext obrnContext, FeeRepo feeRepo)
        {
            _context = context;
            _obrnContext = obrnContext;
            _feeRepo = feeRepo;
        }

        public IEnumerable<Models.Subscription> GetAllSubscriptionsBase()
        {
            return _obrnContext.Subscriptions.ToList();
        }

        public IEnumerable<Models.Subscription> GetAllSubscriptionsFromBusiness(string userId)
        {
            var allSubscriptions = GetAllSubscriptionsBase();

            // Filter transactions by UserId
            var subscriptionsFromUser = allSubscriptions.Where(s => s.FkBusinessId == userId);

            return subscriptionsFromUser;
        }

        public Models.Subscription CreateSubscriptionForBusiness(Session session, string userId, string feeStripeId, string transactionId)
        {
            var fee = _feeRepo.GetFeeByStripeId(feeStripeId);
            var StripeSubscriptionTitle = fee.Title;
            var StripeFeeType = fee.FeeType;
            var StripeFeeDescription = fee.Description;

            var createdSubscription = new Models.Subscription
            {
                FkBusinessId = userId,
                Amount = Convert.ToDecimal(session.AmountTotal),
                SubscriptionTitle = StripeSubscriptionTitle,
                FeeType = StripeFeeType,
                Description = StripeFeeDescription,
                IsActive = true,
                FkTransactionId = transactionId,

            };
            try
            {
                _obrnContext.Subscriptions.Add(createdSubscription);
                _obrnContext.SaveChanges();
                return createdSubscription;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public string DeleteSubscriptionsFromBusiness(string businessId)
        {
            try
            {
                IEnumerable<Models.Subscription> subscriptions = GetAllSubscriptionsFromBusiness(businessId);
                if (subscriptions == null)
                {
                    return "Subscription does not exist";
                }
                _obrnContext.Subscriptions.RemoveRange(subscriptions);
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
