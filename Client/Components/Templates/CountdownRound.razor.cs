using Microsoft.AspNetCore.Components;
using ScuffedCountdown.Client.Services;
using ValueType = ScuffedCountdown.Client.Services.ValueType;

namespace ScuffedCountdown.Client.Components.Templates
{
    public partial class CountdownRound<T> : ComponentBase
    {
        [Inject]
        private CommonJsService _CommonJs { get; set; } = default!;

        [Parameter]
        public CountdownRoundType Type { get; set; }

        private ValueDrawingService<T> _ValueDrawer = default!;

        private List<DrawnValue<T>?> _DrawnValues = new();
        private List<DrawnValue<T>?> _OverdrawnValues = new();

        private bool _HasDraws = false;

        private string _DrawPrimaryLabel => Type switch
        {
            CountdownRoundType.Numbers => "Draw High Number",
            CountdownRoundType.Letters => "Draw Vowel",
            _ => ""
        };

        private string _DrawSecondaryLabel => Type switch
        {
            CountdownRoundType.Numbers => "Draw Low Number",
            CountdownRoundType.Letters => "Draw Consonant",
            _ => ""
        };

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _ValueDrawer = new(GetValues());
            _DrawnValues = InitializeDrawnValues();

        }

        private async Task DrawValue(ValueCategory category)
        {
            ValueType type = default;
            switch (Type)
            {
                case CountdownRoundType.Numbers:
                    switch (category)
                    {
                        case CountdownRound<T>.ValueCategory.Primary:
                            type = ValueType.HighNumber;
                            break;
                        case CountdownRound<T>.ValueCategory.Secondary:
                            type = ValueType.LowNumber;
                            break;
                    }
                    break;
                case CountdownRoundType.Letters:
                    switch (category)
                    {
                        case CountdownRound<T>.ValueCategory.Primary:
                            type = ValueType.Vowel;
                            break;
                        case CountdownRound<T>.ValueCategory.Secondary:
                            type = ValueType.Consonant;
                            break;
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid/unsupported category");
            }

            try
            {
                var value = _ValueDrawer.DrawValue(type);
                foreach (var slot in _DrawnValues)
                {
                    if (slot == null)
                    {
                        _DrawnValues[_DrawnValues.IndexOf(slot)] = new DrawnValue<T>(value);
                        _HasDraws = true;
                        return;
                    }
                }
                _OverdrawnValues.Add(new DrawnValue<T>(value));
            }
            catch (ValueListEmptyException)
            {
                await _CommonJs.Alert("No more draws for this category");
            }
        }

        private void Reset()
        {
            _ValueDrawer.Reset();
            _DrawnValues = InitializeDrawnValues();
            _HasDraws = false;
        }

        private List<DrawableValue<T>> GetValues()
        {
            var values = new List<DrawableValue<T>>();
            switch (Type)
            {
                case CountdownRoundType.Numbers:
                    values.AddRange(CountdownConstants.HighNumbers
                        .Cast<DrawableValue<T>>());
                    values.AddRange(CountdownConstants.LowNumbers
                        .Cast<DrawableValue<T>>());
                    break;
                case CountdownRoundType.Letters:
                    values.AddRange(CountdownConstants.Vowels
                        .Cast<DrawableValue<T>>());
                    values.AddRange(CountdownConstants.Consonants
                        .Cast<DrawableValue<T>>());
                    break;
            }
            return values;
        }

        private List<DrawnValue<T>?> InitializeDrawnValues()
        {
            var values = new List<DrawnValue<T>?>();
            int slotsAmount = 0;
            switch (Type)
            {
                case CountdownRoundType.Numbers:
                    slotsAmount = CountdownConstants.NumbersAmount;
                    break;
                case CountdownRoundType.Letters:
                    slotsAmount = CountdownConstants.LettersAmount;
                    break;
            }

            for (int i = 0; i < slotsAmount; i++)
                values.Add(null);

            return values;
        }

        private enum ValueCategory
        {
            Primary,
            Secondary
        }
    }

    public class DrawnValue<T>
    {
        public T Value { get; set; }

        public DrawnValue(T value)
        {
            Value = value;
        }
    }

    public enum CountdownRoundType
    {
        Numbers,
        Letters
    }
}
