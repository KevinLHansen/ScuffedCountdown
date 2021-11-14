namespace ScuffedCountdown.Data
{
    public class NumberModel : DrawableValueBase
    {
        public int Number { get; set; }

        public NumberModel(int number, int amount, ValueType type)
        {
            this.Number = number;
            this.Amount = amount;
            this.Type = type;
        }
    }
}
