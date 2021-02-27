namespace BDC.Core
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }


        public static Result Success(string msg)
        {
            return new Result { IsSuccess = true, Message = msg };
        }

        public static Result Failure(string msg)
        {
            return new Result { Message = msg };
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static new Result<T> Success(string msg)
        {
            return new Result<T> { IsSuccess = true, Message = msg };
        }

        public static Result<T> Success(T data, string msg)
        {
            return new Result<T> { Data = data, IsSuccess = true, Message = msg };
        }

        public static new Result<T> Failure(string msg)
        {
            return new Result<T> { Message = msg };
        }

        public static Result<T> Failure(T data, string msg)
        {
            return new Result<T> { Data = data, Message = msg };
        }
    }
}
