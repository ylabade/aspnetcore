namespace System.Threading.Tasks
{
    public static class NullableTaskExtensions
    {
        public static Task<T?> AsNullable<T>(this Task<T> task) where T : class
            => task!;
    }
}

