using ScuffedCountdown.Data;
using ValueType = ScuffedCountdown.Data.ValueType;

namespace ScuffedCountdown.Services
{
    public class DrawValueService
    {
        private List<DrawableValueBase> Values;
        private ServiceType Type;
        private Random Rng;

        public DrawValueService(ServiceType type)
        {
            Values = new List<DrawableValueBase>();
            Type = type;
            switch (type)
            {
                case ServiceType.Numbers:
                    InitializeNumbers();
                    break;

                case ServiceType.Letters:
                    InitializeLetters();
                    break;
            }
            Rng = new Random();
        }

        public string? DrawValue(ValueType type)
        {
            var values = Values.Where(x => x.Type == type);

            int amountsTotal = GetAmountsTotal(values);

            int selector = Rng.Next(1, amountsTotal + 1);

            var counter = 0;

            if (type == ValueType.HighNumber || type == ValueType.LowNumber)
            {
                foreach (var value in values)
                {
                    var number = (NumberModel)value;

                    counter += number.Amount;

                    if (selector <= counter)
                    {
                        int toReturn = number.Number;
                        if (number.Amount == 1)
                            Values.Remove(number);
                        else
                            number.Amount--;

                        return toReturn.ToString();
                    }
                }
            }
            else if (type == ValueType.Vowel || type == ValueType.Consonant)
            {
                foreach (var value in values)
                {
                    var letter = (LetterModel)value;

                    counter += letter.Amount;

                    if (selector <= counter)
                    {
                        char toReturn = letter.Character;
                        if (letter.Amount == 1)
                            Values.Remove(letter);
                        else
                            letter.Amount--;

                        return toReturn.ToString();
                    }
                }
            }
            return null;
        }

        public void Reset()
        {
            switch (Type)
            {
                case ServiceType.Numbers:
                    InitializeNumbers();
                    break;
                case ServiceType.Letters:
                    InitializeLetters();
                    break;
            }
        }

        private int GetAmountsTotal(IEnumerable<DrawableValueBase> values)
        {
            int amountsTotal = 0;
            foreach (var value in values) { amountsTotal += value.Amount; }
            return amountsTotal;
        }

        private void InitializeNumbers()
        {
            Values = new List<DrawableValueBase>
            {
                // High numbers
                new NumberModel(25, 1, ValueType.HighNumber),
                new NumberModel(50, 1, ValueType.HighNumber),
                new NumberModel(75, 1, ValueType.HighNumber),
                new NumberModel(100, 1, ValueType.HighNumber),
            };

            for (int i = 1; i <= 10; i++)
            {
                // Low numbers
                Values.Add(new NumberModel(i, 2, ValueType.LowNumber));
            }
        }

        private void InitializeLetters()
        {
            Values = new List<DrawableValueBase>
            {
                // Vowels
                new LetterModel('A', 15, ValueType.Vowel),
                new LetterModel('E', 21, ValueType.Vowel),
                new LetterModel('I', 13, ValueType.Vowel),
                new LetterModel('O', 13, ValueType.Vowel),
                new LetterModel('U', 5, ValueType.Vowel),
                // Consonants
                new LetterModel('B', 2, ValueType.Consonant),
                new LetterModel('C', 3, ValueType.Consonant),
                new LetterModel('D', 6, ValueType.Consonant),
                new LetterModel('F', 2, ValueType.Consonant),
                new LetterModel('G', 3, ValueType.Consonant),
                new LetterModel('H', 2, ValueType.Consonant),
                new LetterModel('J', 1, ValueType.Consonant),
                new LetterModel('K', 1, ValueType.Consonant),
                new LetterModel('L', 5, ValueType.Consonant),
                new LetterModel('M', 4, ValueType.Consonant),
                new LetterModel('N', 8, ValueType.Consonant),
                new LetterModel('P', 4, ValueType.Consonant),
                new LetterModel('Q', 1, ValueType.Consonant),
                new LetterModel('R', 9, ValueType.Consonant),
                new LetterModel('S', 9, ValueType.Consonant),
                new LetterModel('T', 9, ValueType.Consonant),
                new LetterModel('V', 1, ValueType.Consonant),
                new LetterModel('W', 1, ValueType.Consonant),
                new LetterModel('X', 1, ValueType.Consonant),
                new LetterModel('Y', 1, ValueType.Consonant),
                new LetterModel('Z', 1, ValueType.Consonant)
            };
        }

        public enum ServiceType
        {
            Numbers = 0,
            Letters = 1
        }
    }
}
