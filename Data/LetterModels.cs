namespace ScuffedCountdown.Data
{
    public class LetterModel : DrawableValueBase
    {
        public char Character { get; set; }

        public LetterModel(char character, int amount, ValueType type)
        {
            this.Character = character;
            this.Amount = amount;
            this.Type = type;
        }
    }
}
