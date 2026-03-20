using Domain.Common.Exceptions;

using FluentAssertions;

namespace Domain.Tests.Utils.Extensions;

public static class ActExtensions
{
    public static void ThrowsException<TEnum, T>(
        this Func<T> act,
        TEnum expectedError) where TEnum : Enum
    {
        var ex = act.Should()
            .Throw<SlaisException>()
            .WithMessage("*")
            .Which;

        ex.ErrorCode.Should().Be(Convert.ToInt32(expectedError));
    }

    public static void ThrowsException<TEnum>(
        this Action act,
        TEnum expectedError) where TEnum : Enum
    {
        var ex = act.Should()
            .Throw<SlaisException>()
            .WithMessage("*")
            .Which;

        ex.ErrorCode.Should().Be(Convert.ToInt32(expectedError));
    }
}
