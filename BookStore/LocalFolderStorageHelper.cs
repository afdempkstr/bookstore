using BookStore.Application;
using BookStore.Domain.Application;
using System;
using System.IO;

namespace BookStore
{
    public class LocalFolderStorageHelper : IStorageHelper
    {
        public OperationResult<string> UploadImage(string filename, string contentType, Stream imageFileStream, string folder = null)
        {
            try
            {
                string path = Path.Combine(folder ?? "", filename);
                SaveStream(imageFileStream, path);
                return new OperationResult<string>(filename);
            }
            catch (Exception e)
            {
                return new OperationResult<string>(e);
            }
        }

        private void SaveStream(Stream stream, string destination)
        {
            using (var fileStream = new FileStream(destination, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}