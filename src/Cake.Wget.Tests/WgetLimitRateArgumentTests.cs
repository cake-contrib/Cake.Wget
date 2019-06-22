using System;
using System.Linq;
using Xunit;

namespace Cake.Wget.Tests
{
    public class WgetLimitRateArgumentTests
    {
        [Fact]
        public void Should_Throw_If_LimitRate_Value_Is_Negative()
        {
            const int limitRate = -1;
            var limitRateArgument = Record.Exception(() => new WgetLimitRateArgument(limitRate));
            Assert.IsType<ArgumentOutOfRangeException>(limitRateArgument);
        }

        [Fact]
        public void Should_Throw_If_LimitRateUnit_Value_Is_Invalid()
        {
            int limitRateUnit = Enum.GetValues(typeof(LimitRateUnitEnum)).Cast<int>().Max() + 1;
            var argument = new WgetLimitRateArgument(100, (LimitRateUnitEnum)limitRateUnit);
            var limitRateArgument = Record.Exception(() => argument.GetFormattedLimitRateValue());
            Assert.IsType<NotImplementedException>(limitRateArgument);
        }

        [Fact]
        public void LimitRate_Enum_Unchanged()
        {
            var names = Enum.GetNames(typeof(LimitRateUnitEnum));
            Assert.Equal(3, names.Length);
            Assert.Equal("None", names[0]);
            Assert.Equal("Kilobytes", names[1]);
            Assert.Equal("Megabytes", names[2]);
        }

        [Theory]
        [InlineData(3, "3")]
        [InlineData(10.56, "10.56")]
        public void Should_Format_LimitRateUnit_Without_RateUnit(double limitRate, string expectedValue)
        {
            var argument = new WgetLimitRateArgument(limitRate);
            Assert.Equal(expectedValue, argument.GetFormattedLimitRateValue());
        }

        [Theory]
        [InlineData(1, LimitRateUnitEnum.None, "1")]
        [InlineData(1, LimitRateUnitEnum.Kilobytes, "1k")]
        [InlineData(1, LimitRateUnitEnum.Megabytes, "1m")]
        [InlineData(2.6, LimitRateUnitEnum.None, "2.6")]
        [InlineData(2.6, LimitRateUnitEnum.Kilobytes, "2.6k")]
        [InlineData(2.6, LimitRateUnitEnum.Megabytes, "2.6m")]
        [InlineData(1000.678, LimitRateUnitEnum.None, "1000.678")]
        [InlineData(1000.678, LimitRateUnitEnum.Kilobytes, "1000.678k")]
        [InlineData(1000.678, LimitRateUnitEnum.Megabytes, "1000.678m")]
        public void Should_Format_LimitRateUnit_With_RateUnit(
            double limitRate,
            LimitRateUnitEnum limitRateUnit,
            string expectedValue)
        {
            var argument = new WgetLimitRateArgument(limitRate, limitRateUnit);
            Assert.Equal(expectedValue, argument.GetFormattedLimitRateValue());
        }
    }
}
