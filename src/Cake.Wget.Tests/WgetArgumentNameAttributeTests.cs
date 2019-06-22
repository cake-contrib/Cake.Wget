using System;
using Xunit;

namespace Cake.Wget.Tests
{
    public class WgetArgumentNameAttributeTests
    {
        [Fact]
        public void Should_Throw_If_ArgumentName_Is_Null()
        {
            var wgetArgumentName = Record.Exception(() => new WgetArgumentNameAttribute(null));
            Assert.IsType<ArgumentNullException>(wgetArgumentName);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData(" --output-file ", "--output-file")]
        [InlineData("--quiet", "--quiet")]
        public void Should_Set_Name_Of_Wget_Argument(string name, string expectedValue)
        {
            var wgetArgumentName = new WgetArgumentNameAttribute(name);
            Assert.Equal(wgetArgumentName.Name, expectedValue);
        }
    }
}
