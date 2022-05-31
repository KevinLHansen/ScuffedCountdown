namespace ScuffedCountdown.Client.Services
{
    public class CountdownManager : ICountdownManager
    {
        private Dictionary<string, Team> _Teams = new Dictionary<string, Team>
        {
            { "team1", new Team() },
            { "team2", new Team() }
        };


        public int GetScore(string team)
        {
            return _Teams[team].Score;
        }

        public void SetScore(string team, int score)
        {
            _Teams[team].Score = score;
        }

        public int AddScore(string team, int score)
        {
            var newScore = _Teams[team].Score + score;
            SetScore(team, newScore);
            return newScore;
        }

        public int SubtractScore(string team, int score)
        {
            var newScore = _Teams[team].Score - score;
            SetScore(team, newScore);
            return newScore;
        }
    }

    public class Team
    {
        public int Score { get; set; } = 0;
    }
}
