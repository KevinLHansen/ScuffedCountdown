using System.Text.Json;

namespace ScuffedCountdown.Client.Services
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Attempts to clone the object by serializing, then deserializing it.
        /// </summary>
        public static T Clone<T>(this object value)
        {
            var options = new JsonSerializerOptions { IncludeFields = true };
            var serialized = JsonSerializer.Serialize(value, options);
            var clone = JsonSerializer.Deserialize<T>(serialized, options);

            if (clone == null)
                throw new Exception("Error cloning object");

            return clone;
        }
    }
}
