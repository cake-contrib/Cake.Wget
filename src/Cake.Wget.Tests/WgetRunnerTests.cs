using System;
using System.Globalization;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Wget.Tests
{
    public class WgetRunnerTests : IDisposable
    {
        const string FakeUrl = "http://fake.url";
        WgetFixture _fixture;

        public WgetRunnerTests()
        {
            _fixture = new WgetFixture
            {
                Settings = new WgetSettings
                {
                    Url = new Uri(FakeUrl),
                },
            };
        }

        public void Dispose()
        {
            _fixture = null;
        }

        [Fact]
        public void Should_Throw_If_Wget_Not_Found()
        {
            _fixture.GivenDefaultToolDoNotExist();
            var result = Record.Exception(() => _fixture.Run());
            Assert.IsType<CakeException>(result);
            Assert.Equal("wget: Could not locate executable.", result.Message);
        }

        [Theory]
        [InlineData("/bin/wget", "/bin/wget")]
        [InlineData("./tools/wget", "/Working/tools/wget")]
        public void Should_Use_Wget_From_Tool_Path_If_Provided(string toolPath, string expectedPath)
        {
            _fixture.Settings.ToolPath = toolPath;
            _fixture.GivenSettingsToolPathExist();
            var result = _fixture.Run();
            Assert.Equal(expectedPath, result.Path.FullPath);
        }

        [Fact]
        public void Should_Throw_If_Settings_Is_Null()
        {
            _fixture.Settings = null;
            var result = Record.Exception(() => _fixture.Run());
            Assert.IsType<ArgumentNullException>(result);
            Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
        }

        [Fact]
        public void Should_Throw_If_Url_And_InputFile_Is_Null()
        {
            _fixture.Settings.Url = null;
            _fixture.Settings.InputFile = null;
            var result = Record.Exception(() => _fixture.Run());
            Assert.IsType<ArgumentException>(result);
            Assert.Equal(Messages.InputParametersError, ((ArgumentException)result).Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Throw_If_Url_Is_Null_And_InputFile_Is_Empty(string inputFile)
        {
            _fixture.Settings.Url = null;
            _fixture.Settings.InputFile = inputFile;
            var result = Record.Exception(() => _fixture.Run());
            Assert.IsType<ArgumentException>(result);
            Assert.Equal(Messages.InputParametersError, ((ArgumentException)result).Message);
        }

        [Fact]
        public void Should_Set_Help_Switch_As_Argument()
        {
            _fixture.Settings.Help = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Help));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_Help_Switch_As_Argument()
        {
            _fixture.Settings.Help = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Help));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Version_Switch_As_Argument()
        {
            _fixture.Settings.Version = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Version));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_Version_Switch_As_Argument()
        {
            _fixture.Settings.Version = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Version));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Url_As_Argument()
        {
            var result = _fixture.Run();
            Assert.Contains(FakeUrl, result.Args);
        }

        [Fact]
        public void Should_Set_OutputDocument_As_Argument()
        {
            const string output = "output.txt";
            _fixture.Settings.OutputDocument = output;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.OutputDocument));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{output}\"", result.Args);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Not_Set_Empty_OutputDocument_As_Argument(string output)
        {
            _fixture.Settings.OutputDocument = output;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.OutputDocument));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_OutputFile_As_Argument()
        {
            const string logFile = "logfile.txt";
            _fixture.Settings.OutputFile = logFile;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.OutputFile));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{logFile}\"", result.Args);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Not_Set_Empty_OutputFile_As_Argument(string logFile)
        {
            _fixture.Settings.OutputFile = logFile;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.OutputFile));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_AppendOutput_As_Argument()
        {
            const string appendOutput = "logfile.txt";
            _fixture.Settings.AppendOutput = appendOutput;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.AppendOutput));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{appendOutput}\"", result.Args);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Not_Set_Empty_AppendOutput_As_Argument(string appendOutput)
        {
            _fixture.Settings.AppendOutput = appendOutput;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.AppendOutput));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Quiet_Switch_As_Argument()
        {
            _fixture.Settings.Quiet = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Quiet));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_Quiet_Switch_As_Argument()
        {
            _fixture.Settings.Quiet = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Quiet));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Debug_Switch_As_Argument()
        {
            _fixture.Settings.Debug = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Debug));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_Debug_Switch_As_Argument()
        {
            _fixture.Settings.Debug = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Debug));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Verbose_Switch_As_Argument()
        {
            _fixture.Settings.Verbose = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Verbose));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_Verbose_Switch_As_Argument()
        {
            _fixture.Settings.Verbose = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Verbose));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_NoVerbose_Switch_As_Argument()
        {
            _fixture.Settings.NoVerbose = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.NoVerbose));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_NoVerbose_Switch_As_Argument()
        {
            _fixture.Settings.NoVerbose = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.NoVerbose));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_InputFile_As_Argument()
        {
            const string inputFile = "InputFile.txt";
            _fixture.Settings.InputFile = inputFile;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.InputFile));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{inputFile}\"", result.Args);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Not_Set_Empty_InputFile_As_Argument(string inputFile)
        {
            _fixture.Settings.InputFile = inputFile;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.InputFile));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_ForceHtml_Switch_As_Argument()
        {
            _fixture.Settings.ForceHtml = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.ForceHtml));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_ForceHtml_Switch_As_Argument()
        {
            _fixture.Settings.ForceHtml = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.ForceHtml));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Base_As_Argument()
        {
            const string baseUrl = "http://foo/baz";
            _fixture.Settings.Base = baseUrl;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Base));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{baseUrl}\"", result.Args);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Not_Set_Empty_Base_As_Argument(string baseUrl)
        {
            _fixture.Settings.Base = baseUrl;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Base));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_RejectedLog_As_Argument()
        {
            const string rejectedLog = "rejected.txt";
            _fixture.Settings.RejectedLog = rejectedLog;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.RejectedLog));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{rejectedLog}\"", result.Args);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Not_Set_Empty_RejectedLog_As_Argument(string rejectedLog)
        {
            _fixture.Settings.RejectedLog = rejectedLog;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.RejectedLog));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Tries_Switch_As_Argument()
        {
            var tries = 10u;
            _fixture.Settings.Tries = tries;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Tries));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}{tries}", result.Args);
        }

        [Fact]
        public void Should_Not_Set_Tries_Switch_As_Argument()
        {
            _fixture.Settings.Tries = 0;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Tries));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_DirectoryPrefix_As_Argument()
        {
            const string directoryPrefix = "/opt/download";
            _fixture.Settings.DirectoryPrefix = directoryPrefix;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.DirectoryPrefix));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{directoryPrefix}\"", result.Args);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Not_Set_DirectoryPrefix_As_Argument(string directoryPrefix)
        {
            _fixture.Settings.DirectoryPrefix = directoryPrefix;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.DirectoryPrefix));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Theory]
        [InlineData(1, LimitRateUnitEnum.None)]
        [InlineData(1, LimitRateUnitEnum.Kilobytes)]
        [InlineData(1, LimitRateUnitEnum.Megabytes)]
        [InlineData(2.6, LimitRateUnitEnum.None)]
        [InlineData(2.6, LimitRateUnitEnum.Kilobytes)]
        [InlineData(2.6, LimitRateUnitEnum.Megabytes)]
        public void Should_Set_LimitRate_As_Argument(double limitRate, LimitRateUnitEnum limitRateUnit)
        {
            var rate = new WgetLimitRateArgument(limitRate, limitRateUnit);
            _fixture.Settings.LimitRate = rate;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.LimitRate));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}{rate.GetFormattedLimitRateValue()}", result.Args);
        }

        [Fact]
        public void Should_NotSet_LimitRate_Null_As_Argument()
        {
            _fixture.Settings.LimitRate = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.LimitRate));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_NotSet_LimitRate_Zero_As_Argument()
        {
            _fixture.Settings.LimitRate = new WgetLimitRateArgument(0.0);
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.LimitRate));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_RetryConnRefused_Switch_As_Argument()
        {
            _fixture.Settings.RetryConnectionRefused = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.RetryConnectionRefused));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_RetryConnRefused_Switch_As_Argument()
        {
            _fixture.Settings.RetryConnectionRefused = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.RetryConnectionRefused));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Recursive_Switch_As_Argument()
        {
            _fixture.Settings.Recursive = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Recursive));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_Recursive_Switch_As_Argument()
        {
            _fixture.Settings.Recursive = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Recursive));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void Should_Set_Level_As_Argument(uint recursiveLevel)
        {
            _fixture.Settings.Level = recursiveLevel;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Level));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}{recursiveLevel.ToString()}", result.Args);
        }

        [Fact]
        public void Should_Not_Set_Level_As_Argument()
        {
            _fixture.Settings.Level = 0;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Level));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Wait_As_Argument()
        {
            var wait = TimeSpan.FromSeconds(60.7);
            _fixture.Settings.Wait = wait;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Wait));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}{wait.TotalSeconds.ToString(CultureInfo.InvariantCulture)}", result.Args);
        }

        [Fact]
        public void Should_Not_Set_Wait_As_Argument()
        {
            _fixture.Settings.Wait = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Wait));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_WaitRetry_As_Argument()
        {
            var waitRetry = TimeSpan.FromSeconds(10.4);
            _fixture.Settings.WaitRetry = waitRetry;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.WaitRetry));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}{waitRetry.TotalSeconds.ToString(CultureInfo.InvariantCulture)}", result.Args);
        }

        [Fact]
        public void Should_Not_Set_WaitRetry_As_Argument()
        {
            _fixture.Settings.WaitRetry = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.WaitRetry));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_RandomWait_Switch_As_Argument()
        {
            _fixture.Settings.RandomWait = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.RandomWait));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_RandomWait_Switch_As_Argument()
        {
            _fixture.Settings.RandomWait = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.RandomWait));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_User_Switch_As_Argument()
        {
            var user = "user";
            _fixture.Settings.User = user;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.User));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{user}\"", result.Args);
        }

        [Fact]
        public void Should_Not_Set_User_Switch_As_Argument()
        {
            _fixture.Settings.User = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.User));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Password_Switch_As_Argument()
        {
            var password = "password";
            _fixture.Settings.Password = password;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Password));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{password}\"", result.Args);
        }

        [Fact]
        public void Should_Not_Set_Password_Switch_As_Argument()
        {
            _fixture.Settings.Password = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Password));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_HttpUser_Switch_As_Argument()
        {
            var httpUser = "http-user";
            _fixture.Settings.HttpUser = httpUser;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.HttpUser));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{httpUser}\"", result.Args);
        }

        [Fact]
        public void Should_Not_Set_HttpUser_Switch_As_Argument()
        {
            _fixture.Settings.HttpUser = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.HttpUser));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_HttpPassword_Switch_As_Argument()
        {
            var httpPassword = "http-password";
            _fixture.Settings.HttpPassword = httpPassword;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.HttpPassword));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{httpPassword}\"", result.Args);
        }

        [Fact]
        public void Should_Not_Set_HttpPassword_Switch_As_Argument()
        {
            _fixture.Settings.HttpPassword = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.HttpPassword));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_FtpUser_Switch_As_Argument()
        {
            var ftpUser = "ftp-user";
            _fixture.Settings.FtpUser = ftpUser;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.FtpUser));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{ftpUser}\"", result.Args);
        }

        [Fact]
        public void Should_Not_Set_FtpUser_Switch_As_Argument()
        {
            _fixture.Settings.FtpUser = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.FtpUser));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_FtpPassword_Switch_As_Argument()
        {
            var ftpPassword = "ftp-password";
            _fixture.Settings.FtpPassword = ftpPassword;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.FtpPassword));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}\"{ftpPassword}\"", result.Args);
        }

        [Fact]
        public void Should_Not_Set_FtpPassword_Switch_As_Argument()
        {
            _fixture.Settings.FtpPassword = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.FtpPassword));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Continue_Switch_As_Argument()
        {
            _fixture.Settings.Continue = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Continue));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_Continue_Switch_As_Argument()
        {
            _fixture.Settings.Continue = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Continue));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Timeout_As_Argument()
        {
            var timeout = TimeSpan.FromSeconds(60.7);
            _fixture.Settings.Timeout = timeout;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Timeout));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}{timeout.TotalSeconds.ToString(CultureInfo.InvariantCulture)}", result.Args);
        }

        [Fact]
        public void Should_Not_Set_Timeout_As_Argument()
        {
            _fixture.Settings.Timeout = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Timeout));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_DnsTimeout_As_Argument()
        {
            var dnsTimeout = TimeSpan.FromSeconds(30.3);
            _fixture.Settings.DnsTimeout = dnsTimeout;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.DnsTimeout));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}{dnsTimeout.TotalSeconds.ToString(CultureInfo.InvariantCulture)}", result.Args);
        }

        [Fact]
        public void Should_Not_Set_DnsTimeout_As_Argument()
        {
            _fixture.Settings.DnsTimeout = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.DnsTimeout));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_ConnectTimeout_As_Argument()
        {
            var connectTimeout = TimeSpan.FromSeconds(30.3);
            _fixture.Settings.ConnectTimeout = connectTimeout;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.ConnectTimeout));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}{connectTimeout.TotalSeconds.ToString(CultureInfo.InvariantCulture)}", result.Args);
        }

        [Fact]
        public void Should_Not_Set_ConnectTimeout_As_Argument()
        {
            _fixture.Settings.ConnectTimeout = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.ConnectTimeout));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_ReadTimeout_As_Argument()
        {
            var readTimeout = TimeSpan.FromSeconds(30.3);
            _fixture.Settings.ReadTimeout = readTimeout;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.ReadTimeout));
            Assert.Contains($"{expectedSwitchName}{WgetSettings.SwitchSeparator}{readTimeout.TotalSeconds.ToString(CultureInfo.InvariantCulture)}", result.Args);
        }

        [Fact]
        public void Should_Not_Set_ReadTimeout_As_Argument()
        {
            _fixture.Settings.ReadTimeout = null;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.ReadTimeout));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Set_Background_Switch_As_Argument()
        {
            _fixture.Settings.Background = true;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Background));
            Assert.Contains(expectedSwitchName, result.Args);
        }

        [Fact]
        public void Should_Not_Set_Background_Switch_As_Argument()
        {
            _fixture.Settings.Background = false;
            var result = _fixture.Run();
            var expectedSwitchName = _fixture.Settings.GetArgumentName(nameof(WgetSettings.Background));
            Assert.DoesNotContain(expectedSwitchName, result.Args);
        }
    }
}
