namespace NetEvolve.Extensions.Tasks;

using NetEvolve.Arguments;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Extension methods for <see cref="Task"/>, <see cref="Task{TResult}" />, <see cref="ValueTask"/> and <see cref="ValueTask{TResult}"/>.
/// </summary>
public static class TaskOfTExtensions
{
    /// <summary>
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="ValueTask"/> objects have completed.
    /// </summary>
    /// <param name="todo"><see cref="ValueTask"/> to be accomplished.</param>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="todo"/> was successful.</returns>
    public static async ValueTask<bool> WithTimeoutAsync(
        this ValueTask todo,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        var todoTask = todo.AsTask();
        var winner = await Task.WhenAny(
                todoTask,
                Task.Delay(timeoutInMilliseconds, cancellationToken)
            )
            .ConfigureAwait(false);
        return winner == todoTask;
    }

    /// <summary>
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="Task"/> objects have completed.
    /// </summary>
    /// <param name="todo"><see cref="Task"/> to be accomplished.</param>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="todo"/> was successful.</returns>
    public static async Task<bool> WithTimeoutAsync(
        this Task todo,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        Argument.ThrowIfNull(todo);

        var winner = await Task.WhenAny(todo, Task.Delay(timeoutInMilliseconds, cancellationToken))
            .ConfigureAwait(false);
        return winner == todo;
    }

    /// <summary>
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="Task{TResult}"/> objects have completed.
    /// </summary>
    /// <param name="todo"><see cref="Task{TResult}"/> to be accomplished.</param>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="todo"/> was successful.</returns>
    [SuppressMessage(
        "Usage",
        "VSTHRD003:Avoid awaiting foreign Tasks",
        Justification = "As designed."
    )]
    public static async Task<(bool isValid, T result)> WithTimeoutAsync<T>(
        this Task<T> todo,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        Argument.ThrowIfNull(todo);

        var winner = await Task.WhenAny(todo, Task.Delay(timeoutInMilliseconds, cancellationToken))
            .ConfigureAwait(false);
        var result = await todo.ConfigureAwait(false);
        return (winner == todo, result);
    }

    /// <summary>
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="ValueTask{TResult}"/> objects have completed.
    /// </summary>
    /// <param name="todo"><see cref="ValueTask{TResult}"/> to be accomplished.</param>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="todo"/> was successful.</returns>
    public static async ValueTask<(bool isValid, T result)> WithTimeoutAsync<T>(
        this ValueTask<T> todo,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        var todoTask = todo.AsTask();
        var winner = await Task.WhenAny(
                todoTask,
                Task.Delay(timeoutInMilliseconds, cancellationToken)
            )
            .ConfigureAwait(false);
        var result = await todoTask.ConfigureAwait(false);
        return (winner == todoTask, result);
    }
}
