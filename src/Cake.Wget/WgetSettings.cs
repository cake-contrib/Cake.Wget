using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Cake.Core.Tooling;

namespace Cake.Wget
{
    /// <summary>
    /// Class holding configuration items for Wget Cake alias.
    /// </summary>
    /// <remarks>
    /// For detailed reference of available command options see Wget <a href="https://www.gnu.org/software/wget/manual/wget.html">manual pages</a>
    /// Additional parameters you can define via <c>ArgumentCustomization</c>.
    /// <code>
    /// var settings = new WgetSettings {
    ///     ArgumentCustomization = args => args.Append("YOUR_PARAM_HERE")
    /// };
    /// </code>
    /// </remarks>
    public class WgetSettings : ToolSettings
    {
        /// <summary>
        /// Separator character between Wget parameter switch and its value.
        /// </summary>
        public const string SwitchSeparator = "=";

        /// <summary>
        /// Print a help message describing all of Wget’s command-line options.
        /// </summary>
        [WgetArgumentName("--help")]
        public bool Help { get; set; }

        /// <summary>
        /// Display the version of Wget.
        /// </summary>
        [WgetArgumentName("--version")]
        public bool Version { get; set; }

        /// <summary>
        /// URL to be downloaded.
        /// </summary>
        [WgetArgumentName("")]
        public Uri Url { get; set; }

        /// <summary>
        /// The documents will not be written to the appropriate files, but all will be concatenated together and written to file.
        /// If <c>-</c> is used as file, documents will be printed to standard output, disabling link conversion.
        /// (Use <samp>./-</samp> to print to a file literally named <c>-</c>.)
        /// </summary>
        [WgetArgumentName("--output-document")]
        public string OutputDocument { get; set; }

        /// <summary>
        /// Log all messages to <c>logfile</c>. The messages are normally reported to standard error.
        /// </summary>
        [WgetArgumentName("--output-file")]
        public string OutputFile { get; set; }

        /// <summary>
        /// Append to logfile. This is the same as <c>-o</c>, only it appends to logfile instead of overwriting the old log file.
        /// If logfile does not exist, a new file is created.
        /// </summary>
        [WgetArgumentName("--append-output")]
        public string AppendOutput { get; set; }

        /// <summary>
        /// Turn off Wget’s output.
        /// </summary>
        [WgetArgumentName("--quiet")]
        public bool Quiet { get; set; }

        /// <summary>
        /// <para>Turn on debug output, meaning various information important to the developers of Wget if it does not work properly.</para>
        /// <para>Your system administrator may have chosen to compile Wget without debug support, in which case <see cref="Debug"/> will not work.
        /// Please note that compiling with debug support is always safe. Wget compiled with the debug support will not print any debug info unless requested with <see cref="Debug"/>.</para>
        /// </summary>
        [WgetArgumentName("--debug")]
        public bool Debug { get; set; }

        /// <summary>
        /// Turn on verbose output, with all the available data. The default output is verbose.
        /// </summary>
        [WgetArgumentName("--verbose")]
        public bool Verbose { get; set; }

        /// <summary>
        /// Turn off verbose without being completely quiet (use <see cref="Quiet"/> for that), which means that error messages and basic information still get printed.
        /// </summary>
        [WgetArgumentName("--no-verbose")]
        public bool NoVerbose { get; set; }

        /// <summary>
        /// Read URLs from a local or external file. If <c>-</c> is specified as file, URLs are read from the standard input.
        /// (Use <sampl>./-</sampl> to read from a file literally named <c>-</c>.)
        /// </summary>
        /// <remarks>
        /// <para>If this function is used, no URLs need be present on the command line.
        /// If there are URLs both on the command line and in an input file, those on the command lines will be the first ones to be retrieved.
        /// If <see cref="ForceHtml"/> is not specified, then file should consist of a series of URLs, one per line.</para>
        /// <para>However, if you specify <see cref="ForceHtml"/>, the document will be regarded as <c>html</c>.
        /// In that case you may have problems with relative links, which you can solve either by adding <c><base href="url" /></c> to the documents or by specifying <see cref="Base"/>.</para>
        /// <para>If the file is an external one, the document will be automatically treated as <c>html</c> if the Content-Type matches <c>text/html</c>. Furthermore, the file’s location will be implicitly used as base href if none was specified.</para>
        /// </remarks>
        [WgetArgumentName("--input-file")]
        public string InputFile { get; set; }

