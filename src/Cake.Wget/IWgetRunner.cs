namespace Cake.Wget
{
    /// <summary>
    /// Interface defining WgetRunner.
    /// </summary>
    public interface IWgetRunner
    {
        /// <summary>
        /// Run Wget tool.
        /// </summary>
        /// <param name="settings">Wget parameter settings.</param>
        /// <returns>Wget tool runner.</returns>
        IWgetRunner Run(WgetSettings settings);
    }
}
