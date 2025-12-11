namespace Simpl.Mobile.Mechanic.Core;

public sealed record BusinessResponse<T>(T? Data, string[] Errors);


public static class SuccessResponse
{
    public static BusinessResponse<T> Create<T>(T data)
    {
        return new BusinessResponse<T>(data, Array.Empty<string>());
    }
}

public static class ErrorResponse
{
    public static BusinessResponse<T> Create<T>(params string[] errors)
    {
        return new BusinessResponse<T>(default, errors);
    }
}