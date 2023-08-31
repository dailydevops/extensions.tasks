﻿namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using NetEvolve.Extensions.XUnit;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsTaskTimespanTests
{
    [Fact]
    public async Task WithTimeoutAsync_ParamTaskNull_ArgumentNullException()
    {
        Task task = null!;

        _ = await Assert
            .ThrowsAsync<ArgumentNullException>(
                "task",
                async () => await task!.WithTimeoutAsync(TimeSpan.FromMilliseconds(100)).ConfigureAwait(false)
            )
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(75);

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeout)
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
        var timeout = TimeSpan.FromMilliseconds(20);

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeout)
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
        var timeout = TimeSpan.FromMilliseconds(100);

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeout)
            .ConfigureAwait(false);
        Assert.True(isValid);

        static Task TestMethod() => Task.CompletedTask;
    }

    [Fact]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeout = Timeout.InfiniteTimeSpan;

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeout)
            .ConfigureAwait(false);
        Assert.True(isValid);

        static async Task TestMethod()
        {
            await Task.Delay(75).ConfigureAwait(false);
            return;
        }
    }
}
