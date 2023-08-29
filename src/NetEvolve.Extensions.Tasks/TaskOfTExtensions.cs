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
    /// <param name="task"><see cref="ValueTask"/> to be accomplished.</param>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="task"/> was successful.</returns>
    public static async ValueTask<bool> WithTimeoutAsync(
        this ValueTask task,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        var todoTask = task.AsTask();
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
    /// <param name="task"><see cref="Task"/> to be accomplished.</param>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="task"/> was successful.</returns>
    public static async Task<bool> WithTimeoutAsync(
        this Task task,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        Argument.ThrowIfNull(task);

        var winner = await Task.WhenAny(task, Task.Delay(timeoutInMilliseconds, cancellationToken))
            .ConfigureAwait(false);
        return winner == task;
    }

    /// <summary>
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="Task{TResult}"/> objects have completed.
    /// </summary>
    /// <param name="task"><see cref="Task{TResult}"/> to be accomplished.</param>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="task"/> was successful.</returns>
    [SuppressMessage(
        "Usage",
        "VSTHRD003:Avoid awaiting foreign Tasks",
        Justification = "As designed."
    )]
    public static async Task<(bool isValid, T result)> WithTimeoutAsync<T>(
        this Task<T> task,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        Argument.ThrowIfNull(task);

        var winner = await Task.WhenAny(task, Task.Delay(timeoutInMilliseconds, cancellationToken))
            .ConfigureAwait(false);
        var result = await task.ConfigureAwait(false);
        return (winner == task, result);
    }

    /// <summary>
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="ValueTask{TResult}"/> objects have completed.
    /// </summary>
    /// <param name="task"><see cref="ValueTask{TResult}"/> to be accomplished.</param>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="task"/> was successful.</returns>
    public static async ValueTask<(bool isValid, T result)> WithTimeoutAsync<T>(
        this ValueTask<T> task,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        var todoTask = task.AsTask();
        var winner = await Task.WhenAny(
                todoTask,
                Task.Delay(timeoutInMilliseconds, cancellationToken)
            )
            .ConfigureAwait(false);
        var result = await todoTask.ConfigureAwait(false);
        return (winner == todoTask, result);
    }
}
