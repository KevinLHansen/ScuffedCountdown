using Blazored.LocalStorage;
using ColorHelper;

namespace ScuffedCountdown.Client.Services
{
    public class StateManager
    {
        private const string STATE_KEY = "grannyballs";

        private readonly ILocalStorageService _LocalStorage;

        public StateManager(ILocalStorageService localStorage)
        {
            _LocalStorage = localStorage;
        }

        public async Task Initalize()
        {
            var state = await _LocalStorage.GetItemAsync<ScuffedCountdownState>(STATE_KEY);
            if (state == null)
                await _LocalStorage.SetItemAsync(STATE_KEY, new ScuffedCountdownState());
        }

        /// <summary>
        /// Attempts to read state from local browser storage, creates new state instance if unsuccessful.
        /// </summary>
        public async Task<ScuffedCountdownState> GetState()
        {
            var state = await _LocalStorage.GetItemAsync<ScuffedCountdownState>(STATE_KEY);
            if (state == null)
                throw new Exception($"{nameof(ScuffedCountdownState)} not found in LocalStorage");

            return state;
        }

        /// <summary>
        /// Applies modifications defined in <paramref name="stateModifier"/> to state stored in browser.
        /// </summary>
        public async Task ModifyState(Action<ScuffedCountdownState> stateModifier)
        {
            var state = await _LocalStorage.GetItemAsync<ScuffedCountdownState>(STATE_KEY);
            stateModifier.Invoke(state);
            await _LocalStorage.SetItemAsync(STATE_KEY, state);
        }
    }

    public class ScuffedCountdownState
    {
        public UserSettings UserSettings { get; set; } = new();
        public GameState GameState { get; set; } = new();
    }

    public class UserSettings
    {
        public HSL? ThemeColor { get; set; }
        public bool UseFreeDictionary { get; set; } = true;
        public bool UseUrbanDictionary { get; set; } = true;
    }

    public class GameState
    {
        public int TeamScore1 { get; set; }
        public int TeamScore2 { get; set; }
    }
}
