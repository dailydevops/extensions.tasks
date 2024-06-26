﻿namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Extensions.XUnit;
using Xunit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsValueTaskTimespanTests
{
    [Fact]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(150);

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        Assert.True(isValid);

        static async ValueTask TestMethod() => await Task.Delay(20);
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(20);

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        Assert.False(isValid);

        static async ValueTask TestMethod() => await Task.Delay(150);
    }

    [Fact]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(150);

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        Assert.True(isValid);

        static ValueTask TestMethod() => ValueTask.CompletedTask;
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeout = Timeout.InfiniteTimeSpan;

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        Assert.True(isValid);

        static async ValueTask TestMethod() => await Task.Delay(150);
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeout = TimeSpan.Zero;

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        Assert.False(isValid);

        static async ValueTask TestMethod() => await Task.Delay(150);
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException()
    {
        var timeout = new TimeSpan(0, 0, 0, 0, -2);

        var testCode = async () => await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("timeout", testCode);

        static async ValueTask TestMethod() => await Task.Delay(150);
    }
}
