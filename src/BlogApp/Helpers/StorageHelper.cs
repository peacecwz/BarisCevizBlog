using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp
{
    public class StorageHelper
    {
        #region Constructor
        private static StorageHelper _instance;

        public static StorageHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StorageHelper();
                return _instance;
            }
        }

        #endregion

        CloudStorageAccount storageAccount;
        CloudBlobClient blobClient;
        CloudBlobContainer container;
        public StorageHelper()
        {
            storageAccount = CloudStorageAccount.Parse(Startup.WebConfiguration.GetValue<string>("StorageConStr"));
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("blog");
        }

        public async Task<string> UploadFile(Stream stream, string friendlyUrl)
        {
            if (await container.CreateIfNotExistsAsync())
                await container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        });
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(friendlyUrl + "-" + Guid.NewGuid().ToString().Split('-')[0] + ".jpg");

            try
            {
                await blockBlob.UploadFromStreamAsync(stream);
                return blockBlob.Uri.ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}
