#pragma warning disable CA2012 // Use ValueTasks correctly - intentional for testing WhenAll behavior

namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class ValueTaskExtensionsWhenAllTests
{
    [Test]
    public async Task WhenAll_WithParamsArray_AllTasksComplete()
    {
        var results = new List<int>();

        await ValueTask.WhenAll(AddValueAsync(results, 1), AddValueAsync(results, 2), AddValueAsync(results, 3));

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results).Contains(1);
        _ = await Assert.That(results).Contains(2);
        _ = await Assert.That(results).Contains(3);
    }

    [Test]
    public async Task WhenAll_WithEmptyArray_CompletesImmediately() =>
        await ValueTask.WhenAll(Array.Empty<ValueTask>());

    [Test]
    public async Task WhenAll_WithNullArray_ThrowsArgumentNullException()
    {
        ValueTask[]? tasks = null;

        var testCode = async () => await ValueTask.WhenAll(tasks!);

        _ = await Assert.ThrowsAsync<ArgumentNullException>("tasks", testCode);
    }

    [Test]
    public async Task WhenAll_WithIEnumerable_AllTasksComplete()
    {
        var results = new List<int>();
        IEnumerable<ValueTask> tasks =
        [
            AddValueAsync(results, 1),
            AddValueAsync(results, 2),
            AddValueAsync(results, 3),
        ];

        await ValueTask.WhenAll(tasks);

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results).Contains(1);
        _ = await Assert.That(results).Contains(2);
        _ = await Assert.That(results).Contains(3);
    }

    [Test]
    public async Task WhenAll_WithEmptyIEnumerable_CompletesImmediately()
    {
        IEnumerable<ValueTask> tasks = [];

        await ValueTask.WhenAll(tasks);
    }

    [Test]
    public async Task WhenAll_WithNullIEnumerable_ThrowsArgumentNullException()
    {
        IEnumerable<ValueTask>? tasks = null;

        var testCode = async () => await ValueTask.WhenAll(tasks!);

        _ = await Assert.ThrowsAsync<ArgumentNullException>("tasks", testCode);
    }

    [Test]
    public async Task WhenAll_WithAlreadyCompletedTasks_CompletesImmediately() =>
        await ValueTask.WhenAll(ValueTask.CompletedTask, ValueTask.CompletedTask, ValueTask.CompletedTask);

    [Test]
    public async Task WhenAll_WithDelayedTasks_WaitsForAllToComplete()
    {
        var completionOrder = new List<int>();

        await ValueTask.WhenAll(
            DelayAndAddAsync(completionOrder, 1, 30),
            DelayAndAddAsync(completionOrder, 2, 10),
            DelayAndAddAsync(completionOrder, 3, 20)
        );

        _ = await Assert.That(completionOrder).Count().IsEqualTo(3);
    }

#if NET9_0_OR_GREATER
    [Test]
    public async Task WhenAll_WithReadOnlySpan_AllTasksComplete()
    {
        var results = new List<int>();
        ReadOnlySpan<ValueTask> tasks =
        [
            AddValueAsync(results, 1),
            AddValueAsync(results, 2),
            AddValueAsync(results, 3),
        ];

        await ValueTask.WhenAll(tasks);

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results).Contains(1);
        _ = await Assert.That(results).Contains(2);
        _ = await Assert.That(results).Contains(3);
    }

    [Test]
    public async Task WhenAll_WithEmptyReadOnlySpan_CompletesImmediately()
    {
        ReadOnlySpan<ValueTask> tasks = [];

        await ValueTask.WhenAll(tasks);
    }
#endif

    private static async ValueTask AddValueAsync(List<int> list, int value)
    {
        await Task.Yield();
        lock (list)
        {
            list.Add(value);
        }
    }

    private static async ValueTask DelayAndAddAsync(List<int> list, int value, int delayMs)
    {
        await Task.Delay(delayMs);
        lock (list)
        {
            list.Add(value);
        }
    }
}
