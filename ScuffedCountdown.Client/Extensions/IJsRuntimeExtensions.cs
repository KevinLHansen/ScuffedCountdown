using Microsoft.JSInterop;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ScuffedCountdown.Client.Extensions
{
    public static class IJsRuntimeExtensions
    {
        /// <summary>
        /// Attempts to automatically resolve the path to the required JS file and then import it.
        /// </summary>
        /// <remarks>
        /// The component in which this method is used <b>must</b> be collocated by a <c>[component_name].razor.js</c> file. This is the file which will be imported.
        /// </remarks>
        /// <param name="caller">Do not provide this, it is resolved automatically.</param>
        public static Task<IJSObjectReference> ImportJsModule(this IJSRuntime jsRuntime, [CallerFilePath] string? caller = default)
        {
            if (caller == default)
                throw new Exception("Caller could not be resolved");

            var assemblyName = Assembly
                .GetCallingAssembly()
                .GetName()
                .Name ?? throw new Exception("Path could not be resolved");

            int startIndex = caller.IndexOf(assemblyName) + assemblyName.Length + 1;

            var modulePath = caller.Substring(startIndex)
                .Replace("\\", "/")
                .Replace("razor.cs", "razor.js");

            string fullPath = $"./{modulePath}";

            return jsRuntime.InvokeAsync<IJSObjectReference>("import", fullPath).AsTask();
        }
    }
}
