namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using NetEvolve.Extensions.XUnit;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsTaskOfTTimespanTests
{
    [Fact]
    public async Task WithTimeoutAsync_ParamTaskNull_ArgumentNullException()
    {
        Task<bool> task = null!;

        _ = await Assert
            .ThrowsAsync<ArgumentNullException>(
                "task",
                async () =>
                    await task!
                        .WithTimeoutAsync(TimeSpan.FromMilliseconds(100))
                        .ConfigureAwait(false)
            )
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(75);

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false);
        Assert.True(isValid);
        Assert.Equal(1, result);

        static async Task<int> TestMethod()
        {
            await Task.Delay(20).ConfigureAwait(false);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(20);

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false);
        Assert.False(isValid);
        Assert.Equal(1, result);

        static async Task<int> TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(20);

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false);
        Assert.True(isValid);
        Assert.Equal(1, result);

        static Task<int> TestMethod() => Task.FromResult(1);
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeout = Timeout.InfiniteTimeSpan;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false);
        Assert.True(isValid);
        Assert.Equal(1, result);

        static async Task<int> TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowsArgumentOutOfRangeException()
    {
        var timeout = new TimeSpan(0, 0, 0, 0, -2);

        _ = await Assert
            .ThrowsAsync<ArgumentOutOfRangeException>(
                "timeout",
                async () => await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false)
            )
            .ConfigureAwait(false);

        static async Task<int> TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return 1;
        }
    }
}
