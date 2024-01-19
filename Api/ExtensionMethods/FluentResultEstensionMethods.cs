using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.ExtensionMethods;

public static class FluentResultEstensionMethods
{
    public static ActionResult Match<TValue>(
        this Result<TValue> result,
        Func<TValue, ActionResult> success,
        Func<ActionResult> failure)
    {
        if (result.IsSuccess)
            return success(result.Value);
        else
            return failure();
    }

    public static ActionResult Match<TValue>(
        this Result<TValue> result,
        Func<TValue, ActionResult> success,
        Func<Result, ActionResult> failure)
    {
        if (result.IsSuccess)
            return success(result.Value);
        else
            return failure(result.ToResult());
    }
}
