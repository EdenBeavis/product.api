namespace product.api.Infrastructure
{
    /// <summary>
    /// Instead of using Option<bool> to allow railroad in void methods, we use this class instead to point out that it's a void.
    /// </summary>
    public sealed class Void
    {
        private Void()
        {
        }

        public static Void Instance => new Void();
    }
}