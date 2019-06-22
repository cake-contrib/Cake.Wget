using System;
using System.Diagnostics.CodeAnalysis;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Wget
{
    /// <summary>
    /// Wget runner alias.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class WgetRunnerAliases
    {
        /// <summary>
        /// Runs Wget.
        /// </summary>
        /// <param name="context">Represents a context for scripts.</param>
        /// <param name="settings">Configuration items, class <see cref="WgetSettings"/>.</param>
        /// <returns>Instance of <see cref="WgetRunner"/> class.</returns>
        [CakeMethodAlias]
        public static IWgetRunner Wget(this ICakeContext context, WgetSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new WgetRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return runner.Run(settings);
        }
    }
}
