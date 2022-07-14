namespace ScuffedCountdown.Client.Services
{
    public interface ICountdownManager
    {
        public Task Initialize();
        public Task SetScore(string team, int score);
        public Task<int> GetScore(string team);
        public Task<int> AddScore(string team, int score);
        public Task<int> SubtractScore(string team, int score);
    }
}
