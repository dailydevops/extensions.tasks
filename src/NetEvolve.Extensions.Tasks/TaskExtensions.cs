namespace NetEvolve.Extensions.Tasks;

using System.Threading.Tasks;

/// <summary>
/// Extension methods for <see cref="Task"/>, <see cref="Task{TResult}" />, <see cref="ValueTask"/> and <see cref="ValueTask{TResult}"/>.
/// </summary>
#if NETSTANDARD2_0
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Microsoft.Design",
    "CA1062:Validate arguments of public methods",
    Justification = "False positive"
)]
#endif
public static partial class TaskExtensions { }
