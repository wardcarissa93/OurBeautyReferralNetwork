using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.EntityExtensions;
using OurBeautyReferralNetwork.Models;
using static System.Net.Mime.MediaTypeNames;

namespace OurBeautyReferralNetwork.Repositories
{
    public class ServiceRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public ServiceRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Service> GetAllServicesBase()
        {
            return _obrnContext.Services.ToList();
        }

        public IEnumerable<Service> GetAllServices()
        {
            var services = GetAllServicesBase();

            var extendedServices = services.Select(service =>
            {
                decimal? discountPercentage = GetServiceDiscount(service.PkServiceId);
                decimal actualDiscount = discountPercentage ?? 0;

                return service.ExtendService(actualDiscount);
            });

            return null;
        }

        public Service GetServiceById(int serviceId)
        {
            var service = _obrnContext.Services.FirstOrDefault(t => t.PkServiceId == serviceId);
            if (service == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return service;
        }

        public decimal? GetServiceDiscount(int serviceId)
        {
            var service = GetServiceById(serviceId);
            if (service == null)
            {
                return null;
            }

            var query = from s in _obrnContext.Services
                        join d in _obrnContext.Discounts
                        on s.FkDiscountId equals d.PkDiscountId
                        where s.PkServiceId == serviceId
                        select new
                        {
                            DiscountPrice = s.BasePrice - (s.BasePrice * d.Percentage)
                        };

            var result = query.FirstOrDefault();

            return result?.DiscountPrice;
        }


        public IEnumerable<Service> GetAllServicesOfBusiness(string businessId)
        {
            var services =  _obrnContext.Services
                .Where(s => s.FkBusinessId == businessId)
                .ToList();

            var extendedServices = services.Select(service =>
            {
                decimal? discountPercentage = GetServiceDiscount(service.PkServiceId);
                decimal actualDiscount = discountPercentage ?? 0;

                return service.ExtendService(actualDiscount);
            });

            return null;
        }

        public bool CreateServiceForBusiness(Service service, string businessId)
        {
            bool isSuccess = true;
            try
            {
                _obrnContext.Services.Add(new Service
                {
                    PkServiceId = service.PkServiceId,
                    Image = service.Image,
                    FkBusinessId = businessId,
                    ServiceName = service.ServiceName,
                    Description = service.Description,
                    FkDiscountId = service.FkDiscountId,
                    BasePrice = service.BasePrice
                });
                _obrnContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
