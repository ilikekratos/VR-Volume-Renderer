using System;

namespace rt
{
    public class Sphere : Geometry
    {
        private Vector Center { get; set; }
        private double Radius { get; set; }

        public Sphere(Vector center, double radius, Material material, Color color) : base(material, color)
        {
            Center = center;
            Radius = radius;
        }

        public override Intersection GetIntersection(Line line, double minDist, double maxDist)
        {
            // ADD CODE HERE: Calculate the intersection between the given line and this sphere
            double h = line.X0.X - Center.X; // b - xc
            double m = line.X0.Y - Center.Y; // m - yc
            double n = line.X0.Z - Center.Z; // f - zc
            double i1 = Math.Pow(line.Dx.X, 2) + Math.Pow(line.Dx.Y, 2) + Math.Pow(line.Dx.Z, 2); // a^2 + c^2 + e^2
            double i2 = 2 * (line.Dx.X * h + line.Dx.Y * m + line.Dx.Z * n);
            double i3 = Math.Pow(h, 2) + Math.Pow(m, 2) + Math.Pow(n, 2) - Math.Pow(Radius, 2);
            double delta = Math.Pow(i2, 2) - 4 * i1 * i3;
            if (delta < 0)
            {
                // there is no solution, therefore, no intersection
                return new Intersection();
            }

            double t1 = (-i2 + Math.Sqrt(delta)) / 2 * i1;
            double t2 = (-i2 - Math.Sqrt(delta)) / 2 * i1;

            if (t1 > minDist && t1 < maxDist)
            {
                return new Intersection(true, true, this, line, t1);
            }
            else if (t2 > minDist && t2 < maxDist)
            {
                return new Intersection(true, true, this, line, t2);
            }

            return new Intersection();
        }

        public override Vector Normal(Vector v)
        {
            var n = v - Center;
            n.Normalize();
            return n;
        }
    }
}