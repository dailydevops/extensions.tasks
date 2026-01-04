namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsValueTaskTimespanTests
{
    [Test]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(1000);

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsTrue();

        static async ValueTask TestMethod() => await Task.Delay(20);
    }

    [Test]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(20);

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsFalse();

        static async ValueTask TestMethod() => await Task.Delay(1000);
    }

    [Test]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(1000);

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsTrue();

        static ValueTask TestMethod() => ValueTask.CompletedTask;
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeout = Timeout.InfiniteTimeSpan;

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsTrue();

        static async ValueTask TestMethod() => await Task.Delay(1000);
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeout = TimeSpan.Zero;

        var isValid = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsFalse();

        static async ValueTask TestMethod() => await Task.Delay(1000);
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException()
    {
        var timeout = new TimeSpan(0, 0, 0, 0, -2);

        var testCode = async () => await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("timeout", testCode);

        static async ValueTask TestMethod() => await Task.Delay(1000);
    }
}
