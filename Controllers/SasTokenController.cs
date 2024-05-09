using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;

namespace OurBeautyReferralNetwork.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SasTokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SasTokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetSasToken()
        {
            try
            {
                // Retrieve Azure Blob Storage settings from app settings
                string connectionString = _configuration["ConnectionStrings:AzureBlobStorage"];
                string containerName = _configuration["AzureBlobStorage:ContainerName"];
                int sasTokenExpirationHours = _configuration.GetValue<int>("AzureBlobStorage:SasTokenExpirationHours");

                // Create storage account instance
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

                // Create a CloudBlobClient to interact with Blob service
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Get reference to the container
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Set permissions and expiration for the SAS token
                SharedAccessBlobPolicy sasPolicy = new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List, // Adjust permissions as needed
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(sasTokenExpirationHours) // Set expiration time based on configuration
                };

                // Generate SAS token for the container
                string sasToken = container.GetSharedAccessSignature(sasPolicy);

                // Return the SAS token as JSON
                return Ok(new { SasToken = sasToken });
            }
            catch (Exception ex)
            {
                // Handle error and return appropriate response
                return StatusCode(500, new { Error = "Failed to generate SAS token.", Message = ex.Message });
            }
        }
    }
}
