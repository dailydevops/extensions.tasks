# NetEvolve.Extensions.Tasks

[![NuGet](https://img.shields.io/nuget/v/NetEvolve.Extensions.Tasks.svg)](https://www.nuget.org/packages/NetEvolve.Extensions.Tasks/)
[![License](https://img.shields.io/github/license/dailydevops/extensions.tasks.svg)](LICENSE)

A lightweight library providing extension methods for `Task`, `Task<T>`, `ValueTask`, and `ValueTask<T>` to simplify asynchronous programming patterns in .NET applications.

Part of the [Daily DevOps & .NET - NetEvolve](https://daily-devops.net/) project, which aims to provide a set of useful libraries for .NET developers.

## 📦 Installation

```bash
dotnet add package NetEvolve.Extensions.Tasks
```

## 🎯 Features

- **WhenAll for ValueTask**: Aggregate multiple `ValueTask` or `ValueTask<T>` operations similar to `Task.WhenAll`
- **Timeout Support**: Execute async operations with configurable timeout handling
- **Multi-Framework Support**: Compatible with .NET Standard 2.0, .NET 6.0, .NET 7.0, .NET 8.0, .NET 9.0, and .NET 10.0
- **Lightweight**: Minimal dependencies, focused on essential functionality
- **Modern**: Supports both `Task` and `ValueTask` patterns

## 🚀 Usage

### WhenAll (ValueTask)

Aggregate multiple `ValueTask` operations, similar to `Task.WhenAll`. This is useful when you have multiple `ValueTask` instances that you want to await together.

#### ValueTask with params array

```csharp
using NetEvolve.Extensions.Tasks;

await ValueTask.WhenAll(
    DoSomethingAsync(),
    DoSomethingElseAsync(),
    DoAnotherThingAsync()
);

Console.WriteLine("All operations completed");
```

#### ValueTask with IEnumerable

```csharp
using NetEvolve.Extensions.Tasks;

var tasks = new List<ValueTask>
{
    ProcessItemAsync(1),
    ProcessItemAsync(2),
    ProcessItemAsync(3)
};

await ValueTask.WhenAll(tasks);
```

#### ValueTask\<T\> with results

```csharp
using NetEvolve.Extensions.Tasks;

var results = await ValueTask<int>.WhenAll(
    GetValueAsync(1),
    GetValueAsync(2),
    GetValueAsync(3)
);

// results is int[] { 1, 2, 3 } - results are returned in the same order as tasks
foreach (var result in results)
{
    Console.WriteLine(result);
}
```

#### ValueTask\<T\> with IEnumerable

```csharp
using NetEvolve.Extensions.Tasks;

IEnumerable<ValueTask<string>> tasks = GetTasksAsync();
string[] results = await ValueTask<string>.WhenAll(tasks);
```

### WithTimeoutAsync

Execute asynchronous operations with a timeout. Returns `true` if the operation completes within the specified timeout, `false` otherwise.

#### Task with Timeout (milliseconds)

```csharp
using NetEvolve.Extensions.Tasks;

var task = SomeAsyncOperation();
var completed = await task.WithTimeoutAsync(5000); // 5 seconds timeout

if (completed)
{
    Console.WriteLine("Operation completed successfully");
}
else
{
    Console.WriteLine("Operation timed out");
}
```

#### Task with Timeout (TimeSpan)

```csharp
using NetEvolve.Extensions.Tasks;

var task = SomeAsyncOperation();
var completed = await task.WithTimeoutAsync(TimeSpan.FromSeconds(5));

if (completed)
{
    Console.WriteLine("Operation completed successfully");
}
else
{
    Console.WriteLine("Operation timed out");
}
```

#### Task\<T\> with Timeout

```csharp
using NetEvolve.Extensions.Tasks;

var task = GetDataAsync();
var completed = await task.WithTimeoutAsync(3000);

if (completed)
{
    var result = await task; // Safe to await again, already completed
    Console.WriteLine($"Result: {result}");
}
else
{
    Console.WriteLine("Operation timed out");
}
```

#### ValueTask with Timeout

```csharp
using NetEvolve.Extensions.Tasks;

ValueTask operation = PerformAsyncOperation();
var completed = await operation.WithTimeoutAsync(TimeSpan.FromSeconds(10));

if (!completed)
{
    Console.WriteLine("Operation did not complete in time");
}
```

#### With CancellationToken

```csharp
using NetEvolve.Extensions.Tasks;

var cts = new CancellationTokenSource();
var task = LongRunningOperation();

try
{
    var completed = await task.WithTimeoutAsync(5000, cts.Token);
    if (!completed)
    {
        Console.WriteLine("Timeout occurred");
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled");
}
```

## 📚 API Reference

### Extension Methods

All extension methods are available for:
- `Task`
- `Task<T>`
- `ValueTask`
- `ValueTask<T>`

### WhenAll

Creates a task that completes when all of the supplied `ValueTask` instances have completed.

#### WhenAll(IEnumerable\<ValueTask\>)

Waits for all tasks in an enumerable collection to complete.

**Parameters:**
- `tasks` (IEnumerable\<ValueTask\>): The tasks to wait on for completion.

**Returns:** `ValueTask` - A task that represents the completion of all supplied tasks.

**Exceptions:**
- `ArgumentNullException`: If the tasks argument is null.

#### WhenAll(params ValueTask[])

Waits for all tasks in an array to complete.

**Parameters:**
- `tasks` (ValueTask[]): The tasks to wait on for completion.

**Returns:** `ValueTask` - A task that represents the completion of all supplied tasks.

**Exceptions:**
- `ArgumentNullException`: If the tasks argument is null.

#### WhenAll(ReadOnlySpan\<ValueTask\>) (.NET 9+)

Waits for all tasks in a span to complete.

**Parameters:**
- `tasks` (ReadOnlySpan\<ValueTask\>): The tasks to wait on for completion.

**Returns:** `ValueTask` - A task that represents the completion of all supplied tasks.

#### WhenAll\<TResult\>(IEnumerable\<ValueTask\<TResult\>\>)

Waits for all tasks in an enumerable collection to complete and returns their results.

**Parameters:**
- `tasks` (IEnumerable\<ValueTask\<TResult\>\>): The tasks to wait on for completion.

**Returns:** `ValueTask<TResult[]>` - A task containing an array of all task results in the same order as the input tasks.

**Exceptions:**
- `ArgumentNullException`: If the tasks argument is null.

#### WhenAll\<TResult\>(params ValueTask\<TResult\>[])

Waits for all tasks in an array to complete and returns their results.

**Parameters:**
- `tasks` (ValueTask\<TResult\>[]): The tasks to wait on for completion.

**Returns:** `ValueTask<TResult[]>` - A task containing an array of all task results in the same order as the input tasks.

**Exceptions:**
- `ArgumentNullException`: If the tasks argument is null.

#### WhenAll\<TResult\>(ReadOnlySpan\<ValueTask\<TResult\>\>) (.NET 9+)

Waits for all tasks in a span to complete and returns their results.

**Parameters:**
- `tasks` (ReadOnlySpan\<ValueTask\<TResult\>\>): The tasks to wait on for completion.

**Returns:** `ValueTask<TResult[]>` - A task containing an array of all task results in the same order as the input tasks.

### WithTimeoutAsync

#### WithTimeoutAsync(int, CancellationToken)

Waits for a task to complete within the specified timeout in milliseconds.

**Parameters:**
- `timeoutInMilliseconds` (int): Timeout in milliseconds. Use `Timeout.Infinite` (-1) to wait indefinitely.
- `cancellationToken` (CancellationToken): Optional cancellation token.

**Returns:** `Task<bool>` - `true` if the operation completed within the timeout, `false` otherwise.

**Exceptions:**
- `ArgumentNullException`: If the task is null.
- `ArgumentOutOfRangeException`: If timeout is less than -1.
- `OperationCanceledException`: If the operation is cancelled via the cancellation token.

#### WithTimeoutAsync(TimeSpan, CancellationToken)

Waits for a task to complete within the specified timeout.

**Parameters:**
- `timeout` (TimeSpan): Timeout duration. Use `Timeout.InfiniteTimeSpan` to wait indefinitely.
- `cancellationToken` (CancellationToken): Optional cancellation token.

**Returns:** `Task<bool>` - `true` if the operation completed within the timeout, `false` otherwise.

**Exceptions:**
- `ArgumentNullException`: If the task is null.
- `ArgumentOutOfRangeException`: If timeout is less than `Timeout.InfiniteTimeSpan`.
- `OperationCanceledException`: If the operation is cancelled via the cancellation token.

## 🎓 Best Practices

### WhenAll

1. **Prefer WhenAll over sequential awaits**: When you have multiple independent `ValueTask` operations, use `WhenAll` to run them concurrently instead of awaiting them sequentially.
2. **Result order is preserved**: The results array maintains the same order as the input tasks, regardless of completion order.
3. **Handle exceptions**: If any task throws an exception, it will be propagated. Consider wrapping in try-catch if needed.

### WithTimeoutAsync

1. **Check the return value**: Always check if the operation completed successfully before accessing results.
2. **Use TimeSpan for clarity**: Prefer `TimeSpan` overloads for better readability in production code.
3. **Handle cancellation**: Consider using `CancellationToken` for graceful shutdown scenarios.
4. **Don't ignore timeouts**: Log or handle timeout scenarios appropriately in your application.

## 🔗 Related Projects

- [NetEvolve.Arguments](https://github.com/dailydevops/arguments) - Argument validation library used internally

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/dailydevops/LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📞 Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/dailydevops/extensions.tasks).