using Microsoft.JSInterop;

namespace ScuffedCountdown.Client.Services
{
    public class CommonJsService
    {
        public const string AUDIO_ERROR_ID = "audio-error";

        private readonly Task<IJSObjectReference> _ModuleTask;

        public CommonJsService(IJSRuntime jsRuntime)
        {
            _ModuleTask = jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./js/common.js").AsTask();
        }

        public async Task SetMasterColor(int hue, int saturation)
        {
            var module = await _ModuleTask;
            await module.InvokeVoidAsync("setMasterColor", $"{hue}, {saturation}%");
        }

        public async Task ClickElement(string id)
        {
            var module = await _ModuleTask;
            await module.InvokeVoidAsync("clickElement", id);
        }

        public async Task Alert(string message)
        {
            var module = await _ModuleTask;
            await module.InvokeVoidAsync("alert1", message);
        }

        public async Task PlayErrorSound()
        {
            var module = await _ModuleTask;
            await module.InvokeVoidAsync("playErrorSound", AUDIO_ERROR_ID);
        }
    }
}
