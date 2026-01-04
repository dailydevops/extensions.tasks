namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsValueTaskIntTests
{
    [Test]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeoutInMilliseconds = 1000;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsTrue();

        static async ValueTask TestMethod() => await Task.Delay(20);
    }

    [Test]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeoutInMilliseconds = 20;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsFalse();

        static async ValueTask TestMethod() => await Task.Delay(120);
    }

    [Test]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeoutInMilliseconds = 1000;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsTrue();

        static ValueTask TestMethod() => ValueTask.CompletedTask;
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeoutInMilliseconds = Timeout.Infinite;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsTrue();

        static async ValueTask TestMethod() => await Task.Delay(1000);
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeoutInMilliseconds = 0;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsFalse();

        static async ValueTask TestMethod() => await Task.Delay(1000);
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException()
    {
        var timeoutInMilliseconds = -2;

        var testCode = async () => await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("timeoutInMilliseconds", testCode);

        static async ValueTask TestMethod() => await Task.Delay(1000);
    }
}
