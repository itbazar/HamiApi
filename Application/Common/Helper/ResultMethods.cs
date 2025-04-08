namespace Application.Common.Helper;

public static class ResultMethods
{
    public static Result<TValue> GetResult<TValue>(
    TValue value,
    Success success)
    {
        return new Result<TValue>()
            .WithValue(value)
            .WithSuccess(success);
    }
}
