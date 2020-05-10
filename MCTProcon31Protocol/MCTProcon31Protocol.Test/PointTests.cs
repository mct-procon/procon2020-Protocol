using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MCTProcon31Protocol.Test
{
    public class PointTests
    {
        [Fact]
        public void EqualTest()
        {
            Assert.True(new VelocityPoint(0, 0) == new VelocityPoint(0, 0));
            Assert.True(new VelocityPoint(2, 1) == new VelocityPoint(2, 1));
            Assert.False(new VelocityPoint(1, 2) == new VelocityPoint(2, 1));
        }
    }
}
