
using ScuffedCountdown.Client.Extensions;

namespace ScuffedCountdown.Client.Services
{
    public class ValueDrawingService<T>
    {
        private List<DrawableValue<T>> _Values;
        private List<DrawableValue<T>> _ValuesInitial;
        private Random _Rng = new Random();

        public ValueDrawingService(List<DrawableValue<T>> values)
        {
            if (values.Count < 1)
                throw new ArgumentException("Empty list provided");

            _Values = values;
            _ValuesInitial = _Values.Clone<List<DrawableValue<T>>>();
        }

        public T DrawValue(ValueType type)
        {
            var values = _Values.Where(x => x.Type == type);
            var amountsTotal = GetAmountsTotal(values);
            var selector = _Rng.Next(1, amountsTotal + 1);

            var counter = 0;
            foreach (var value in values)
            {
                counter += value.Amount;
                if (selector <= counter)
                {
                    if (value.Amount == 1)
                        _Values.Remove(value);
                    else
                        value.Amount--;

                    return value.Value;
                }
            }
            throw new ValueListEmptyException();
        }

        public void Reset()
        {
            _Values = _ValuesInitial.Clone<List<DrawableValue<T>>>();
        }

        private int GetAmountsTotal(IEnumerable<DrawableValue<T>> values)
        {
            int amountsTotal = 0;
            foreach (var value in values)
                amountsTotal += value.Amount;
            return amountsTotal;
        }
    }

    public class DrawableValue<T>
    {
        public T Value;
        public int Amount;
        public ValueType Type;

        public DrawableValue(T value, int amount, ValueType type)
        {
            Value = value;
            Amount = amount;
            Type = type;
        }
    }

    public enum ValueType
    {
        HighNumber,
        LowNumber,
        Vowel,
        Consonant
    }

    public class ValueListEmptyException : Exception
    {
        public override string Message => "Value list is empty";
    }
}
