using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class FeeRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public FeeRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<FeeAndCommission> GetAllFees()
        {
            return _obrnContext.FeeAndCommissions.ToList();
        }

        public FeeAndCommission GetFeeById(string feeId)
        {
            var fee = _obrnContext.FeeAndCommissions.FirstOrDefault(f => f.PkFeeId == feeId);
            if (fee == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return fee;
        }

    }
}
