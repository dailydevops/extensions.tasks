namespace NetEvolve.Extensions.Tasks;

using NetEvolve.Arguments;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Extension methods for <see cref="Task"/>, <see cref="Task{TResult}" />, <see cref="ValueTask"/> and <see cref="ValueTask{TResult}"/>.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="ValueTask"/> objects have completed.
    /// </summary>
    /// <param name="task"><see cref="ValueTask"/> to be accomplished.</param>
    /// <param name="timeout">The time span to wait before the result is <see langword="false"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="task"/> was successful.</returns>
    public static async ValueTask<bool> WithTimeoutAsync(
        this ValueTask task,
        TimeSpan timeout,
        CancellationToken cancellationToken = default
    )
    {
        Argument.ThrowIfLessThan(timeout, Timeout.InfiniteTimeSpan);

        if (timeout <= TimeSpan.Zero)
        {
            await task.ConfigureAwait(false);
            return timeout == Timeout.InfiniteTimeSpan;
        }

        if (task.IsCompleted)
        {
            return true;
        }

        var todoTask = task.AsTask();
        var winner = await Task.WhenAny(todoTask, Task.Delay(timeout, cancellationToken))
            .ConfigureAwait(false);
        await winner.ConfigureAwait(false);
        return winner == todoTask;
    }
}
