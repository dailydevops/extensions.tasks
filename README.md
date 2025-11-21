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

- **Timeout Support**: Execute async operations with configurable timeout handling
- **Multi-Framework Support**: Compatible with .NET Standard 2.0, .NET 8.0, .NET 9.0, and .NET 10.0
- **Lightweight**: Minimal dependencies, focused on essential functionality
- **Modern**: Supports both `Task` and `ValueTask` patterns

## 🚀 Usage

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