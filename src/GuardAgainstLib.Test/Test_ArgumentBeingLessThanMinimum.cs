﻿using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace GuardAgainstLib.Test
{
    public class Test_ArgumentBeingLessThanMinimum : TestBase
    {
        public Test_ArgumentBeingLessThanMinimum(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void WhenArgumentIsEqualToMinimum_ShouldNotThrow()
        {
            var myArgument = "A";
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingLessThanMinimum(myArgument, "A", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }

        [Fact]
        public void WhenArgumentIsGreaterThanMinimum_ShouldNotThrow()
        {
            var myArgument = "B";
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingLessThanMinimum(myArgument, "A", nameof(myArgument), null, new Dictionary<object, object>
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
                GuardAgainst.ArgumentBeingLessThanMinimum(myArgument, "B", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });

            ex.ParamName.ShouldBe(nameof(myArgument));
            ex.Data.Count.ShouldBe(1);
            ex.Data["a"].ShouldBe("1");
        }

        [Fact]
        public void WhenArgumentIsNull_ShouldNotThrowException()
        {
            var myArgument = default(string);
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingLessThanMinimum(myArgument, "B", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }

        [Fact]
        public void WhenMinimumValueIsNull_ShouldThrowArgumentNullException()
        {
            var myArgument = "A";
            var ex = Should.Throw<ArgumentNullException>(() =>
            {
                GuardAgainst.ArgumentBeingLessThanMinimum(myArgument, null, nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });

            ex.ParamName.ShouldBe("minimumAllowedValue");
            ex.Data.Count.ShouldBe(1);
        }

        [Fact]
        public void WhenArgumentIsEqualToMinimum_ShouldNotBeSlow()
        {
            var myArgument = "A";
            Should.CompleteIn(() =>
            {
                GuardAgainst.ArgumentBeingLessThanMinimum(myArgument, "A", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            }, TimeSpan.FromMilliseconds(1));
        }

        [Fact]
        public void WhenArgumentIsGreaterThanMinimum_ShouldNotBeSlow()
        {
            var myArgument = "B";
            Should.CompleteIn(() =>
            {
                GuardAgainst.ArgumentBeingLessThanMinimum(myArgument, "A", nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            }, TimeSpan.FromMilliseconds(1));
        }
    }
}
