using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Wget
{
    /// <summary>
    /// A wrapper around the Wget tool.
    /// </summary>
    public class WgetRunner : Tool<WgetSettings>, IWgetRunner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WgetRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">Represents a file system.</param>
        /// <param name="environment">Represents the environment Cake operates in.</param>
        /// <param name="processRunner">Represents a process runner.</param>
        /// <param name="tools">Represents a tool locator.</param>
        public WgetRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Runs Wget tool.
        /// </summary>
        /// <param name="settings">Wget tool settings - <see cref="WgetSettings"/> class.</param>
        /// <returns>Wget runner instance.</returns>
        public IWgetRunner Run(WgetSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.Url == null && string.IsNullOrWhiteSpace(settings.InputFile))
            {
                throw new ArgumentException(Messages.InputParametersError);
            }

            var args = GetSettingsArguments(settings);
            Run(settings, args);
            return this;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>Tool name.</returns>
        protected override string GetToolName() => "wget";

        /// <summary>
        /// Gets the name of the tool executables.
        /// </summary>
        /// <returns>Possible tool executable names.</returns>
        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "wget.exe", "wget" };

        private static ProcessArgumentBuilder GetSettingsArguments(WgetSettings settings)
        {
            var args = new ProcessArgumentBuilder();
            settings?.Evaluate(args);
            return args;
        }
    }
}
