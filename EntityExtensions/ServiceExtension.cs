using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;

namespace OurBeautyReferralNetwork.EntityExtensions
{
    public static class ServiceExtension
    {
        public static ServiceDTO ExtendService(this Models.Service service, Discount discount)
        {
            return new ServiceDTO
            {
                PkServiceId = service.PkServiceId,
                ServiceName = service.ServiceName,
                Image = service.Image,
                FkBusinessId = service.FkBusinessId,
                FkDiscountId = service.FkDiscountId,
                Description = service.Description,
                FkCategoryId = service.FkCategoryId,
                BasePrice = service.BasePrice,
                FkDiscount = discount
            };
        }
    }
}

