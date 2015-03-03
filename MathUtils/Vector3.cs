using System;
using System.Threading;

namespace MathUtils
{
    public class Vector3
    {
        public static int AllocationCount;

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3()
            : this(0, 0, 0)
        {
        }

        public Vector3(double x, double y, double z)
        {
            Interlocked.Increment(ref AllocationCount);

            X = x;
            Y = y;
            Z = z;
        }

        public double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }

        public double LengthSquared
        {
            get { return X*X + Y*Y + Z*Z; }
        }

        public void Normalize()
        {
            double length = Length;
            X /= length;
            Y /= length;
            Z /= length;
        }

        public static Vector3 operator -(Vector3 v1)
        {
            return new Vector3(-v1.X, -v1.Y, -v1.Z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator *(Vector3 v1, double d)
        {
            return new Vector3(v1.X*d, v1.Y*d, v1.Z*d);
        }

        public static Vector3 operator *(double d, Vector3 v1)
        {
            return new Vector3(v1.X * d, v1.Y * d, v1.Z * d);
        }

        public static Vector3 operator /(Vector3 v1, double d)
        {
            double inv = 1/d;
            return new Vector3(v1.X * inv, v1.Y * inv, v1.Z * inv);
        }

        public static double DotProduct(Vector3 v1, Vector3 v2)
        {
            return v1.X*v2.X + v1.Y*v2.Y + v1.Z*v2.Z;
        }

        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X);
        }

        // rotation Y-Z
        public static Vector3 Pitch(Vector3 v1, double angle) // in radians
        {
            double cs = Math.Cos(angle);
            double sn = Math.Sin(angle);
            double x = v1.X;
            double y = (v1.Y * cs) - (v1.Z * sn);
            double z = (v1.Y * sn) + (v1.Z * cs);
            return new Vector3(x, y, z);
        }

        // rotation X-Z
        public static Vector3 Yaw(Vector3 v1, double angle)
        {
            double cs = Math.Cos(angle);
            double sn = Math.Sin(angle);
            double x = (v1.Z * sn) + (v1.X * cs);
            double y = v1.Y;
            double z = (v1.Z * cs) - (v1.X * sn);
            return new Vector3(x, y, z);
        }

        // rotation X-Y
        public static Vector3 Roll(Vector3 v1, double angle)
        {
            double cs = Math.Cos(angle);
            double sn = Math.Sin(angle);
            double x = (v1.X * cs) - (v1.Y * sn);
            double y = (v1.X * sn) + (v1.Y * cs);
            double z = v1.Z;
            return new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return String.Format("{0:F6};{1:F6};{2:F6}", X, Y, Z);
        }
    }
}
