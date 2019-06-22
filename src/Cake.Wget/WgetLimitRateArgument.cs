using System;
using static System.FormattableString;

namespace Cake.Wget
{
    /// <summary>
    /// Create instance of this class to define value of argument <see cref="WgetSettings.LimitRate"/>.
    /// </summary>
    public partial class WgetLimitRateArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WgetLimitRateArgument"/> class with default units.
        /// </summary>
        /// <param name="value">Limit rate in bytes per second.</param>
        public WgetLimitRateArgument(double value)
        {
            ValidateLimitRateValue(value);

            Value = value;
            Unit = LimitRateUnitEnum.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WgetLimitRateArgument"/> class.
        /// </summary>
        /// <param name="value">Limit rate value.</param>
        /// <param name="unit">Limit rate units.</param>
        public WgetLimitRateArgument(double value, LimitRateUnitEnum unit)
        {
            ValidateLimitRateValue(value);

            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Gets Limit rate value.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Gets limit rate units.
        /// </summary>
        public LimitRateUnitEnum Unit { get; }

        /// <summary>
        /// Gets formatted value of the <see cref="WgetSettings.LimitRate"/> argument.
        /// </summary>
        /// <returns>Formatted argument value.</returns>
        public string GetFormattedLimitRateValue()
        {
            switch (Unit)
            {
                case LimitRateUnitEnum.None:
                    return Invariant($"{Value}");
                case LimitRateUnitEnum.Kilobytes:
                    return Invariant($"{Value}k");
                case LimitRateUnitEnum.Megabytes:
                    return Invariant($"{Value}m");
                default:
                    throw new NotImplementedException($"Limit rate unit '{Unit}' is not implemented.");
            }
        }

        private void ValidateLimitRateValue(double limitRate)
        {
            if (limitRate < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(limitRate), "Limit rate must be positive number.");
            }
        }
    }
}
