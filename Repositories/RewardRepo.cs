using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class RewardRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public RewardRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Reward> GetAllRewards()
        {
            return _obrnContext.Rewards.ToList();
        }

        public Reward GetRewardById(int rewardId)
        {
            var reward = _obrnContext.Rewards.FirstOrDefault(r => r.PkRewardId == rewardId);
            if (reward == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return reward;
        }

    }
}
