using System;
using Shouldly;
using Xunit;

namespace GuardAgainstLib.Test
{
    public class Test_ArgumentBeingWhitespace
    {
        [Theory]
        [InlineData(default(string), "myVal", "Argh!")]
        [InlineData(default(string), "myVal", "")]
        [InlineData(default(string), "myVal", "   ")]
        [InlineData(default(string), "myVal", null)]
        [InlineData(default(string), "", "")]
        [InlineData(default(string), null, "")]
        [InlineData(default(string), null, "   ")]
        [InlineData(default(string), "", null)]
        [InlineData(default(string), "   ", null)]
        [InlineData(default(string), null, null)]
        public void WhenArgumentIsNull_ShouldNotThrowException(string arg,
                                                               string argName,
                                                               string msg)
        {
            Should.NotThrow(() => { GuardAgainst.ArgumentBeingWhitespace(arg, argName, msg); });
        }

        [Theory]
        [InlineData("", "myVal", "Argh!")]
        [InlineData("", "myVal", "")]
        [InlineData("", "myVal", "   ")]
        [InlineData("", "myVal", null)]
        [InlineData("", "", "")]
        [InlineData("", null, "")]
        [InlineData("", null, "   ")]
        [InlineData("", "", null)]
        [InlineData("", "   ", null)]
        [InlineData("", null, null)]
        public void WhenArgumentIsEmpty_ShouldThrowArgumentException(string arg,
                                                                     string argName,
                                                                     string msg)
        {
            var ex = Should.Throw<ArgumentException>(() =>
            {
                GuardAgainst.ArgumentBeingWhitespace(arg, argName, msg);
            });

            ex.ParamName.ShouldBe(argName.NullIfWhitespace());
            ex.Message.ShouldContain(msg.NullIfWhitespace() ?? "Exception");
        }

        [Theory]
        [InlineData("   ", "myVal", "Argh!")]
        [InlineData("   ", "myVal", "")]
        [InlineData("   ", "myVal", "   ")]
        [InlineData("   ", "myVal", null)]
        [InlineData("   ", "", "")]
        [InlineData("   ", null, "")]
        [InlineData("   ", null, "   ")]
        [InlineData("   ", "", null)]
        [InlineData("   ", "   ", null)]
        [InlineData("   ", null, null)]
        public void WhenArgumentIsWhitespace_ShouldThrowArgumentException(string arg,
                                                                          string argName,
                                                                          string msg)
        {
            var ex = Should.Throw<ArgumentException>(() =>
            {
                GuardAgainst.ArgumentBeingWhitespace(arg, argName, msg);
            });

            ex.ParamName.ShouldBe(argName.NullIfWhitespace());
            ex.Message.ShouldContain(msg.NullIfWhitespace() ?? "Exception");
        }

        [Theory]
        [InlineData(" a ", "myVal", "Argh!")]
        [InlineData(" a ", "myVal", "")]
        [InlineData(" a ", "myVal", "   ")]
        [InlineData(" a ", "myVal", null)]
        [InlineData(" a ", "", "")]
        [InlineData(" a ", null, "")]
        [InlineData(" a ", null, "   ")]
        [InlineData(" a ", "", null)]
        [InlineData(" a ", "   ", null)]
        [InlineData(" a ", null, null)]
        public void WhenArgumentIsNotWhitespace_ShouldNotThrowException(string arg,
                                                                        string argName,
                                                                        string msg)
        {
            Should.NotThrow(() => { GuardAgainst.ArgumentBeingWhitespace(arg, argName, msg); });
        }
    }
}