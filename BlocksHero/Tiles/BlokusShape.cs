using System;
using System.Collections.Generic;

namespace BlocksHero.Tiles
{
    public class BlokusShape
    {
        public enum ShapeType
        {
            One,
            Two,
            Three,
            Four,
            Five
        }

        public static Dictionary<ShapeType, List<TileShape>> Shapes = new Dictionary<ShapeType, List<TileShape>>
        {
            { ShapeType.One,
                new List<TileShape> {
                    new TileShape(new byte[1] { 1 }, 1)
                }
            },
            { ShapeType.Two,
                new List<TileShape> {
                    new TileShape(new byte[2] { 1, 1 }, 2)
                }
            },
            { ShapeType.Three,
                new List<TileShape> {
                    new TileShape(new byte[4] { 1, 1, 0, 1 }, 2),
                    new TileShape(new byte[3] { 1, 1, 1 }, 3),
                }
            },
            { ShapeType.Four,
                new List<TileShape> {
                    new TileShape(new byte[4] { 1, 1, 1, 1 }, 2),
                    new TileShape(new byte[6] { 0, 1, 0, 1, 1, 1 }, 3),
                    new TileShape(new byte[6] { 0, 0, 1, 1, 1, 1 }, 3),
                    new TileShape(new byte[6] { 0, 1, 1, 1, 1, 0 }, 3),
                    new TileShape(new byte[4] { 1, 1, 1, 1 }, 4),
                }
            },
            { ShapeType.Five,
                new List<TileShape> {
                    new TileShape(new byte[6] { 1, 1, 1, 0, 1, 1 }, 2),
                    new TileShape(new byte[6] { 1, 0, 1, 1, 1, 1 }, 2),
                    new TileShape(new byte[9] { 0, 0, 1, 1, 1, 1, 1, 0, 0 }, 3),
                    new TileShape(new byte[9] { 0, 1, 1, 1, 1, 0, 1, 0, 0 }, 3),
                    new TileShape(new byte[9] { 0, 1, 1, 1, 1, 0, 0, 1, 0 }, 3),
                    new TileShape(new byte[9] { 0, 1, 0, 1, 1, 1, 0, 1, 0 }, 3),
                    new TileShape(new byte[9] { 0, 1, 0, 0, 1, 0, 1, 1, 1 }, 3),
                    new TileShape(new byte[9] { 1, 0, 0, 1, 0, 0, 1, 1, 1 }, 3),
                    new TileShape(new byte[8] { 1, 0, 0, 0, 1, 1, 1, 1 }, 4),
                    new TileShape(new byte[8] { 0, 1, 1, 1, 1, 1, 0, 0 }, 4),
                    new TileShape(new byte[8] { 0, 1, 0, 0, 1, 1, 1, 1 }, 4),
                    new TileShape(new byte[5] { 1, 1, 1, 1, 1 }, 5),
                }
            },
        };

        static public TileShape RandomTileShape(ShapeType shapeType)
        {
            var shapes = Shapes[shapeType];

            if (shapes.Count == 1)
            {
                return shapes[0];
            }

            var random = new Random();
            int index = random.Next(shapes.Count);

            return shapes[index];
        }
    }
}
