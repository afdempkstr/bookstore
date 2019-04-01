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
        private static string _storageAccount = CloudConfigurationManager.GetSetting("StorageAccountName");
        private static string _storageKey = CloudConfigurationManager.GetSetting("StorageAccountKey");
        private static string _storageContainer = CloudConfigurationManager.GetSetting("StorageContainerName");

        private CloudBlobContainer _container;

        private static CloudStorageAccount StorageAccount
        {
            get
            {
                var connectionString = $"DefaultEndpointsProtocol=https;AccountName={_storageAccount};AccountKey={_storageKey}";
                return CloudStorageAccount.Parse(connectionString);
            }
        }

        public AzureStorageHelper()
        {
            var storageAccount = StorageAccount;
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(_storageContainer);
        }

        public OperationResult<string> UploadImage(string filename, string contentType, Stream imageFileStream)
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