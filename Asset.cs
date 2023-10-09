using rt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ray_tracer
{
    internal class Asset
    {
        private string assetFileName = "assets/head-181x217x181.dat";
        public static double fromOriginPositionX = 0;
        public static double fromOriginPositionY = 0;
        public static double fromOriginPositionZ = 0;
        public static int assetX = 181;
        public static int assetY = 217;
        public static int assetZ = 181;
        public static int cropX = 50;
        public static int cropY = 0;
        public static int cropZ = 0;
        private Vector coordinateBottomDown = new Vector(fromOriginPositionX, fromOriginPositionY, fromOriginPositionZ);
        private Vector coordinateTopUp = new Vector(
            fromOriginPositionX + assetX - cropX, fromOriginPositionY + assetY - cropY, fromOriginPositionZ + assetZ - cropZ);

        public int[ , , ] AssetMap { get; } = new int[assetX, assetY, assetZ];

        public Asset()
        {
            readAsset();
        }

        public bool isPointInside(Vector position)
        {
            return position.X > coordinateBottomDown.X && position.Y > coordinateBottomDown.Y &&
                position.Z > coordinateBottomDown.Z && position.X < coordinateTopUp.X - cropX && position.Y < coordinateTopUp.Y - cropY &&
                position.Z < coordinateTopUp.Z - cropZ;
        }

        public int getAssetValueFromPosition(Vector position)
        {
            if (!isPointInside(position))
            {
                return 0;
            }
            int distanceX = Convert.ToInt32(Math.Floor(Math.Abs(position.X - coordinateBottomDown.X) / getCubeXLength()));
            int distanceY = Convert.ToInt32(Math.Floor(Math.Abs(position.Y - coordinateBottomDown.Y) / getCubeYLength()));
            int distanceZ = Convert.ToInt32(Math.Floor(Math.Abs(position.Z - coordinateBottomDown.Z) / getCubeZLength()));
            return AssetMap[distanceX, distanceY, distanceZ];
        }

        private void readAsset()
        {
            using (FileStream fileStream = new FileStream(assetFileName, FileMode.Open))
            {
                for (int i = 0; i < assetX; i++)
                {
                    for (int j = 0; j < assetY; j++)
                    {
                        for (int z = 0; z < assetZ; z++)
                        {
                            int data = fileStream.ReadByte();
                            if (data == -1)
                            {
                                Console.WriteLine("Cannot Read file!");
                                return;
                            }
                            AssetMap[i, j, z] = data;
                        }
                    }
                }
            }
        }

        public double getCubeXLength()
        {
            return Math.Abs(coordinateTopUp.X - coordinateBottomDown.X) / assetX;
        }

        public double getCubeYLength()
        {
            return Math.Abs(coordinateTopUp.Y - coordinateBottomDown.Y) / assetY;
        }

        public double getCubeZLength()
        {
            return Math.Abs(coordinateTopUp.Z - coordinateBottomDown.Z) / assetZ;
        }

        public double getCenterX()
        {
            return coordinateBottomDown.X + (coordinateTopUp.X - coordinateBottomDown.X) / 2;
        }

        public double getCenterY()
        {
            return coordinateBottomDown.Y + (coordinateTopUp.Y - coordinateBottomDown.Y) / 2;
        }

        public double getCenterZ()
        {
            return coordinateBottomDown.Z + (coordinateTopUp.Z - coordinateBottomDown.Z) / 2;
        }
    }
}
