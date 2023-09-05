namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using NetEvolve.Extensions.XUnit;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsTaskIntTests
{
    [Fact]
    public async Task WithTimeoutAsync_ParamTaskNull_ArgumentNullException()
    {
        Task task = null!;

        _ = await Assert
            .ThrowsAsync<ArgumentNullException>(
                "task",
                async () => await task!.WithTimeoutAsync(100).ConfigureAwait(false)
            )
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeoutInMilliseconds = 75;

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeoutInMilliseconds)
            .ConfigureAwait(false);
        Assert.True(isValid);

        static async Task TestMethod()
        {
            await Task.Delay(20).ConfigureAwait(false);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeoutInMilliseconds = 20;

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeoutInMilliseconds)
            .ConfigureAwait(false);
        Assert.False(isValid);

        static async Task TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeoutInMilliseconds = 75;

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeoutInMilliseconds)
            .ConfigureAwait(false);
        Assert.True(isValid);

        static Task TestMethod() => Task.CompletedTask;
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeoutInMilliseconds = Timeout.Infinite;

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeoutInMilliseconds)
            .ConfigureAwait(false);
        Assert.True(isValid);

        static async Task TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeoutInMilliseconds = 0;

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeoutInMilliseconds)
            .ConfigureAwait(false);
        Assert.False(isValid);

        static async Task TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException()
    {
        var timeoutInMilliseconds = -2;

        _ = await Assert
            .ThrowsAsync<ArgumentOutOfRangeException>(
                "timeoutInMilliseconds",
                async () =>
                    await TestMethod().WithTimeoutAsync(timeoutInMilliseconds).ConfigureAwait(false)
            )
            .ConfigureAwait(false);

        static async Task TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return;
        }
    }
}
