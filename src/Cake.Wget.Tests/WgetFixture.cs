using Cake.Testing.Fixtures;

namespace Cake.Wget.Tests
{
    public class WgetFixture : ToolFixture<WgetSettings>
    {
        public WgetFixture()
            : base("wget")
        {
        }

        protected override void RunTool()
        {
            var tool = new WgetRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Settings);
        }
    }
}
