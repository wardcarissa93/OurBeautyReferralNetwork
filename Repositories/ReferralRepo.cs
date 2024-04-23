using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class ReferralRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public ReferralRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Referral> GetAllReferrals()
        {
            return _obrnContext.Referrals.ToList();
        }

        public Referral GetReferralById(int referralId)
        {
            var referral = _obrnContext.Referrals.FirstOrDefault(r => r.PkReferralId == referralId);
            if (referral == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return referral;
        }

    }
}
