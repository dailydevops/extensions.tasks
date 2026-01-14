#pragma warning disable CA2012 // Use ValueTasks correctly - intentional for testing WhenAll behavior

namespace NetEvolve.Extensions.Tasks.Tests.Unit;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit;

[UnitTest]
[ExcludeFromCodeCoverage]
public class ValueTaskExtensionsWhenAllOfTTests
{
    [Test]
    public async Task WhenAll_WithParamsArray_ReturnsAllResults()
    {
        var results = await ValueTask<int>.WhenAll(GetValueAsync(1), GetValueAsync(2), GetValueAsync(3));

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results[0]).IsEqualTo(1);
        _ = await Assert.That(results[1]).IsEqualTo(2);
        _ = await Assert.That(results[2]).IsEqualTo(3);
    }

    [Test]
    public async Task WhenAll_WithEmptyArray_ReturnsEmptyArray()
    {
        var results = await ValueTask<int>.WhenAll(Array.Empty<ValueTask<int>>());

        _ = await Assert.That(results).IsEmpty();
    }

    [Test]
    public async Task WhenAll_WithNullArray_ThrowsArgumentNullException()
    {
        ValueTask<int>[]? tasks = null;

        var testCode = async () => await ValueTask<int>.WhenAll(tasks!);

        _ = await Assert.ThrowsAsync<ArgumentNullException>("tasks", testCode);
    }

    [Test]
    public async Task WhenAll_WithIEnumerable_ReturnsAllResults()
    {
        IEnumerable<ValueTask<int>> tasks = [GetValueAsync(1), GetValueAsync(2), GetValueAsync(3)];

        var results = await ValueTask<int>.WhenAll(tasks);

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results[0]).IsEqualTo(1);
        _ = await Assert.That(results[1]).IsEqualTo(2);
        _ = await Assert.That(results[2]).IsEqualTo(3);
    }

    [Test]
    public async Task WhenAll_WithEmptyIEnumerable_ReturnsEmptyArray()
    {
        IEnumerable<ValueTask<int>> tasks = [];

        var results = await ValueTask<int>.WhenAll(tasks);

        _ = await Assert.That(results).IsEmpty();
    }

    [Test]
    public async Task WhenAll_WithNullIEnumerable_ThrowsArgumentNullException()
    {
        IEnumerable<ValueTask<int>>? tasks = null;

        var testCode = async () => await ValueTask<int>.WhenAll(tasks!);

        _ = await Assert.ThrowsAsync<ArgumentNullException>("tasks", testCode);
    }

    [Test]
    public async Task WhenAll_WithAlreadyCompletedTasks_ReturnsAllResults()
    {
        var results = await ValueTask<int>.WhenAll(new ValueTask<int>(1), new ValueTask<int>(2), new ValueTask<int>(3));

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results[0]).IsEqualTo(1);
        _ = await Assert.That(results[1]).IsEqualTo(2);
        _ = await Assert.That(results[2]).IsEqualTo(3);
    }

    [Test]
    public async Task WhenAll_WithDelayedTasks_ReturnsResultsInOrder()
    {
        var results = await ValueTask<int>.WhenAll(
            DelayAndReturnAsync(1, 30),
            DelayAndReturnAsync(2, 10),
            DelayAndReturnAsync(3, 20)
        );

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results[0]).IsEqualTo(1);
        _ = await Assert.That(results[1]).IsEqualTo(2);
        _ = await Assert.That(results[2]).IsEqualTo(3);
    }

    [Test]
    public async Task WhenAll_WithStringResults_ReturnsAllStrings()
    {
        var results = await ValueTask<string>.WhenAll(
            GetStringAsync("Hello"),
            GetStringAsync("World"),
            GetStringAsync("!")
        );

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results[0]).IsEqualTo("Hello");
        _ = await Assert.That(results[1]).IsEqualTo("World");
        _ = await Assert.That(results[2]).IsEqualTo("!");
    }

    [Test]
    public async Task WhenAll_WithListAsIEnumerable_ReturnsAllResults()
    {
        var tasks = new List<ValueTask<int>> { GetValueAsync(10), GetValueAsync(20), GetValueAsync(30) };

        var results = await ValueTask<int>.WhenAll(tasks);

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results[0]).IsEqualTo(10);
        _ = await Assert.That(results[1]).IsEqualTo(20);
        _ = await Assert.That(results[2]).IsEqualTo(30);
    }

#if NET9_0_OR_GREATER
    [Test]
    public async Task WhenAll_WithReadOnlySpan_ReturnsAllResults()
    {
        ReadOnlySpan<ValueTask<int>> tasks = [GetValueAsync(1), GetValueAsync(2), GetValueAsync(3)];

        var results = await ValueTask<int>.WhenAll(tasks);

        _ = await Assert.That(results).Count().IsEqualTo(3);
        _ = await Assert.That(results[0]).IsEqualTo(1);
        _ = await Assert.That(results[1]).IsEqualTo(2);
        _ = await Assert.That(results[2]).IsEqualTo(3);
    }

    [Test]
    public async Task WhenAll_WithEmptyReadOnlySpan_ReturnsEmptyArray()
    {
        ReadOnlySpan<ValueTask<int>> tasks = [];

        var results = await ValueTask<int>.WhenAll(tasks);

        _ = await Assert.That(results).IsEmpty();
    }
#endif

    private static async ValueTask<int> GetValueAsync(int value)
    {
        await Task.Yield();
        return value;
    }

    private static async ValueTask<int> DelayAndReturnAsync(int value, int delayMs)
    {
        await Task.Delay(delayMs);
        return value;
    }

    private static async ValueTask<string> GetStringAsync(string value)
    {
        await Task.Yield();
        return value;
    }
}
