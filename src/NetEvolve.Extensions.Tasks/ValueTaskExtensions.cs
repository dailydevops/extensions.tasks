namespace NetEvolve.Extensions.Tasks;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides extension methods for <see cref="ValueTask"/> and <see cref="ValueTask{TResult}"/>.
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "As designed.")]
[SuppressMessage(
    "Design",
    "CA1000:Do not declare static members on generic types",
    Justification = "Extension methods require static declaration."
)]
public static class ValueTaskExtensions
{
    extension(ValueTask)
    {
        /// <summary>
        /// Creates a <see cref="ValueTask"/> that will complete when all of the <see cref="ValueTask"/> objects in an enumerable collection have completed.
        /// </summary>
        /// <param name="tasks">The tasks to wait on for completion.</param>
        /// <returns>A <see cref="ValueTask"/> that represents the completion of all of the supplied tasks.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument was <see langword="null"/>.</exception>
        [SuppressMessage(
            "Style",
            "VSTHRD200:Use \"Async\" suffix for async methods",
            Justification = "Matching Task.WhenAll naming convention."
        )]
        public static async ValueTask WhenAll(IEnumerable<ValueTask> tasks)
        {
            ArgumentNullException.ThrowIfNull(tasks);

            var taskList = tasks as IList<ValueTask> ?? [.. tasks];

            if (taskList.Count == 0)
            {
                return;
            }

            for (var i = 0; i < taskList.Count; i++)
            {
                _ = taskList[i].GetAwaiter();
            }

            for (var i = 0; i < taskList.Count; i++)
            {
                await taskList[i].ConfigureAwait(false);
            }
        }

#if NET9_0_OR_GREATER
        /// <summary>
        /// Creates a <see cref="ValueTask"/> that will complete when all of the supplied tasks have completed.
        /// </summary>
        /// <param name="tasks">The tasks to wait on for completion.</param>
        /// <returns>A <see cref="ValueTask"/> that represents the completion of all of the supplied tasks.</returns>
        [SuppressMessage(
            "Style",
            "VSTHRD200:Use \"Async\" suffix for async methods",
            Justification = "Matching Task.WhenAll naming convention."
        )]
        public static ValueTask WhenAll(scoped ReadOnlySpan<ValueTask> tasks)
        {
            if (tasks.Length == 0)
            {
                return ValueTask.CompletedTask;
            }

            return WhenAllCore(tasks.ToArray());
        }
#endif

        /// <summary>
        /// Creates a <see cref="ValueTask"/> that will complete when all of the <see cref="ValueTask"/> objects in an array have completed.
        /// </summary>
        /// <param name="tasks">The tasks to wait on for completion.</param>
        /// <returns>A <see cref="ValueTask"/> that represents the completion of all of the supplied tasks.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument was <see langword="null"/>.</exception>
        [SuppressMessage(
            "Style",
            "VSTHRD200:Use \"Async\" suffix for async methods",
            Justification = "Matching Task.WhenAll naming convention."
        )]
        [SuppressMessage(
            "Maintainability",
            "CA1062:Validate arguments of public methods",
            Justification = "Parameter is validated with ArgumentNullException.ThrowIfNull."
        )]
        public static ValueTask WhenAll(params ValueTask[] tasks)
        {
            ArgumentNullException.ThrowIfNull(tasks);

            if (tasks.Length == 0)
            {
                return default;
            }

            return WhenAllCore(tasks);
        }

        [SuppressMessage(
            "Style",
            "VSTHRD200:Use \"Async\" suffix for async methods",
            Justification = "Private helper method."
        )]
        private static async ValueTask WhenAllCore(ValueTask[] tasks)
        {
            for (var i = 0; i < tasks.Length; i++)
            {
                _ = tasks[i].GetAwaiter();
            }

            for (var i = 0; i < tasks.Length; i++)
            {
                await tasks[i].ConfigureAwait(false);
            }
        }
    }

    extension<TResult>(ValueTask<TResult>)
    {
        /// <summary>
        /// Creates a <see cref="ValueTask{TResult}"/> that will complete when all of the <see cref="ValueTask{TResult}"/> objects in an enumerable collection have completed.
        /// </summary>
        /// <param name="tasks">The tasks to wait on for completion.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> that represents the completion of all of the supplied tasks.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument was <see langword="null"/>.</exception>
        [SuppressMessage(
            "Style",
            "VSTHRD200:Use \"Async\" suffix for async methods",
            Justification = "Matching Task.WhenAll naming convention."
        )]
        public static async ValueTask<TResult[]> WhenAll(IEnumerable<ValueTask<TResult>> tasks)
        {
            ArgumentNullException.ThrowIfNull(tasks);

            var taskList = tasks as IList<ValueTask<TResult>> ?? [.. tasks];

            if (taskList.Count == 0)
            {
                return [];
            }

            var results = new TResult[taskList.Count];

            for (var i = 0; i < taskList.Count; i++)
            {
                _ = taskList[i].GetAwaiter();
            }

            for (var i = 0; i < taskList.Count; i++)
            {
                results[i] = await taskList[i].ConfigureAwait(false);
            }

            return results;
        }

#if NET9_0_OR_GREATER
        /// <summary>
        /// Creates a <see cref="ValueTask{TResult}"/> that will complete when all of the supplied tasks have completed.
        /// </summary>
        /// <param name="tasks">The tasks to wait on for completion.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> that represents the completion of all of the supplied tasks.</returns>
        [SuppressMessage(
            "Style",
            "VSTHRD200:Use \"Async\" suffix for async methods",
            Justification = "Matching Task.WhenAll naming convention."
        )]
        public static ValueTask<TResult[]> WhenAll(scoped ReadOnlySpan<ValueTask<TResult>> tasks)
        {
            if (tasks.Length == 0)
            {
                return new ValueTask<TResult[]>([]);
            }

            return WhenAllCore(tasks.ToArray());
        }
#endif

        /// <summary>
        /// Creates a <see cref="ValueTask{TResult}"/> that will complete when all of the <see cref="ValueTask{TResult}"/> objects in an array have completed.
        /// </summary>
        /// <param name="tasks">The tasks to wait on for completion.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> that represents the completion of all of the supplied tasks.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="tasks"/> argument was <see langword="null"/>.</exception>
        [SuppressMessage(
            "Style",
            "VSTHRD200:Use \"Async\" suffix for async methods",
            Justification = "Matching Task.WhenAll naming convention."
        )]
        [SuppressMessage(
            "Maintainability",
            "CA1062:Validate arguments of public methods",
            Justification = "Parameter is validated with ArgumentNullException.ThrowIfNull."
        )]
        public static ValueTask<TResult[]> WhenAll(params ValueTask<TResult>[] tasks)
        {
            ArgumentNullException.ThrowIfNull(tasks);

            if (tasks.Length == 0)
            {
                return new ValueTask<TResult[]>([]);
            }

            return WhenAllCore(tasks);
        }

        [SuppressMessage(
            "Style",
            "VSTHRD200:Use \"Async\" suffix for async methods",
            Justification = "Private helper method."
        )]
        private static async ValueTask<TResult[]> WhenAllCore(ValueTask<TResult>[] tasks)
        {
            var results = new TResult[tasks.Length];

            for (var i = 0; i < tasks.Length; i++)
            {
                _ = tasks[i].GetAwaiter();
            }

            for (var i = 0; i < tasks.Length; i++)
            {
                results[i] = await tasks[i].ConfigureAwait(false);
            }

            return results;
        }
    }
}