        /// <summary>
        /// When input is read from a file, force it to be treated as an HTML file. This enables you to retrieve relative links from existing HTML files on your local disk,
        /// by adding <c><base href="url" /></c> to HTML, or using the <see cref="Base"/> command-line option.
        /// </summary>
        [WgetArgumentName("--force-html")]
        public bool ForceHtml { get; set; }

        /// <summary>
        /// Resolves relative links using URL as the point of reference, when reading links from an HTML file specified via the <seealso cref="InputFile"/> option (together with <seealso cref="ForceHtml"/>, or when the input file was fetched remotely from a server describing it as HTML).
        /// This is equivalent to the presence of a BASE tag in the HTML input file, with URL as the value for the href attribute.
        /// </summary>
        /// <remarks>
        /// For instance, if you specify <samp>http://foo/bar/a.html</samp> for URL, and Wget reads <samp>../baz/b.html</samp> from the input file, it would be resolved to <samp>http://foo/baz/b.html</samp>.
        /// </remarks>
        [WgetArgumentName("--base")]
        public string Base { get; set; }

        /// <summary>
        /// Logs all URL rejections to <c>logfile</c> as comma separated values.
        /// The values include the reason of rejection, the URL and the parent URL it was found in.
        /// </summary>
        [WgetArgumentName("--rejected-log")]
        public string RejectedLog { get; set; }

        /// <summary>
        /// Set number of tries to number.
        /// </summary>
        /// <remarks>
        /// The default is to retry <c>20</c> times, with the exception of fatal errors like <emp>connection refused</emp> or <emp>not found</emp> (404), which are not retried.
        /// </remarks>
        [WgetArgumentName("--tries")]
        public uint Tries { get; set; }

        /// <summary>
        /// Set directory prefix to <c>prefix</c>.
        /// </summary>
        /// <remarks>
        /// The directory prefix is the directory where all other files and subdirectories will be saved to, i.e. the top of the retrieval tree.
        /// The default is ‘.’ (the current directory).
        /// </remarks>
        [WgetArgumentName("--directory-prefix")]
        public string DirectoryPrefix { get; set; }

        /// <summary>
        /// Limit the download speed to <c>amount</c> bytes per second. Amount may be expressed in bytes, kilobytes or megabytes.
        /// This is useful when, for whatever reason, you don’t want Wget to consume the entire available bandwidth.
        /// <code>
        /// LimitRate = new WgetLimitRateArgument(300, LimitRateUnitEnum.Kilobytes);
        /// </code>
        /// <seealso cref="WgetLimitRateArgument"/>
        /// <seealso cref="LimitRateUnitEnum"/>
        /// </summary>
        /// <remarks>
        /// Note that Wget implements the limiting by sleeping the appropriate amount of time after a network read that took less time than specified by the rate.
        /// Eventually this strategy causes the TCP transfer to slow down to approximately the specified rate.However, it may take some time for this balance to be achieved,
        /// so don’t be surprised if limiting the rate does not work well with very small files.
        /// </remarks>
        [WgetArgumentName("--limit-rate")]
        public WgetLimitRateArgument LimitRate { get; set; }

        /// <summary>
        /// Consider <emp>connection refused</emp> a transient error and try again.
        /// </summary>
        /// <remarks>
        /// Normally Wget gives up on a URL when it is unable to connect to the site because failure to connect is taken as a sign that the server is not running at all
        /// and that retries would not help. This option is for mirroring unreliable sites whose servers tend to disappear for short periods of time.
        /// </remarks>
        [WgetArgumentName("--retry-connrefused")]
        public bool RetryConnectionRefused { get; set; }

        /// <summary>
        /// Turn on recursive retrieving.
        /// </summary>
        /// <remarks>
        /// See <a href="https://www.gnu.org/software/wget/manual/wget.html#Recursive-Download">Recursive Download</a>, for more details. The default maximum depth is <c>5</c>.
        /// <seealso cref="Level"/>
        /// </remarks>
        [WgetArgumentName("--recursive")]
        public bool Recursive { get; set; }

