namespace ScuffedCountdown.Client.Extensions
{
    public static class GuidExtensions
    {
        public static string Short(this Guid guid)
        {
            var result = Convert.ToBase64String(guid.ToByteArray())
                .Replace("=", "");
            return result;
        }
    }
}
