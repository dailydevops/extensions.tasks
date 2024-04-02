namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Extensions.XUnit;
using Xunit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsTaskOfTTimespanTests
{
    [Fact]
    public async Task WithTimeoutAsync_ParamTaskNull_ArgumentNullException()
    {
        Task<bool> task = null!;

        var testCode = async () => await task!.WithTimeoutAsync(TimeSpan.FromMilliseconds(100));
        _ = await Assert.ThrowsAsync<ArgumentNullException>("task", testCode);
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(150);

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        Assert.True(isValid);
        Assert.Equal(1, result);

        static async Task<int> TestMethod()
        {
            await Task.Delay(20);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(20);

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        Assert.False(isValid);
        Assert.Equal(1, result);

        static async Task<int> TestMethod()
        {
            await Task.Delay(150);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(20);

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        Assert.True(isValid);
        Assert.Equal(1, result);

        static Task<int> TestMethod() => Task.FromResult(1);
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeout = Timeout.InfiniteTimeSpan;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        Assert.True(isValid);
        Assert.Equal(1, result);

        static async Task<int> TestMethod()
        {
            await Task.Delay(150);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeout = TimeSpan.Zero;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        Assert.False(isValid);
        Assert.Equal(1, result);

        static async Task<int> TestMethod()
        {
            await Task.Delay(150);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowsArgumentOutOfRangeException()
    {
        var timeout = new TimeSpan(0, 0, 0, 0, -2);

        var testCode = async () => await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("timeout", testCode);

        static async Task<int> TestMethod()
        {
            await Task.Delay(150);
            return 1;
        }
    }
}
