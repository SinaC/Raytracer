using System;
using MathUtils;

namespace RayTracer.Pigments
{
    public class Checkboard : Pigment
    {
        private const double Tolerance = 0.0001; // used to compensate Math.Floor rounding error

        public enum Axes
        {
            Cubic,
            XY,
            XZ,
            YZ,
        }

        public Color Color1 { get; private set; }
        public Color Color2 { get; private set; }
        public double TileSize { get; private set; }
        public Axes Axis { get; private set; }

        private readonly Func<Vector3, bool> _computeFunc;

        public Checkboard(double tileSize = 1, Axes axis = Axes.Cubic)
            : this(new Color(1, 1, 1), new Color(0, 0, 0), tileSize, axis)
        {
        }

        public Checkboard(Color color1, Color color2, double tileSize = 1, Axes axis = Axes.Cubic)
        {
            Color1 = color1;
            Color2 = color2;
            TileSize = tileSize;
            Axis = axis;

            switch (axis)
            {
                case Axes.Cubic:
                    _computeFunc = ComputeColorCubic;
                    break;
                case Axes.XY:
                    _computeFunc = ComputeColorXY;
                    break;
                case Axes.XZ:
                    _computeFunc = ComputeColorXZ;
                    break;
                case Axes.YZ:
                    _computeFunc = ComputeColorYZ;
                    break;
                default:
                    _computeFunc = ComputeColorXY;
                    break;
            }
        }

        public override Color ComputeColor(Vector3 point)
        {
            return _computeFunc(point)
                       ? Color1
                       : Color2;
        }

        private bool ComputeColorCubic(Vector3 point)
        {
            double invSize = 1/TileSize;
            double tx = Math.Floor(Tolerance + point.X*invSize);
            double ty = Math.Floor(Tolerance + point.Y*invSize);
            double tz = Math.Floor(Tolerance + point.Z*invSize);
            return ((int) (tx + ty + tz))%2 == 0;
        }

        private bool ComputeColorXY(Vector3 point)
        {
            double invSize = 1/TileSize;
            double tx = Math.Floor(Tolerance + point.X*invSize);
            double ty = Math.Floor(Tolerance + point.Y*invSize);
            return ((int) (tx + ty))%2 == 0;
        }

        private bool ComputeColorXZ(Vector3 point)
        {
            double invSize = 1/TileSize;
            double tx = Math.Floor(Tolerance + point.X*invSize);
            double tz = Math.Floor(Tolerance + point.Z*invSize);
            return ((int) (tx + tz))%2 == 0;
        }

        private bool ComputeColorYZ(Vector3 point)
        {
            double invSize = 1/TileSize;
            double ty = Math.Floor(Tolerance + point.Y*invSize);
            double tz = Math.Floor(Tolerance + point.Z*invSize);
            return ((int) (ty + tz))%2 == 0;
        }
    }
}
