namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Extensions.XUnit;
using Xunit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsValueTaskIntTests
{
    [Fact]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeoutInMilliseconds = 75;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.True(isValid);

        static async ValueTask TestMethod()
        {
            await Task.Delay(20);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeoutInMilliseconds = 20;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.False(isValid);

        static async ValueTask TestMethod()
        {
            await Task.Delay(75);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeoutInMilliseconds = 75;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.True(isValid);

        static ValueTask TestMethod() => ValueTask.CompletedTask;
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeoutInMilliseconds = Timeout.Infinite;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.True(isValid);

        static async ValueTask TestMethod()
        {
            await Task.Delay(75);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeoutInMilliseconds = 0;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.False(isValid);

        static async ValueTask TestMethod()
        {
            await Task.Delay(75);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException()
    {
        var timeoutInMilliseconds = -2;

        var testCode = async () => await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            "timeoutInMilliseconds",
            testCode
        );

        static async ValueTask TestMethod()
        {
            await Task.Delay(75);
            return;
        }
    }
}
