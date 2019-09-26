using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BlocksHero.Tiles
{
    public class TileGroup
    {
        public Point Tile { get; set; }
        public TileShape TileShape { get; set; }
        public bool Rotatable { get; set; }
        public int Layer { get; set; }

        public byte[] ShapeMask { get; private set; }

        private uint _rotateAngle;
        public uint RotateAngle
        {
            get { return _rotateAngle; }
            private set
            {
                _rotateAngle = value;

                this.UpdateComputed();
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

        public HashSet<TileGroup> Children { get; set; }

        public TileGroup(Point tile, TileShape tileShape, bool rotatable = false, int layer = 0)
        {
            Tile = tile;
            TileShape = tileShape;
            Rotatable = rotatable;
            Layer = layer;

            ShapeMask = new byte[tileShape.Length];
            ShapeMask.Fill<byte>(1);
        }

        private void UpdateComputed()
        {
            uint times = RotateAngle / 90u;
            byte[] shape = new byte[TileShape.Length];

            for (int i = 0; i < shape.Length; i++)
            {
                shape[i] = (byte)(TileShape.Shape[i] & ShapeMask[i]);
            }

            this._transformedShape = new TileShape(
                TileShape.RotateBytes(shape, TileShape.Pitch, times),
                times % 2 == 0 ? TileShape.Pitch : TileShape.Height
            );
            this._rectangles = TileShape.convertShapeToRectangles(
                TransformedShape, Tile.X, Tile.Y
            );
        }

        public void MaskTileShape(int x, int y, byte mask)
        {
            var index = y * this.TileShape.Pitch + x;
            var oldValue = this.ShapeMask[index];

            if (oldValue != mask)
            {
                this.ShapeMask[index] = mask;

                this.UpdateComputed();
            }
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

            var pTile = parent.Tile;
            var cTile = child.Tile;
            var pShape = parent.TransformedShape;
            var cShape = child.TransformedShape;

            if (
              pTile.X > cTile.X ||
              pTile.X + pShape.Width < cTile.X ||
              pTile.Y > cTile.Y ||
              pTile.Y + pShape.Height < cTile.Y
            )
            {
                return false;
            }

            var offset = pTile + cTile;

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
