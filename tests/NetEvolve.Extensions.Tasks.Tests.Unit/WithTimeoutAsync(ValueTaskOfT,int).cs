﻿namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Extensions.XUnit;
using Xunit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsValueTaskOfTIntTests
{
    [Fact]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeoutInMilliseconds = 150;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.True(isValid);
        Assert.Equal(1, result);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(20);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeoutInMilliseconds = 20;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.False(isValid);
        Assert.Equal(1, result);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(150);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeoutInMilliseconds = 20;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.True(isValid);
        Assert.Equal(1, result);

        static ValueTask<int> TestMethod() => ValueTask.FromResult(1);
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeoutInMilliseconds = Timeout.Infinite;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.True(isValid);
        Assert.Equal(1, result);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(150);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeoutInMilliseconds = 0;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        Assert.False(isValid);
        Assert.Equal(1, result);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(150);
            return 1;
        }
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException()
    {
        var timeoutInMilliseconds = -2;

        var testCode = async () => await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("timeoutInMilliseconds", testCode);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(150);
            return 1;
        }
    }
}
