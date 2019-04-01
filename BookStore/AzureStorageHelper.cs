using System;
using BookStore.Application;
using BookStore.Domain.Application;
using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BookStore
{
    public class AzureStorageHelper : IStorageHelper
    {
        private static readonly string _storageAccount;
        private static readonly string _storageKey;
        private static readonly string _storageContainer;
        private static readonly CloudStorageAccount _cloudStorageAccount;
        
        private CloudBlobContainer _container;

        static AzureStorageHelper()
        {
            _storageAccount = CloudConfigurationManager.GetSetting("StorageAccountName");
            _storageKey = CloudConfigurationManager.GetSetting("StorageAccountKey");
            _storageContainer = CloudConfigurationManager.GetSetting("StorageContainerName");
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={_storageAccount};AccountKey={_storageKey}";
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
        }
        
        public AzureStorageHelper()
        {
            var blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(_storageContainer);
        }

        public OperationResult<string> UploadImage(string filename, string contentType, Stream imageFileStream, string folder = null)
        {
            try
            {
                var blob = _container.GetBlockBlobReference(filename);
                blob.Properties.ContentType = contentType;
                blob.UploadFromStream(imageFileStream);
                var result = blob.Uri.ToString();
                return new OperationResult<string>(result);
            }
            catch (Exception e)
            {
                return new OperationResult<string>(false, "Could not upload image", e);
            }
        }
    }
}