using System;
using OOGLibrary.IO;

namespace OOGLibrary.GameTypes
{
    class Point3F
    {
        public Single X { get; set; }
        public Single Z { get; set; }
        public Single Y { get; set; }

        public Point3F() { }

        public Point3F(float x, float z, float y)
        {
            X = x;
            Z = z;
            Y = y;
        }

        public Point3F(Point3F p)
        {
            X = p.X;
            Z = p.Z;
            Y = p.Y;
        }
        public DataStream Serialize(DataStream ds)
        {
            return Serialize(ds, false);
        }
        public DataStream Serialize(DataStream ds, bool swap)
        {
            ds.WriteFloat(X, swap);
            ds.WriteFloat(Z, swap);
            ds.WriteFloat(Y, swap);

            return ds;
        }

        public static Point3F operator +(Point3F p1, Point3F p2)
        {
            return new Point3F(p1.X + p2.X, p1.Z + p2.Z, p1.Y + p2.Y);
            
        }

        public static Point3F operator +(Point3F p1, float summand)
        {
            return new Point3F(p1.X + summand, p1.Z + summand, p1.Y + summand);
        }

        public static Point3F operator +(Point3F p1, double summand)
        {
            float summandF = (float)summand;
            return new Point3F(p1.X + summandF, p1.Z + summandF, p1.Y + summandF);
        }

        public static Point3F operator -(Point3F p1, Point3F p2)
        {
            return new Point3F(p1.X - p2.X, p1.Z - p2.Z, p1.Y - p2.Y);
        }

        public static Point3F operator -(Point3F p1, float subtrahend)
        {
            return new Point3F(p1.X - subtrahend, p1.Z - subtrahend, p1.Y - subtrahend);
        }

        public static Point3F operator -(Point3F p1, double subtrahend)
        {
            float subtrahendF = (float)subtrahend;
            return new Point3F(p1.X - subtrahendF, p1.Z - subtrahendF, p1.Y - subtrahendF);
        }

        public static Point3F operator *(Point3F p1, float multiplier)
        {
            return new Point3F(p1.X * multiplier, p1.Z * multiplier, p1.Y * multiplier);
        }

        public static Point3F operator /(Point3F p1, float divider)
        {
            return new Point3F(p1.X / divider, p1.Z / divider, p1.Y / divider);
        }

        public Double Distance2D(Point3F destPoint)
        {
            double dx = destPoint.X - X;
            double dy = destPoint.Y - Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public Double Distance3D(Point3F destPoint)
        {
            double dx = destPoint.X - X;
            double dy = destPoint.Y - Y;
            double dz = destPoint.Z - Z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public override string ToString()
        {
            return String.Format("X: {0:0.00} Y: {1:0.00} Z: {2:0.00}", X, Y, Z);
        }
    }
}
