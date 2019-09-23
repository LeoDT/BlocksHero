using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace BlocksHero.Tiles
{
    public class TileShape
    {
        public byte[] Shape { get; set; }
        public int Pitch { get; set; }

        public int Width
        {
            get
            {
                return Pitch;
            }
        }

        public int Height
        {
            get
            {
                return (int)Math.Ceiling((double)Shape.Length / (double)Pitch);
            }
        }

        public byte this[int index]
        {
            get
            {
                return Shape[index];
            }

            set
            {
                Shape[index] = value;
            }
        }

        public int Length
        {
            get { return Shape.Length; }
        }

        public TileShape()
        {
        }

        public static List<Rectangle> convertShapeToRectangles(TileShape tileShape, int offsetX, int offsetY)
        {
            var rects = new List<Rectangle>();

            for (int y = 0; y < tileShape.Height; y++)
            {
                int left = -1;
                int right = -1;

                int rowStart = y * tileShape.Width;

                for (int x = 0; x < tileShape.Width; x++)
                {
                    var cell = tileShape.Shape[rowStart + x];

                    if (cell == 1)
                    {
                        left = left != -1 ? left : x;
                        right = right != -1 ? right : x;

                        if (left != -1)
                        {
                            right = x;
                        }
                    }

                    if (cell == 0 || x == tileShape.Width - 1)
                    {
                        if (left != -1 && right != -1)
                        {
                            rects.Add(
                                new Rectangle(left + offsetX, y + offsetY, right - left, 1)
                            );

                            left = -1;
                            right = -1;
                        }
                    }
                }
            }


            return rects;
        }

        public static byte[] RotateBytes(byte[] bytes, int pitch, uint times = 1)
        {
            int n = bytes.Length / pitch;
            int m = pitch;
            byte[] newBytes = new byte[bytes.Length];
            uint t = times % 4u;

            switch (t)
            {
                case 1:
                case 3:
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < m; j++)
                        {

                            newBytes[j * n + n - 1 - i] = bytes[i * m + j];
                        }
                    }

                    if (t == 3)
                    {
                        Array.Reverse(newBytes);
                    }

                    break;

                default:
                    Array.Copy(bytes, newBytes, bytes.Length);

                    if (t == 2)
                    {
                        Array.Reverse(newBytes);
                    }

                    break;
            }

            return newBytes;
        }
    }
}
