using Microsoft.JSInterop;

namespace AFS.Core.Services
{
    /// <summary>
    /// JavaScript interop service for file operations.
    /// Sealed because it's a DI service not designed for inheritance.
    /// </summary>
    public sealed class JsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public JsInterop(IJSRuntime jsRuntime)
        {
            ArgumentNullException.ThrowIfNull(jsRuntime);
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/AFS.ComponentLibrary/JsInterop.js").AsTask());
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value.ConfigureAwait(false);
                await module.DisposeAsync().ConfigureAwait(false);
            }

            GC.SuppressFinalize(this);
        }

        public async ValueTask ExportToFileAsync(string fileName, DotNetStreamReference streamRef)
        {
            var module = await moduleTask.Value.ConfigureAwait(false);
            await module.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef).ConfigureAwait(false);
        }

        public async ValueTask TriggerClickAsync(string id)
        {
            var module = await moduleTask.Value.ConfigureAwait(false);
            await module.InvokeVoidAsync("triggerClick", id).ConfigureAwait(false);
        }

    }
}
