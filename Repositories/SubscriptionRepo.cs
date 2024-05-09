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
    }
}
