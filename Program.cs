﻿using ray_tracer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace rt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cleanup
            const string frames = "frames";
            if (Directory.Exists(frames))
            {
                var d = new DirectoryInfo(frames);
                foreach (var file in d.EnumerateFiles("*.png")) {
                    file.Delete();
                }
            }
            Directory.CreateDirectory(frames);

            // Scene
            var geometries = new Geometry[]
            {
                
            };

            var lights = new Light[]
            {
                new Light(
                    new Vector(-60.0, -5.0, -6.0),
                    new Color(1, 0.5, 0.2, 1.0),
                    new Color(0.3, 0.5, 0.2, 1.0),
                    new Color(0.3, 1, 0.2, 1.0),
                    0.6
                )
            };
            Asset asset = new Asset();
            ColorMap colorMap = new ColorMap();
            var rt = new RayTracer(lights, asset, colorMap);

            const int width = 800;
            const int height = 600;

            // Go around an approximate middle of the scene and generate frames
            var middle = new Vector(asset.getCenterX(), asset.getCenterY(), asset.getCenterZ());
            var up = new Vector(-Math.Sqrt(0.125), -Math.Sqrt(0.75), Math.Sqrt(0.125)).Normalize();
            var first = (middle ^ up).Normalize();
            const double dist = 250.0;
            const int n = 20;
            const double step = 360.0 / n;

            var tasks = new Task[n];
            for (var i = 0; i < n; i++)
            {
                var ind = new[]{i};
                tasks[i] = Task.Run(() =>
                {
                    var k = ind[0];
                    var a = (step * k) * Math.PI / 180.0;
                    var ca =  Math.Cos(a);
                    var sa =  Math.Sin(a);

                    var dir = first * ca + (up ^ first) * sa + up * (up * first) * (1.0 - ca);

                    var camera = new Camera(
                        middle + dir * dist,
                        dir * -1.0,
                        up,
                        65.0,
                        160.0,
                        120.0,
                        0.0,
                        1000.0
                    );

                    var filename = frames+"/" + $"{k + 1:000}" + ".png";

                    rt.Render(camera, width, height, filename);
                    Console.WriteLine($"Frame {k+1}/{n} completed");
                });
            }

            Task.WaitAll(tasks);
        }
    }
}