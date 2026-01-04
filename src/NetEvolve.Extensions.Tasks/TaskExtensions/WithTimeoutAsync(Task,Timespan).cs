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
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="Task"/> objects have completed.
    /// </summary>
    /// <param name="task"><see cref="Task"/> to be accomplished.</param>
    /// <param name="timeout">The time span to wait before the result is <see langword="false"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="task"/> was successful.</returns>
    public static async Task<bool> WithTimeoutAsync(
        this Task task,
        TimeSpan timeout,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentOutOfRangeException.ThrowIfLessThan(timeout, Timeout.InfiniteTimeSpan);

        if (timeout <= TimeSpan.Zero)
        {
            await task.ConfigureAwait(false);
            return timeout == Timeout.InfiniteTimeSpan;
        }

        if (task.IsCompleted)
        {
            return true;
        }

        var winner = await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)).ConfigureAwait(false);
        await winner.ConfigureAwait(false);
        return winner == task;
    }
}