        /// <summary>
        /// Specify recursion maximum depth level <c>depth</c> (see <a href="https://www.gnu.org/software/wget/manual/wget.html#Recursive-Download">Recursive Download</a>).
        /// <seealso cref="Recursive"/>
        /// </summary>
        [WgetArgumentName("--level")]
        public uint Level { get; set; }

        /// <summary>
        /// Wait the specified number of seconds between the retrievals. Use of this option is recommended, as it lightens the server load by making the requests less frequent.
        /// </summary>
        /// <remarks>
        /// Specifying a large value for this option is useful if the network or the destination host is down, so that Wget can wait long enough to reasonably expect the network error to be fixed before the retry.
        /// The waiting interval specified by this function is influenced by <seealso cref="RandomWait"/>.
        /// </remarks>
        [WgetArgumentName("--wait")]
        public TimeSpan? Wait { get; set; }

        /// <summary>
        /// If you don’t want Wget to wait between every retrieval, but only between retries of failed downloads, you can use this option.
        /// Wget will use linear back-off, waiting 1 second after the first failure on a given file, then waiting 2 seconds after the second failure on that file, up to the maximum number of <c>seconds</c> you specify.
        /// </summary>
        /// <remarks>
        /// By default, Wget will assume a value of <c>10</c> seconds.
        /// </remarks>
        [WgetArgumentName("--wait-retry")]
        public TimeSpan? WaitRetry { get; set; }

        /// <summary>
        /// Some web sites may perform log analysis to identify retrieval programs such as Wget by looking for statistically significant similarities in the time between requests.
        /// This option causes the time between requests to vary between 0.5 and 1.5 * <c>wait</c> seconds, where <c>wait</c> was specified using the <seealso cref="Wait"/> option, in order to mask Wget’s presence from such analysis.
        /// </summary>
        [WgetArgumentName("--random-wait")]
        public bool RandomWait { get; set; }

        /// <summary>
        /// Specify the username for both FTP and HTTP file retrieval.
        /// This parameter can be overridden using the <seealso cref="FtpUser"/> option for FTP connections and the <seealso cref="HttpUser"/> option for HTTP connections.
        /// </summary>
        [WgetArgumentName("--user")]
        public string User { get; set; }

        /// <summary>
        /// Specify the password for both FTP and HTTP file retrieval.
        /// This parameter can be overridden using the <seealso cref="FtpPassword"/> option for FTP connections and the <seealso cref="HttpPassword"/> option for HTTP connections.
        /// </summary>
        [WgetArgumentName("--password")]
        public string Password { get; set; }

        /// <summary>
        /// Specify the username on an HTTP server.
        /// According to the type of the challenge, Wget will encode them using either the <c>basic</c> (insecure), the <c>digest</c>, or the Windows <c>NTLM</c> authentication scheme.
        /// </summary>
        [WgetArgumentName("--http-user")]
        public string HttpUser { get; set; }

        /// <summary>
        /// Specify the password on an HTTP server.
        /// According to the type of the challenge, Wget will encode them using either the <c>basic</c> (insecure), the <c>digest</c>, or the Windows <c>NTLM</c> authentication scheme.
        /// </summary>
        [WgetArgumentName("--http-password")]
        public string HttpPassword { get; set; }

        /// <summary>
        /// Specify the username on an FTP server.
        /// Without this, or the corresponding startup option, the password defaults to <c>-wget@</c>, normally used for anonymous FTP.
        /// </summary>
        [WgetArgumentName("--ftp-user")]
        public string FtpUser { get; set; }

        /// <summary>
        /// Specify the password on an FTP server.
        /// Without this, or the corresponding startup option, the password defaults to <c>-wget@</c>, normally used for anonymous FTP.
        /// </summary>
        [WgetArgumentName("--ftp-password")]
        public string FtpPassword { get; set; }

        /// <summary>
        /// Continue getting a partially-downloaded file. This is useful when you want to finish up a download started by a previous instance of Wget, or by another program.
        /// </summary>
        /// <remarks>
        /// Note that you don’t need to specify this option if you just want the current invocation of Wget to retry downloading a file should the connection be lost midway through.
        /// This is the default behavior. <see cref="Continue"/> only affects resumption of downloads started prior to this invocation of Wget, and whose local files are still sitting around.
        /// </remarks>
        [WgetArgumentName("--continue")]
        public bool Continue { get; set; }

