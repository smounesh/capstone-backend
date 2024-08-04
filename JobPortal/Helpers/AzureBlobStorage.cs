using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JobPortal.Helpers
{
    public class AzureBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly ILogger<AzureBlobStorage> _logger;

        public AzureBlobStorage(IConfiguration configuration, ILogger<AzureBlobStorage> logger)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("ConnectionString is null or empty");
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("Connection String: {ConnectionString}", connectionString);

            // Create a BlobServiceClient using the connection string
            _blobServiceClient = new BlobServiceClient(new Uri(connectionString));
            _containerName = configuration["AzureBlobStorage:ContainerName"] ?? throw new ArgumentNullException("ContainerName is null or empty"); // Set your container name from configuration
        }

        public async Task<(string Uri, string FileName)> UploadResumeAsync(Stream fileStream, string originalFileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                _logger.LogInformation("Creating container client for {ContainerName}", _containerName);
                await containerClient.CreateIfNotExistsAsync();

                string uuidFileName = GenerateUUIDFileName(originalFileName);
                _logger.LogInformation("Generated UUID file name: {UUIDFileName}", uuidFileName);

                var blobClient = containerClient.GetBlobClient(uuidFileName);
                _logger.LogInformation("Uploading file to blob client: {BlobUri}", blobClient.Uri);
                await blobClient.UploadAsync(fileStream, true);

                return (blobClient.Uri.ToString(), uuidFileName);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Error uploading file to Azure Blob Storage. Status: {Status}, ErrorCode: {ErrorCode}", ex.Status, ex.ErrorCode);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while uploading the file.");
                throw;
            }
        }


        public async Task<Stream> GetFileAsync(string fileName)
        {
            try
            {
                _logger.LogInformation("Getting file: {FileName} from container: {ContainerName}", fileName, _containerName);
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                var downloadInfo = await blobClient.DownloadAsync();
                _logger.LogInformation("File: {FileName} retrieved successfully", fileName);
                return downloadInfo.Value.Content;
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Error retrieving file from Azure Blob Storage. Status: {Status}, ErrorCode: {ErrorCode}", ex.Status, ex.ErrorCode);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving the file.");
                throw;
            }
        }

        public async Task UpdateFileAsync(string fileName, Stream fileStream)
        {
            try
            {
                _logger.LogInformation("Updating file: {FileName} in container: {ContainerName}", fileName, _containerName);
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(fileStream, true); // Overwrite if exists
                _logger.LogInformation("File: {FileName} updated successfully", fileName);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Error updating file in Azure Blob Storage. Status: {Status}, ErrorCode: {ErrorCode}", ex.Status, ex.ErrorCode);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the file.");
                throw;
            }
        }

        public async Task DeleteResumeAsync(string fileName)
        {
            try
            {
                _logger.LogInformation("Deleting file: {FileName} from container: {ContainerName}", fileName, _containerName);
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.DeleteIfExistsAsync(); // Delete if exists
                _logger.LogInformation("File: {FileName} deleted successfully", fileName);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Error deleting file from Azure Blob Storage. Status: {Status}, ErrorCode: {ErrorCode}", ex.Status, ex.ErrorCode);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the file.");
                throw;
            }
        }

        private string GenerateUUIDFileName(string originalFileName)
        {
            // Generate a new UUID for the file name
            string uuid = Guid.NewGuid().ToString();

            // Extract the file extension from the original file name
            string fileExtension = Path.GetExtension(originalFileName);

            // Combine the UUID and file extension to create the new file name
            return $"{uuid}{fileExtension}"; // Return the new file name with extension
        }
    }
}
