namespace BlockChain.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public ErrorType Error { get; private set; }

    private readonly T _value;

    private Result(bool isSuccess, T value, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        _value = value;
        Error = errorType;
    }

    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException($"Cannot access the value because the operation failed. Error: {Error}.");
            }

            return _value;
        }
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(isSuccess: true, value, ErrorType.None);
    }

    public static Result<T> Failure(ErrorType errorType)
    {
        return new Result<T>(isSuccess: false, default, errorType);
    }

    public static implicit operator Result<T>(T value)
    {
        return Result<T>.Success(value);
    }

    public static implicit operator Result<T>(ErrorType errorType)
    {
        return Result<T>.Failure(errorType);
    }
}