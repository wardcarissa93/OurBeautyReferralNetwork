using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
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

        public IEnumerable<ServiceDTO> GetAllServices()
        {
            var services = GetAllServicesBase();

            var extendedServices = services.Select(service =>
            {
                decimal? discountPercentage = GetServiceDiscount(service.PkServiceId);
                decimal actualDiscount = discountPercentage ?? 0;
                DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
                Discount discount = discountRepo.GetDiscountById(service.FkDiscountId);

                return service.ExtendService(discount);
            });

            return extendedServices;
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
                            DiscountPercentage = s.BasePrice - (s.BasePrice * d.Percentage)
                        };

            var result = query.FirstOrDefault();

            return result?.DiscountPercentage;
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
                DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
                Discount discount = discountRepo.GetDiscountById(service.FkDiscountId);

                return service.ExtendService(discount);
            });

            return null;
        }

        public ServiceDTO GetServiceOfBusiness(string businessId, int serviceId)
        {
            var service = _obrnContext.Services
                .FirstOrDefault(s => s.FkBusinessId == businessId && s.PkServiceId == serviceId);

            if (service != null)
            {
                decimal? discountPercentage = GetServiceDiscount(service.PkServiceId);
                decimal actualDiscount = discountPercentage ?? 0;
                DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
                Discount discount = discountRepo.GetDiscountById(service.FkDiscountId);

                return service.ExtendService(discount);
            }


            return null;
        }

        public Service CreateServiceForBusiness(ServiceDTO serviceDTO)
        {
            DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
            Discount discount = discountRepo.GetDiscountById(serviceDTO.FkDiscountId);
            var service = new Service
            {
                PkServiceId = serviceDTO.PkServiceId,
                Image = serviceDTO.Image,
                FkBusinessId = serviceDTO.FkBusinessId,
                ServiceName = serviceDTO.ServiceName,
                Description = serviceDTO.Description,
                FkDiscountId = serviceDTO.FkDiscountId,
                BasePrice = serviceDTO.BasePrice,
                FkDiscount = discount,
                FkCategoryId = serviceDTO.FkCategoryId,
            };
            try
            {
                _obrnContext.Services.Add(service);
                _obrnContext.SaveChanges();
                return service;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public bool EditServiceForBusiness(ServiceDTO serviceDTO, string businessId)
        {
            DiscountRepo discountRepo = new DiscountRepo (_context, _obrnContext);
            Discount discount = discountRepo.GetDiscountById(serviceDTO.FkDiscountId);
            Service service = GetServiceById(serviceDTO.PkServiceId);
            if (service != null)
            {
                service.PkServiceId = serviceDTO.PkServiceId;
                service.Image = serviceDTO.Image;
                service.FkBusinessId = businessId;
                service.ServiceName = serviceDTO.ServiceName;
                service.Description = serviceDTO.Description;
                service.FkDiscountId = serviceDTO.FkDiscountId;
                service.BasePrice = serviceDTO.BasePrice;
                service.FkDiscount = discount;
                service.FkCategoryId = serviceDTO.FkCategoryId;

                try
                {
                    _obrnContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        public string Delete(int serviceId)
        {
            try
            {
                var service = GetServiceById(serviceId);
                if (service == null)
                {
                    return "Service does not exist";
                }
                _obrnContext.Services.Remove(service);
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
