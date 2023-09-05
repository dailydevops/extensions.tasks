namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using NetEvolve.Extensions.XUnit;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsValueTaskTimespanTests
{
    [Fact]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(75);

        var isValid = await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false);
        Assert.True(isValid);

        static async ValueTask TestMethod()
        {
            await Task.Delay(20).ConfigureAwait(false);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(20);

        var isValid = await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false);
        Assert.False(isValid);

        static async ValueTask TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(75);

        var isValid = await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false);
        Assert.True(isValid);

        static ValueTask TestMethod() => ValueTask.CompletedTask;
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeout = Timeout.InfiniteTimeSpan;

        var isValid = await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false);
        Assert.True(isValid);

        static async ValueTask TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeout = TimeSpan.Zero;

        var isValid = await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false);
        Assert.False(isValid);

        static async ValueTask TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException()
    {
        var timeout = new TimeSpan(0, 0, 0, 0, -2);

        _ = await Assert
            .ThrowsAsync<ArgumentOutOfRangeException>(
                "timeout",
                async () => await TestMethod().WithTimeoutAsync(timeout).ConfigureAwait(false)
            )
            .ConfigureAwait(false);

        static async ValueTask TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return;
        }
    }
}
