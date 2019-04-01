using BookStore.Application;
using System.IO;

namespace BookStore.Domain.Application
{
    public interface IStorageHelper
    {
        OperationResult<string> UploadImage(string filename, string contentType, Stream imageFileStream);
    }
}
