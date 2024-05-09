using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using Stripe;
using Stripe.BillingPortal;

namespace OurBeautyReferralNetwork.Repositories
{
    public class SubscriptionRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public SubscriptionRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Models.Subscription> GetAllSubscriptionsBase()
        {
            return _obrnContext.Subscriptions.ToList();
        }

        public Models.Subscription CreateSubscriptionForBusiness(Charge charge, string userId)
        {

            var createdSubscription = new Models.Subscription
            {
                FkBusinessId = userId,
                Amount = charge.Amount,
                SubscriptionTitle = "Subscription title",
                FeeType = "VIP",
                Description = "VIP upgrade fee",
                Frequency ="Monthly",
                IsActive = true,

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
