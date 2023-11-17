namespace NetEvolve.Extensions.Tasks;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Arguments;

/// <summary>
/// Extension methods for <see cref="Task"/>, <see cref="Task{TResult}" />, <see cref="ValueTask"/> and <see cref="ValueTask{TResult}"/>.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Returns a <see cref="bool" /> that will complete when any of the supplied <see cref="Task"/> objects have completed.
    /// </summary>
    /// <param name="task"><see cref="Task"/> to be accomplished.</param>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait before there is a negative result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>true, if <paramref name="task"/> was successful.</returns>
    [SuppressMessage(
        "Usage",
        "VSTHRD003:Avoid awaiting foreign Tasks",
        Justification = "As designed."
    )]
    public static async Task<bool> WithTimeoutAsync(
        this Task task,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        Argument.ThrowIfNull(task);
        Argument.ThrowIfLessThan(timeoutInMilliseconds, Timeout.Infinite);

        if (timeoutInMilliseconds <= 0)
        {
            await task.ConfigureAwait(false);
            return timeoutInMilliseconds == Timeout.Infinite;
        }

        if (task.IsCompleted)
        {
            return true;
        }

        var winner = await Task.WhenAny(task, Task.Delay(timeoutInMilliseconds, cancellationToken))
            .ConfigureAwait(false);
        await winner.ConfigureAwait(false);
        return winner == task;
    }
}
