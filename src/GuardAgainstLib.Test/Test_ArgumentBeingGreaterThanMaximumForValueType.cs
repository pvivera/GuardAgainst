﻿using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace GuardAgainstLib.Test
{
    public class Test_ArgumentBeingGreaterThanMaximumForValueType : TestBase
    {
        public Test_ArgumentBeingGreaterThanMaximumForValueType(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WhenArgumentExpressionIsEqualToMaximum_ShouldNotThrow()
        {
            var myArgument = 1;
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(() => myArgument, 1, null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }

        [Fact]
        public void WhenArgumentExpressionIsGreaterThanMaximum_ShouldThrowArgumentOutOfRangeException()
        {
            var myArgument = 2;
            var ex = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(() => myArgument, 1, null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });

            ex.ParamName.ShouldBe(nameof(myArgument));
            ex.Data.Count.ShouldBe(1);
            ex.Data["a"].ShouldBe("1");
        }

        [Fact]
        public void WhenArgumentExpressionIsLessThanMaximum_ShouldNotThrow()
        {
            var myArgument = 1;
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(() => myArgument, 2, null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }

        [Fact]
        public void WhenArgumentIsEqualToMaximum_ShouldNotThrow()
        {
            var myArgument = 1;
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(myArgument, 2, nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }

        [Fact]
        public void WhenArgumentIsGreaterThanMaximum_ShouldThrowArgumentOutOfRangeException()
        {
            var myArgument = 2;
            var ex = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(myArgument, 1, nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });

            ex.ParamName.ShouldBe(nameof(myArgument));
            ex.Data.Count.ShouldBe(1);
            ex.Data["a"].ShouldBe("1");
        }

        [Fact]
        public void WhenArgumentIsLessThanMaximum_ShouldNotThrow()
        {
            var myArgument = 1;
            Should.NotThrow(() =>
            {
                GuardAgainst.ArgumentBeingGreaterThanMaximum(myArgument, 2, nameof(myArgument), null, new Dictionary<object, object>
                {
                    { "a", "1" }
                });
            });
        }
    }
}
