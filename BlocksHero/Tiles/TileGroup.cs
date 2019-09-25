using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BlocksHero.Tiles
{
    public class TileGroup
    {
        public Tile Tile { get; set; }
        public TileShape TileShape { get; set; }
        public bool Rotatable { get; set; }
        public int Layer { get; set; }

        private uint _rotateAngle;
        public uint RotateAngle
        {
            get { return _rotateAngle; }
            private set
            {
                _rotateAngle = value;

                uint times = value / 90u;
                this._transformedShape = new TileShape(
                    TileShape.RotateBytes(TileShape.Shape, TileShape.Pitch, times),
                    times % 2 == 0 ? TileShape.Pitch : TileShape.Height
                );
                this._rectangles = TileShape.convertShapeToRectangles(
                    TransformedShape, Tile.X, Tile.Y
                );
            }
        }

        private TileShape _transformedShape;
        public TileShape TransformedShape
        {
            get => _transformedShape;
        }

        private List<Rectangle> _rectangles;
        public List<Rectangle> Rectangles
        {
            get => _rectangles;
        }

        public HashSet<TileGroup> Children { get; private set; }

        public TileGroup()
        {
        }

        public static bool Intersect(TileGroup a, TileGroup b)
        {
            if (a.Layer != b.Layer)
            {
                return false;
            }

            for (int i = 0; i < a.Rectangles.Count; i++)
            {
                var ar = a.Rectangles[i];

                for (int j = 0; j < b.Rectangles.Count; j++)
                {
                    var br = b.Rectangles[j];

                    if (ar.Intersects(br))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool Contains(TileGroup parent, TileGroup child)
        {
            if (parent.Layer == child.Layer)
            {
                return false;
            }

            Tile pTile = parent.Tile;
            Tile cTile = child.Tile;
            TileShape pShape = parent.TransformedShape;
            TileShape cShape = child.TransformedShape;

            if (
              pTile.X > cTile.X ||
              pTile.X + pShape.Width < cTile.X ||
              pTile.Y > cTile.Y ||
              pTile.Y + pShape.Height < cTile.Y
            )
            {
                return false;
            }

            Tile offset;

            Tile.Add(pTile, cTile, out offset);

            // there should be a corresponding cell === 1 in parent when child cell === 1
            for (int y = 0; y < cShape.Length; y++)
            {
                int cRowStart = y * cShape.Pitch;
                int pRowStart = (y + offset.Y) * pShape.Pitch + offset.X;

                if (pRowStart >= pShape.Length)
                {
                    return false;
                }

                for (int x = 0; x < cShape.Pitch; x++)
                {
                    if (cRowStart + x >= cShape.Length ||
                        pRowStart + x >= pShape.Length)
                    {
                        return false;
                    }

                    byte cell = cShape[cRowStart + x];
                    byte pCell = pShape[pRowStart + x];

                    if (cell == 1 && pCell == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
