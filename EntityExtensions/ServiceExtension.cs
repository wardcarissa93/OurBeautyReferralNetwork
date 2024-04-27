using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.EntityExtensions
{
    public static class ServiceExtension
    {
        public static ServiceDTO ExtendService(this Models.Service service, decimal discountPercentage)
        {
            return new ServiceDTO
            {
                PkServiceId = service.PkServiceId,
                ServiceName = service.ServiceName,
                Image = service.Image,
                FkBusinessId = service.FkBusinessId,
                FkCategoryId = service.FkCategoryId,
                BasePrice = service.BasePrice,
                FkDiscount = new Discount { Percentage = discountPercentage }
            };
        }
    }
}

