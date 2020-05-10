using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MCTProcon31Protocol;

namespace MCTProcon31Protocol.Test
{
    public class VelocityPointTests
    {
        [Fact]
        public void EqualTest()
        {
            Assert.True(new VelocityPoint(0, 0) == new VelocityPoint(0, 0));
            Assert.True(new VelocityPoint(2, 1) == new VelocityPoint(2, 1));
            Assert.False(new VelocityPoint(1, 2) == new VelocityPoint(2, 1));
            Assert.True(new VelocityPoint(-1, -2) == new VelocityPoint(-1, -2));
        }

        [Fact]
        public void AddTest()
        {
            Point x = new Point(1, 5);
            VelocityPoint y = new VelocityPoint(-1, -2);
            Assert.True((x + y) == new Point(0, 3));
        }
    }
}
