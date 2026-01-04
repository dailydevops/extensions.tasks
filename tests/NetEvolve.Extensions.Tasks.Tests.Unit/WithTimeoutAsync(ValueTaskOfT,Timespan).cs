namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class TaskExtensionsValueTaskOfTTimespanTests
{
    [Test]
    public async Task WithTimeoutAsync_IsValidTrue_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(1000);

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsTrue();
        _ = await Assert.That(result).IsEqualTo(1);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(20);
            return 1;
        }
    }

    [Test]
    public async Task WithTimeoutAsync_IsValidFalse_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(20);

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsFalse();
        _ = await Assert.That(result).IsEqualTo(1);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(1000);
            return 1;
        }
    }

    [Test]
    public async Task WithTimeoutAsync_TaskAlreadyCompleted_Expected()
    {
        var timeout = TimeSpan.FromMilliseconds(20);

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsTrue();
        _ = await Assert.That(result).IsEqualTo(1);

        static ValueTask<int> TestMethod() => ValueTask.FromResult(1);
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutInfinite_Expected()
    {
        var timeout = Timeout.InfiniteTimeSpan;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsTrue();
        _ = await Assert.That(result).IsEqualTo(1);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(1000);
            return 1;
        }
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutZero_Expected()
    {
        var timeout = TimeSpan.Zero;

        var (isValid, result) = await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.That(isValid).IsFalse();
        _ = await Assert.That(result).IsEqualTo(1);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(1000);
            return 1;
        }
    }

    [Test]
    public async Task WithTimeoutAsync_TimeoutMinusTwo_ThrowArgumentOutOfRangeException()
    {
        var timeout = new TimeSpan(0, 0, 0, 0, -2);

        var testCode = async () => await TestMethod().WithTimeoutAsync(timeout);
        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>("timeout", testCode);

        static async ValueTask<int> TestMethod()
        {
            await Task.Delay(1000);
            return 1;
        }
    }
}
