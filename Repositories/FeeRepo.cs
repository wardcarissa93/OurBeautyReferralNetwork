using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;
using System.Security.AccessControl;

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

        public bool CreateFee(FeeAndCommission fee)
        {
            bool isSuccess = true;
            try
            {
                _obrnContext.FeeAndCommissions.Add(new FeeAndCommission
                {
                    PkFeeId = fee.PkFeeId,
                    Amount = fee.Amount,
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Frequency = fee.Frequency,
                });
                _obrnContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Update(FeeAndCommission fee)
        {
            var editedFee = GetFeeById(fee.PkFeeId);
            if (editedFee != null)
            {
                editedFee.Amount = fee.Amount;
                editedFee.Description = fee.Description;
                editedFee.Frequency = fee.Frequency;
                editedFee.FeeType = fee.FeeType;

                try
                {
                    _obrnContext.SaveChanges();
                    return true; // Update successful
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    return false; // Update failed due to exception
                }
            }
            else
            {
                return false; // Fee not found or invalid ID
            }
        }


        public string Delete(string feeId)
        {
            try
            {
                var fee = GetFeeById(feeId);
                if (fee == null)
                {
                    return "Fee does not exist";
                }

                _obrnContext.FeeAndCommissions.Remove(fee);
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