        /// <summary>
        /// Set the network timeout to <c>seconds seconds</c>.
        /// This is equivalent to specifying <seealso cref="DnsTimeout"/>, <seealso cref="ConnectTimeout"/> and <seealso cref="ReadTimeout"/>, all at the same time.
        /// </summary>
        /// <remarks>
        /// <para>When interacting with the network, Wget can check for timeout and abort the operation if it takes too long.
        /// This prevents anomalies like hanging reads and infinite connects. The only timeout enabled by default is a 900-second read timeout.
        /// Setting a timeout to <em>0</em> disables it altogether. Unless you know what you are doing, it is best not to change the default timeout settings.</para>
        /// <para>Setting a timeout to <c>0</c> disables it altogether. Unless you know what you are doing, it is best not to change the default timeout settings.</para>
        /// <para>All timeout-related options accept <see langword="decimal"/> values, as well as subsecond values. For example, <c>0.1</c> seconds is a legal (though unwise) choice of timeout.
        /// Subsecond timeouts are useful for checking server response times or for testing network latency.</para>
        /// <seealso cref="DnsTimeout"/>
        /// <seealso cref="ConnectTimeout"/>
        /// <seealso cref="ReadTimeout"/>
        /// </remarks>
        [WgetArgumentName("--timeout")]
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Set the DNS lookup timeout to <c>seconds</c> seconds. DNS lookups that don’t complete within the specified time will fail.
        /// By default, there is no timeout on DNS lookups, other than that implemented by system libraries.
        /// <seealso cref="Timeout"/>
        /// <seealso cref="ConnectTimeout"/>
        /// <seealso cref="ReadTimeout"/>
        /// </summary>
        [WgetArgumentName("--dns-timeout")]
        public TimeSpan? DnsTimeout { get; set; }

        /// <summary>
        /// Set the connect timeout to <c>seconds</c> seconds. TCP connections that take longer to establish will be aborted.
        /// By default, there is no connect timeout, other than that implemented by system libraries.
        /// <seealso cref="Timeout"/>
        /// <seealso cref="DnsTimeout"/>
        /// <seealso cref="ReadTimeout"/>
        /// </summary>
        [WgetArgumentName("--connect-timeout")]
        public TimeSpan? ConnectTimeout { get; set; }

        /// <summary>
        /// Set the read (and write) timeout to <c>seconds</c> seconds.
        /// The “time” of this timeout refers to idle time: if, at any point in the download, no data is received for more than the specified number of seconds, reading fails and the download is restarted.
        /// This option does not directly affect the duration of the entire download.
        /// <seealso cref="Timeout"/>
        /// <seealso cref="ConnectTimeout"/>
        /// <seealso cref="DnsTimeout"/>
        /// </summary>
        /// <remarks>
        /// Of course, the remote server may choose to terminate the connection sooner than this option requires. The default read timeout is <c>900</c> seconds.
        /// </remarks>
        [WgetArgumentName("--read-timeout")]
        public TimeSpan? ReadTimeout { get; set; }

        /// <summary>
        /// Go to background immediately after startup. If no output file is specified via the <seealso cref="OutputFile"/>, output is redirected to <c>wget-log</c>.
        /// </summary>
        [WgetArgumentName("--background")]
        public bool Background { get; set; }

