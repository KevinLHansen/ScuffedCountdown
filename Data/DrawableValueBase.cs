namespace ScuffedCountdown.Data
{
    public abstract class DrawableValueBase
    {
        public int Amount;
        public ValueType Type;
    }

    public enum ValueType
    {
        HighNumber = 0,
        LowNumber = 1,
        Vowel = 2,
        Consonant = 3
    }
}
