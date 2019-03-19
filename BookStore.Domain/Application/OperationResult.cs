using System;

namespace BookStore.Application
{
    public class OperationResult
    {
        public bool Success { get; set; }

        public Exception Exception { get; set; }

        public string ErrorMessage { get; set; }

        public OperationResult(bool success = true, string errorMessage = null, Exception exception = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public OperationResult(Exception exception) : this(false, exception.Message, exception)
        {
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Result { get; set; }

        public OperationResult(T result)
        {
            Result = result;
        }

        public OperationResult(bool success, string errorMessage, Exception exception = null) : base(success, errorMessage, exception)
        {
        }

        public OperationResult(Exception exception) : base(exception)
        {
            Result = default(T);
        }
    }
}