        internal void Evaluate(ProcessArgumentBuilder arguments)
        {
            if (Help)
            {
                arguments.Append(this.GetArgumentName(nameof(Help)));
            }

            if (Version)
            {
                arguments.Append(this.GetArgumentName(nameof(Version)));
            }

            if (Url != default)
            {
                arguments.Append(Url.AbsoluteUri);
            }

            if (!string.IsNullOrWhiteSpace(OutputDocument))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(OutputDocument)), SwitchSeparator, OutputDocument);
            }

            if (!string.IsNullOrWhiteSpace(OutputFile))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(OutputFile)), SwitchSeparator, OutputFile);
            }

            if (!string.IsNullOrWhiteSpace(AppendOutput))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(AppendOutput)), SwitchSeparator, AppendOutput);
            }

            if (Quiet)
            {
                arguments.Append(this.GetArgumentName(nameof(Quiet)));
            }

            if (Debug)
            {
                arguments.Append(this.GetArgumentName(nameof(Debug)));
            }

            if (Verbose)
            {
                arguments.Append(this.GetArgumentName(nameof(Verbose)));
            }

            if (NoVerbose)
            {
                arguments.Append(this.GetArgumentName(nameof(NoVerbose)));
            }

            if (!string.IsNullOrWhiteSpace(InputFile))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(InputFile)), SwitchSeparator, InputFile);
            }

            if (ForceHtml)
            {
                arguments.Append(this.GetArgumentName(nameof(ForceHtml)));
            }

            if (!string.IsNullOrWhiteSpace(Base))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(Base)), SwitchSeparator, Base);
            }

            if (!string.IsNullOrWhiteSpace(RejectedLog))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(RejectedLog)), SwitchSeparator, RejectedLog);
            }

            if (Tries != default)
            {
                arguments.AppendSwitch(this.GetArgumentName(nameof(Tries)), SwitchSeparator, Tries.ToString());
            }

            if (!string.IsNullOrWhiteSpace(DirectoryPrefix))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(DirectoryPrefix)), SwitchSeparator, DirectoryPrefix);
            }

            if (LimitRate != null && LimitRate.Value > 0.0)
            {
                arguments.AppendSwitch(
                    this.GetArgumentName(nameof(LimitRate)),
                    SwitchSeparator,
                    LimitRate.GetFormattedLimitRateValue());
            }

            if (RetryConnectionRefused)
            {
                arguments.Append(this.GetArgumentName(nameof(RetryConnectionRefused)));
            }

            if (Recursive)
            {
                arguments.Append(this.GetArgumentName(nameof(Recursive)));
            }

            if (Level != default)
            {
                arguments.AppendSwitch(this.GetArgumentName(nameof(Level)), SwitchSeparator, Level.ToString());
            }

            if (Wait.HasValue)
            {
                arguments.AppendSwitch(
                    this.GetArgumentName(nameof(Wait)),
                    SwitchSeparator,
                    Wait.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }

            if (WaitRetry.HasValue)
            {
                arguments.AppendSwitch(
                    this.GetArgumentName(nameof(WaitRetry)),
                    SwitchSeparator,
                    WaitRetry.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }

            if (RandomWait)
            {
                arguments.Append(this.GetArgumentName(nameof(RandomWait)));
            }

            if (!string.IsNullOrWhiteSpace(User))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(User)), SwitchSeparator, User);
            }

            if (!string.IsNullOrWhiteSpace(Password))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(Password)), SwitchSeparator, new SecretArgument(new TextArgument(Password)));
            }

            if (!string.IsNullOrWhiteSpace(HttpUser))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(HttpUser)), SwitchSeparator, HttpUser);
            }

            if (!string.IsNullOrWhiteSpace(HttpPassword))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(HttpPassword)), SwitchSeparator, new SecretArgument(new TextArgument(HttpPassword)));
            }

            if (!string.IsNullOrWhiteSpace(FtpUser))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(FtpUser)), SwitchSeparator, FtpUser);
            }

            if (!string.IsNullOrWhiteSpace(FtpPassword))
            {
                arguments.AppendSwitchQuoted(this.GetArgumentName(nameof(FtpPassword)), SwitchSeparator, new SecretArgument(new TextArgument(FtpPassword)));
            }

            if (Continue)
            {
                arguments.Append(this.GetArgumentName(nameof(Continue)));
            }

            if (Timeout.HasValue)
            {
                arguments.AppendSwitch(
                    this.GetArgumentName(nameof(Timeout)),
                    SwitchSeparator,
                    Timeout.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }

            if (DnsTimeout.HasValue)
            {
                arguments.AppendSwitch(
                    this.GetArgumentName(nameof(DnsTimeout)),
                    SwitchSeparator,
                    DnsTimeout.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }

            if (ConnectTimeout.HasValue)
            {
                arguments.AppendSwitch(
                    this.GetArgumentName(nameof(ConnectTimeout)),
                    SwitchSeparator,
                    ConnectTimeout.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }

            if (ReadTimeout.HasValue)
            {
                arguments.AppendSwitch(
                    this.GetArgumentName(nameof(ReadTimeout)),
                    SwitchSeparator,
                    ReadTimeout.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }

            if (Background)
            {
                arguments.Append(this.GetArgumentName(nameof(Background)));
            }
        }
    }
}
