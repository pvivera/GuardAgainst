﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming
// ReSharper disable ExpressionIsAlwaysNull

namespace GuardAgainstLib.Test
{
    public class Test_ArgumentBeingEmpty_String : TestBase
    {
        public Test_ArgumentBeingEmpty_String(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WhenArgumentIsEmptyString_ShouldThrowArgumentException()
        {
            var myArgument = "";
            var ex = Should.Throw<ArgumentException>(() =>
            {
                GuardAgainst.ArgumentBeingEmpty(myArgument, nameof(myArgument), null,
                    new Dictionary<object, object> {{"a", "1"}});
            });

            ex.ParamName.ShouldBe(nameof(myArgument));
            ex.Data.Count.ShouldBe(1);
            ex.Data["a"].ShouldBe("1");
        }

        [Fact]
        public void WhenArgumentIsNotNullOrEmptyString_ShouldNotBeSlow()
        {
            Benchmark.Do(WhenArgumentIsNotNullOrEmptyString_ShouldNotThrow, 1000000, MethodBase.GetCurrentMethod().Name,
                Output);
        }

        [Fact]
        public void WhenArgumentIsNotNullOrEmptyString_ShouldNotThrow()
        {
            var myArgument = " blah ";
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingEmpty(myArgument, nameof(myArgument), null,
                    new Dictionary<object, object> {{"a", "1"}});
            });
        }

        [Fact]
        public void WhenArgumentIsNullString_ShouldNotBeSlow()
        {
            Benchmark.Do(WhenArgumentIsNullString_ShouldNotThrow, 1000000, MethodBase.GetCurrentMethod().Name, Output);
        }

        [Fact]
        public void WhenArgumentIsNullString_ShouldNotThrow()
        {
            var myArgument = default(string);
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingEmpty(myArgument, nameof(myArgument), null,
                    new Dictionary<object, object> {{"a", "1"}});
            });
        }
    }
}
