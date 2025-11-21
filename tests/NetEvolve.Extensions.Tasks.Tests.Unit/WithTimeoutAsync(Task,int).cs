namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsTaskIntTests
{
    [Test]
    public async Task WithTimeoutAsync_ParamTaskNull_ArgumentNullException()
    {
        Task task = null!;

        var testCode = async () => await task!.WithTimeoutAsync(100);
        _ = await Assert.ThrowsAsync<ArgumentNullException>("task", testCode);
    }

    [Test]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeoutInMilliseconds = 150;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsTrue();

        static Task TestMethod() => Task.Delay(20);
    }

    [Test]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeoutInMilliseconds = 20;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsFalse();

        static Task TestMethod() => Task.Delay(150);
    }

    [Test]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeoutInMilliseconds = 150;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsTrue();

        static Task TestMethod() => Task.CompletedTask;
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeoutInMilliseconds = Timeout.Infinite;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsTrue();

        static Task TestMethod() => Task.Delay(150);
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeoutInMilliseconds = 0;

        var isValid = await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.That(isValid).IsFalse();

        static Task TestMethod() => Task.Delay(150);
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException()
    {
        var timeoutInMilliseconds = -2;

        var testCode = async () => await TestMethod().WithTimeoutAsync(timeoutInMilliseconds);
        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("timeoutInMilliseconds", testCode);

        static Task TestMethod() => Task.Delay(150);
    }
}
