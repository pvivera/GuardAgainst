﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable ExpressionIsAlwaysNull

namespace GuardAgainstLib.Test
{
    public class Test_ArgumentBeingGreaterThanMaximum : TestBase
    {
        public Test_ArgumentBeingGreaterThanMaximum(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WhenArgumentIsEqualToMaximum_ShouldNotThrow()
        {
            var myArgument = "A";
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(myArgument, "A", nameof(myArgument), null,
                    new Dictionary<object, object> {{"a", "1"}});
            });
        }

        [Fact]
        public void WhenArgumentIsGreaterThanMaximum_ShouldThrowArgumentOutOfRangeException()
        {
            var myArgument = "B";
            var ex = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(myArgument, "A", nameof(myArgument), null,
                    new Dictionary<object, object> {{"a", "1"}});
            });

            ex.ParamName.ShouldBe(nameof(myArgument));
            ex.Data.Count.ShouldBe(1);
            ex.Data["a"].ShouldBe("1");
        }

        [Fact]
        public void WhenArgumentIsLessThanMaximum_ShouldNotBeSlow()
        {
            Benchmark.Do(WhenArgumentIsLessThanMaximum_ShouldNotThrow, 1000000, MethodBase.GetCurrentMethod().Name,
                Output);
        }

        [Fact]
        public void WhenArgumentIsLessThanMaximum_ShouldNotThrow()
        {
            var myArgument = "A";
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(myArgument, "B", nameof(myArgument), null,
                    new Dictionary<object, object> {{"a", "1"}});
            });
        }

        [Fact]
        public void WhenArgumentIsNull_ShouldNotBeSlow()
        {
            Benchmark.Do(WhenArgumentIsNull_ShouldNotThrow, 1000000, MethodBase.GetCurrentMethod().Name, Output);
        }

        [Fact]
        public void WhenArgumentIsNull_ShouldNotThrow()
        {
            var myArgument = default(string);
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(myArgument, "B", nameof(myArgument), null,
                    new Dictionary<object, object> {{"a", "1"}});
            });
        }

        [Fact]
        public void WhenMaximumValueIsNull_ShouldThrowArgumentOutOfRangeException()
        {
            var myArgument = "A";
            var ex = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(myArgument, null, nameof(myArgument), null,
                    new Dictionary<object, object> {{"a", "1"}});
            });

            ex.ParamName.ShouldBe("myArgument");
            ex.Data.Count.ShouldBe(1);
        }
    }
}
