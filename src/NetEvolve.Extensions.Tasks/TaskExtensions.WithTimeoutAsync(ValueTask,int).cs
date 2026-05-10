namespace NetEvolve.Extensions.Tasks;

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
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="task"/> was successful.</returns>
    public static async ValueTask<bool> WithTimeoutAsync(
        this ValueTask task,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(timeoutInMilliseconds, Timeout.Infinite);

        if (timeoutInMilliseconds <= 0)
        {
            await task.ConfigureAwait(false);
            return timeoutInMilliseconds == Timeout.Infinite;
        }

        if (task.IsCompleted)
        {
            return true;
        }

        var todoTask = task.AsTask();
        var winner = await Task.WhenAny(todoTask, Task.Delay(timeoutInMilliseconds, cancellationToken))
            .ConfigureAwait(false);
        await winner.ConfigureAwait(false);
        return winner == todoTask;
    }
}
