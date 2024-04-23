using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class BusinessRepo
    {
        private readonly obrnDbContext _obrnDbContext;

        public BusinessRepo(obrnDbContext obrnDbContext)
        {
            _obrnDbContext = obrnDbContext;
        }

        public IEnumerable<Business> GetAllBusinesses()
        {
            IEnumerable<Business> businesses = _obrnDbContext.Businesses.ToList();
            return businesses;
        }
    }
}
