using System.Diagnostics.CodeAnalysis;

namespace Cake.Wget
{
    /// <summary>
    /// Defines various static messages such errors, warnings etc.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Messages
    {
        /// <summary>
        /// Error message thrown if mandatory input parameters for <a href="https://www.gnu.org/software/wget/">Wget</a> tool are not set.
        /// </summary>
        public static readonly string InputParametersError = $"You must set at least one input parameter in '{nameof(WgetSettings)}': '{nameof(WgetSettings.Url)}' and/or '{nameof(WgetSettings.InputFile)}'";
    }
}
