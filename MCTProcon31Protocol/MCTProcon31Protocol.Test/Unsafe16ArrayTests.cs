using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MCTProcon31Protocol.Test
{
    public class Unsafe16ArrayTests
    {
        [Fact]
        public void PutAndGetPointTest()
        {
            Unsafe16Array<Point> x = new Unsafe16Array<Point>();
            x[0] = new Point(1, 2);
            x[1] = new Point(3, 4);
            x[2] = new Point(5, 6);
            x[3] = new Point(7, 8);
            x[4] = new Point(9, 10);
            x[5] = new Point(11, 12);
            x[6] = new Point(13, 14);
            x[7] = new Point(15, 16);
            x[8] = new Point(17, 18);
            x[9] = new Point(19, 20);
            x[10] = new Point(21, 22);
            x[11] = new Point(23, 24);
            x[12] = new Point(25, 26);
            x[13] = new Point(27, 28);
            x[14] = new Point(29, 30);
            x[15] = new Point(31, 32);
            Assert.True(x[0] == new Point(1, 2));
            Assert.True(x[1] == new Point(3, 4));
            Assert.True(x[2] == new Point(5, 6));
            Assert.True(x[3] == new Point(7, 8));
            Assert.True(x[4] == new Point(9, 10));
            Assert.True(x[5] == new Point(11, 12));
            Assert.True(x[6] == new Point(13, 14));
            Assert.True(x[7] == new Point(15, 16));
            Assert.True(x[8] == new Point(17, 18));
            Assert.True(x[9] == new Point(19, 20));
            Assert.True(x[10] == new Point(21, 22));
            Assert.True(x[11] == new Point(23, 24));
            Assert.True(x[12] == new Point(25, 26));
            Assert.True(x[13] == new Point(27, 28));
            Assert.True(x[14] == new Point(29, 30));
            Assert.True(x[15] == new Point(31, 32));
        }

        [Fact]
        public void PutAndGetVelocityPointTest()
        {
            Unsafe16Array<VelocityPoint> x = new Unsafe16Array<VelocityPoint>();
            x[0] = new VelocityPoint(1, 2);
            x[1] = new VelocityPoint(3, 4);
            x[2] = new VelocityPoint(5, 6);
            x[3] = new VelocityPoint(7, 8);
            x[4] = new VelocityPoint(-1, -2);
            x[5] = new VelocityPoint(-3, -4);
            x[6] = new VelocityPoint(-5, -6);
            x[7] = new VelocityPoint(-7, -8);
            Assert.True(x[0] == new VelocityPoint(1, 2));
            Assert.True(x[1] == new VelocityPoint(3, 4));
            Assert.True(x[2] == new VelocityPoint(5, 6));
            Assert.True(x[3] == new VelocityPoint(7, 8));
            Assert.True(x[4] == new VelocityPoint(-1, -2));
            Assert.True(x[5] == new VelocityPoint(-3, -4));
            Assert.True(x[6] == new VelocityPoint(-5, -6));
            Assert.True(x[7] == new VelocityPoint(-7, -8));
        }

        [Fact]
        public void EqualPoint3Test()
        {
            Unsafe16Array<Point> x = Unsafe16Array<Point>.Create(new Point[]{ new Point(0,1), new Point(2,3), new Point(4,5), new Point(6, 7)});
            Unsafe16Array<Point> y = Unsafe16Array<Point>.Create(new Point[]{ new Point(0,1), new Point(2,3), new Point(4,5), new Point(8, 9)});
            Assert.True(Unsafe16Array<Point>.Equals(x, y, 3));
            y[2] = new Point(10, 11);
            Assert.False(Unsafe16Array<Point>.Equals(x, y, 3));
        }

        [Fact]
        public void EqualPoint5Test()
        {
            Unsafe16Array<Point> x = Unsafe16Array<Point>.Create(new Point[] { new Point(0, 1), new Point(2, 3), new Point(4, 5), new Point(6, 7), new Point(8, 9)});
            Unsafe16Array<Point> y = Unsafe16Array<Point>.Create(new Point[] { new Point(0, 1), new Point(2, 3), new Point(4, 5), new Point(6, 7), new Point(8, 9)});
            Assert.True(Unsafe16Array<Point>.Equals(x, y, 5));
            y[2] = new Point(10, 11);
            Assert.False(Unsafe16Array<Point>.Equals(x, y, 5));
            y[2] = new Point(4, 5);
            y[4] = new Point(4, 5);
            Assert.False(Unsafe16Array<Point>.Equals(x, y, 5));
        }

        [Fact]
        public void EqualPoint16Test()
        {
            var points = Enumerable.Range(0, 8).Select(i => new Point((byte)(i * 2), (byte)(i * 2 + 1))).ToArray();
            Unsafe16Array<Point> x = Unsafe16Array<Point>.Create(points);
            Unsafe16Array<Point> y = Unsafe16Array<Point>.Create(points);
            Assert.True(Unsafe16Array<Point>.Equals(x, y, 16));
            y[2] = new Point(10, 11);
            Assert.False(Unsafe16Array<Point>.Equals(x, y, 16));
            y[2] = new Point(4, 5);
            Assert.True(Unsafe16Array<Point>.Equals(x, y, 16));
            y[14] = new Point(4, 5);
            Assert.False(Unsafe16Array<Point>.Equals(x, y, 16));
        }
    }
}
