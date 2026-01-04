namespace NetEvolve.Extensions.Tasks;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Extension methods for <see cref="Task"/>, <see cref="Task{TResult}" />, <see cref="ValueTask"/> and <see cref="ValueTask{TResult}"/>.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="ValueTask{TResult}"/> objects have completed.
    /// </summary>
    /// <param name="task"><see cref="ValueTask{TResult}"/> to be accomplished.</param>
    /// <param name="timeout">The time span to wait before the result is <see langword="false"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="task"/> was successful.</returns>
    public static async ValueTask<(bool isValid, T result)> WithTimeoutAsync<T>(
        this ValueTask<T> task,
        TimeSpan timeout,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(timeout, Timeout.InfiniteTimeSpan);

        if (timeout <= TimeSpan.Zero)
        {
            return (timeout == Timeout.InfiniteTimeSpan, await task.ConfigureAwait(false));
        }

        if (task.IsCompleted)
        {
            return (true, await task.ConfigureAwait(false));
        }

        var todoTask = task.AsTask();
        var winner = await Task.WhenAny(todoTask, Task.Delay(timeout, cancellationToken)).ConfigureAwait(false);
        var result = await todoTask.ConfigureAwait(false);
        return (winner == todoTask, result);
    }
}
