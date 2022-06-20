namespace ScuffedCountdown.Client.Extensions
{
    public static class ListExtensions
    {
        public static T Random<T>(this List<T> list)
        {
            var rng = new Random();
            var index = rng.Next(0, list.Count);
            return list[index];
        }
    }
}
