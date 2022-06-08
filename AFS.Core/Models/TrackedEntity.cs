namespace AFS.Core.Models
{
    public class TrackedEntity
    {
        public event Action? OnChange;
        internal void NotifyStateChanged() => OnChange?.Invoke();
    }
}
