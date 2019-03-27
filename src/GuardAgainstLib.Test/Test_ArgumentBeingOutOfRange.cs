﻿using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace GuardAgainstLib.Test
{
    public class Test_ArgumentBeingOutOfRange : TestBase
    {
        public Test_ArgumentBeingOutOfRange(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WhenArgumentIsEqualToMaximum_ShouldNotThrow()
        {
            var myArgument = "D";
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }

        [Fact]
        public void WhenArgumentIsEqualToMinimum_ShouldNotThrow()
        {
            var myArgument = "B";
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }

        [Fact]
        public void WhenArgumentIsGreaterThanMaximum_ShouldThrowArgumentOutOfRangeException()
        {
            var myArgument = "E";
            var ex = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });

            ex.ParamName.ShouldBe(nameof(myArgument));
            ex.Data.Count.ShouldBe(1);
            ex.Data["a"].ShouldBe("1");
        }

        [Fact]
        public void WhenArgumentIsInRange_ShouldNotThrow()
        {
            var myArgument = "C";
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }

        [Fact]
        public void WhenArgumentIsLessThanMinimum_ShouldThrowArgumentOutOfRangeException()
        {
            var myArgument = "A";
            var ex = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });

            ex.ParamName.ShouldBe(nameof(myArgument));
            ex.Data.Count.ShouldBe(1);
            ex.Data["a"].ShouldBe("1");
        }

        [Fact]
        public void WhenArgumentValueIsNull_ShouldNotThrow()
        {
            var myArgument = default(string);
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }
        
        [Fact]
        public void WhenMaximumValueIsNull_ShouldThrowArgumentNullException()
        {
            var myArgument = "A";
            var ex = Should.Throw<ArgumentNullException>(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", null, nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });

            ex.ParamName.ShouldBe("maximumAllowedValue");
            ex.Data.Count.ShouldBe(1);
        }

        [Fact]
        public void WhenMinimumValueIsNull_ShouldThrowArgumentNullException()
        {
            var myArgument = "A";
            var ex = Should.Throw<ArgumentNullException>(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, null, "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });

            ex.ParamName.ShouldBe("minimumAllowedValue");
            ex.Data.Count.ShouldBe(1);
        }

        [Fact]
        public void WhenArgumentIsEqualToMaximum_ShouldNotBeSlow()
        {
            var myArgument = "D";
            Should.CompleteIn(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            }, TimeSpan.FromMilliseconds(1));
        }

        [Fact]
        public void WhenArgumentIsEqualToMinimum_ShouldNotBeSlow()
        {
            var myArgument = "B";
            Should.CompleteIn(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            }, TimeSpan.FromMilliseconds(1));
        }

        [Fact]
        public void WhenArgumentIsInRange_ShouldNotBeSlow()
        {
            var myArgument = "C";
            Should.CompleteIn(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            }, TimeSpan.FromMilliseconds(1));
        }

        [Fact]
        public void WhenArgumentValueIsNull_ShouldNotBeSlow()
        {
            var myArgument = default(string);
            Should.CompleteIn(() =>
            {
                GuardAgainst.ArgumentBeingOutOfRange(myArgument, "B", "D", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            }, TimeSpan.FromMilliseconds(1));
        }
    }
}
