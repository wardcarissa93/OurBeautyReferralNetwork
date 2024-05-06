using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        private readonly BusinessRepo _businessRepo; // Add BusinessRepo as a dependency


        public ServiceRepo(ApplicationDbContext context, obrnDbContext obrnContext, BusinessRepo businessRepo)
        {
            _context = context;
            _obrnContext = obrnContext;
            _businessRepo = businessRepo; // Inject BusinessRepo

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


        public IEnumerable<ServiceDTO> GetAllServicesOfBusiness(string businessId)
        {
            var services = _obrnContext.Services
                .Where(s => s.FkBusinessId == businessId)
                .ToList();

            // Convert the LINQ query to a list to materialize the results
            var extendedServices = services.Select(service =>
            {
                decimal? discountPercentage = GetServiceDiscount(service.PkServiceId);
                Console.WriteLine(discountPercentage);
                decimal actualDiscount = discountPercentage ?? 0;
                Console.WriteLine(actualDiscount);
                DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
                Discount discount = discountRepo.GetDiscountById(service.FkDiscountId);
                Console.WriteLine(discount.ToString());

                // Log before extending the service
                Console.WriteLine($"Extending service ID: {service.PkServiceId} with discount ID: {service.FkDiscountId}");

                // Call ExtendService and log result
                var extendedService = service.ExtendService(discount);
                Console.WriteLine($"Extended service created for service ID: {service.PkServiceId}");

                return extendedService;
            }).ToList(); // Materialize the query to a list

            return extendedServices; // Now you can return the list of extended services
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

        public Service CreateServiceForBusiness(ServiceCreateDTO servicecreateDTO, Discount discount)
        {
            var service = new Service
            {
                PkServiceId = servicecreateDTO.PkServiceId,
                Image = servicecreateDTO.Image,
                FkBusinessId = servicecreateDTO.FkBusinessId,
                ServiceName = servicecreateDTO.ServiceName,
                Description = servicecreateDTO.Description,
                FkDiscountId = servicecreateDTO.FkDiscountId,
                BasePrice = servicecreateDTO.BasePrice,
                FkDiscount = discount,
                FkCategoryId = servicecreateDTO.FkCategoryId,
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

        public IActionResult GetBusinessObject(ServiceDTO serviceDTO)
        {
            var business = _businessRepo.GetBusinessById(serviceDTO.FkBusinessId);
            // Check if businessResult is an OkObjectResult
            if (business != null)
            {
                return new OkObjectResult(business);
            }
            return new NotFoundObjectResult("Business not found");
        }
        public bool EditServiceForBusinessAsync(ServiceDTO serviceDTO, int serviceId)
        {
            DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
            Discount discount = discountRepo.GetDiscountById(serviceDTO.FkDiscountId);
            CategoryRepo categoryRepo = new CategoryRepo(_context, _obrnContext);
            Category category = categoryRepo.GetCategoryById(serviceDTO.FkCategoryId);
            Service service = GetServiceById(serviceId);
            if (service != null)
            {
                service.Image = serviceDTO.Image;
                service.FkBusinessId = serviceDTO.FkBusinessId;
                service.ServiceName = serviceDTO.ServiceName;
                service.Description = serviceDTO.Description;
                service.FkDiscountId = serviceDTO.FkDiscountId;
                service.BasePrice = serviceDTO.BasePrice;
                service.FkDiscount = discount;
                service.FkCategoryId = serviceDTO.FkCategoryId;
                service.FkCategory = category;
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