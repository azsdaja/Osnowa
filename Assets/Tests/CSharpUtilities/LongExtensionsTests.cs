namespace Tests.CSharpUtilities
{
    using FluentAssertions;
    using NUnit.Framework;
    using Osnowa.Osnowa.Core.CSharpUtilities;

    [TestFixture]
    public class LongExtensionsTests
    {
        [TestCase((ulong)0b1100, (ulong)0b0010, true,  (ulong)0b1110)]
        [TestCase((ulong)0b1100, (ulong)0b0010, false, (ulong)0b1100)]
        [TestCase((ulong)0b1110, (ulong)0b0010, true,  (ulong)0b1110)]
        [TestCase((ulong)0b1110, (ulong)0b0010, false, (ulong)0b1100)]
        public void WithFlagSet_ReturnsCorrectResult(ulong input, ulong flag, bool flagValue, ulong expectedResult)
        {
            ulong result = input.WithFlagSet(flag, flagValue);

            result.Should().Be(expectedResult);
        }
        
        [TestCase((ulong)0b1100, (ulong)0b0010, false)]
        [TestCase((ulong)0b1100, (ulong)0b0100, true)]
        [TestCase((ulong)0b1100, (ulong)0b0110, false)]
        [TestCase((ulong)0b1100, (ulong)0b1100, true)]
        public void HasFlag_ReturnsCorrectResult(ulong input, ulong flagChecked, bool expectedResult)
        {
            bool result = input.HasFlag(flagChecked);

            result.Should().Be(expectedResult);
        }
    }
}