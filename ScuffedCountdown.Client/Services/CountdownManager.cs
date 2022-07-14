namespace ScuffedCountdown.Client.Services
{
    public class CountdownManager : ICountdownManager
    {
        private const string TEAM1_KEY = "team1";
        private const string TEAM2_KEY = "team2";

        private readonly StateManager _StateManager;

        public CountdownManager(StateManager stateManager)
        {
            _StateManager = stateManager;
        }

        private Dictionary<string, Team> _Teams = new Dictionary<string, Team>
        {
            { TEAM1_KEY, new Team() },
            { TEAM2_KEY, new Team() }
        };

        public async Task Initialize()
        {
            var state = await _StateManager.GetState();
            _Teams[TEAM1_KEY].Score = state.GameState.TeamScore1;
            _Teams[TEAM2_KEY].Score = state.GameState.TeamScore2;
        }

        public async Task<int> GetScore(string team)
        {
            var state = await _StateManager.GetState();

            switch (team)
            {
                case TEAM1_KEY:
                    return state.GameState.TeamScore1;
                case TEAM2_KEY:
                    return state.GameState.TeamScore2;
            }
            return 0;
        }

        public async Task SetScore(string team, int score)
        {
            _Teams[team].Score = score;

            await _StateManager.ModifyState(state =>
            {
                var gameState = state.GameState;
                switch (team)
                {
                    case TEAM1_KEY:
                        gameState.TeamScore1 = score;
                        break;
                    case TEAM2_KEY:
                        gameState.TeamScore2 = score;
                        break;
                }
            }); 
        }

        public async Task<int> AddScore(string team, int score)
        {
            var newScore = _Teams[team].Score + score;
            await SetScore(team, newScore);
            return newScore;
        }

        public async Task<int> SubtractScore(string team, int score)
        {
            var newScore = _Teams[team].Score - score;
            await SetScore(team, newScore);
            return newScore;
        }
    }

    public class Team
    {
        public int Score { get; set; } = 0;
    }
}
