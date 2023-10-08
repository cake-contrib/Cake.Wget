namespace Cake.Wget
{
    /// <summary>
    /// Defines unit of limit rate value.
    /// </summary>
    public enum LimitRateUnitEnum
    {
        /// <summary>
        /// Limit rate value is in bytes per second.
        /// </summary>
        None = 0,

        /// <summary>
        /// Limit rate value is in kilobytes per second.
        /// </summary>
        Kilobytes,

        /// <summary>
        /// Limit rate value is in megabytes per second.
        /// </summary>
        Megabytes,
    }
}
