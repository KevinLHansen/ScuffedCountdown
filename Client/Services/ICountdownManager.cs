namespace ScuffedCountdown.Client.Services
{
    public interface ICountdownManager
    {
        public void SetScore(string team, int score);
        public int GetScore(string team);
        public int AddScore(string team, int score);
        public int SubtractScore(string team, int score);
    }
}
