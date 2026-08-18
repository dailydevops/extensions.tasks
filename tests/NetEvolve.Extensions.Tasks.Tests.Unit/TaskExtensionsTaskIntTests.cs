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

        var testCode = async () => await task.WithTimeoutAsync(100).ConfigureAwait(false);
        _ = await Assert.ThrowsAsync<ArgumentNullException>("task", testCode);
    }

    [Test]
    public async Task WithTimeoutAsync_IsValidTrue_Expected(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var timeoutInMilliseconds = 1000;

        var isValid = await TestMethod(cancellationToken)
            .WithTimeoutAsync(timeoutInMilliseconds, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        _ = await Assert.That(isValid).IsTrue();

        static Task TestMethod(CancellationToken token = default) => Task.Delay(20, cancellationToken: token);
    }

    [Test]
    public async Task WithTimeoutAsync_IsValidFalse_Expected(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var timeoutInMilliseconds = 20;

        var isValid = await TestMethod(cancellationToken)
            .WithTimeoutAsync(timeoutInMilliseconds, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        _ = await Assert.That(isValid).IsFalse();

        static Task TestMethod(CancellationToken token = default) => Task.Delay(1000, cancellationToken: token);
    }

    [Test]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var timeoutInMilliseconds = 1000;

        var isValid = await TestMethod()
            .WithTimeoutAsync(timeoutInMilliseconds, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        _ = await Assert.That(isValid).IsTrue();

        static Task TestMethod() => Task.CompletedTask;
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var timeoutInMilliseconds = Timeout.Infinite;

        var isValid = await TestMethod(cancellationToken)
            .WithTimeoutAsync(timeoutInMilliseconds, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        _ = await Assert.That(isValid).IsTrue();

        static Task TestMethod(CancellationToken token = default) => Task.Delay(1000, cancellationToken: token);
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutZero_Expected(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var timeoutInMilliseconds = 0;

        var isValid = await TestMethod(cancellationToken)
            .WithTimeoutAsync(timeoutInMilliseconds, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        _ = await Assert.That(isValid).IsFalse();

        static Task TestMethod(CancellationToken token = default) => Task.Delay(1000, cancellationToken: token);
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var timeoutInMilliseconds = -2;

        var testCode = async () =>
            await TestMethod(cancellationToken)
                .WithTimeoutAsync(timeoutInMilliseconds, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("timeoutInMilliseconds", testCode);

        static Task TestMethod(CancellationToken token = default) => Task.Delay(1000, cancellationToken: token);
    }
}
