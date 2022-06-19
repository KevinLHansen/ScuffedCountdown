namespace ScuffedCountdown.Client.Services
{
    public static class CountdownConstants
    {
        public static List<DrawableValue<int>> HighNumbers = new()
        {
            new(25, 1, ValueType.HighNumber),
            new(50, 1, ValueType.HighNumber),
            new(75, 1, ValueType.HighNumber),
            new(100, 1, ValueType.HighNumber)
        };

        public static List<DrawableValue<int>> LowNumbers = new()
        {
            new(1, 2, ValueType.LowNumber),
            new(1, 2, ValueType.LowNumber),
            new(2, 2, ValueType.LowNumber),
            new(2, 2, ValueType.LowNumber),
            new(3, 2, ValueType.LowNumber),
            new(3, 2, ValueType.LowNumber),
            new(4, 2, ValueType.LowNumber),
            new(4, 2, ValueType.LowNumber),
            new(5, 2, ValueType.LowNumber),
            new(5, 2, ValueType.LowNumber),
            new(6, 2, ValueType.LowNumber),
            new(6, 2, ValueType.LowNumber),
            new(7, 2, ValueType.LowNumber),
            new(7, 2, ValueType.LowNumber),
            new(8, 2, ValueType.LowNumber),
            new(8, 2, ValueType.LowNumber),
            new(9, 2, ValueType.LowNumber),
            new(9, 2, ValueType.LowNumber),
            new(10, 2, ValueType.LowNumber),
            new(10, 2, ValueType.LowNumber)
        };

        public static List<DrawableValue<char>> Vowels = new()
        {
            new('A', 15, ValueType.Vowel),
            new('E', 21, ValueType.Vowel),
            new('I', 13, ValueType.Vowel),
            new('O', 13, ValueType.Vowel),
            new('U', 5, ValueType.Vowel)
        };

        public static List<DrawableValue<char>> Consonants = new()
        {
            new('B', 2, ValueType.Consonant),
            new('C', 3, ValueType.Consonant),
            new('D', 6, ValueType.Consonant),
            new('F', 2, ValueType.Consonant),
            new('G', 3, ValueType.Consonant),
            new('H', 2, ValueType.Consonant),
            new('J', 1, ValueType.Consonant),
            new('K', 1, ValueType.Consonant),
            new('L', 5, ValueType.Consonant),
            new('M', 4, ValueType.Consonant),
            new('N', 8, ValueType.Consonant),
            new('P', 4, ValueType.Consonant),
            new('Q', 1, ValueType.Consonant),
            new('R', 9, ValueType.Consonant),
            new('S', 9, ValueType.Consonant),
            new('T', 9, ValueType.Consonant),
            new('V', 1, ValueType.Consonant),
            new('W', 1, ValueType.Consonant),
            new('X', 1, ValueType.Consonant),
            new('Y', 1, ValueType.Consonant),
            new('Z', 1, ValueType.Consonant)
        };

        public static int NumbersAmount = 6;
        public static int LettersAmount = 9;
    }
}
