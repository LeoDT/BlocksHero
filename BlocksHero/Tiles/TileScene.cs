using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace BlocksHero.Tiles
{
    public class Viewport
    {
        public TileScene TileScene { get; set; }
        private Point _xy;
        public Point XY
        {
            get { return _xy; }
            set
            {
                _xy = TileScene.ClampViewportOffset(value);
            }
        }

        public System.Drawing.Size Size { get; set; }

        public Viewport()
        {
        }
    }

    public class TileScene
    {
        public int TilePixelSize { get; set; }
        public System.Drawing.Size Size { get; set; }
        public Viewport Viewport { get; set; }
        public HashSet<TileGroup> TileGroups { get; set; }

        public TileScene(int tilePixelSize, int tileWidth, int tileHeight, int viewportWidth, int viewportHeight)
        {
            int width = tileWidth * tilePixelSize;
            int height = tileHeight * tilePixelSize;

            TilePixelSize = tilePixelSize;
            Size = new System.Drawing.Size(width, height);
            Viewport = new Viewport
            {
                TileScene = this,
                XY = new Point(
                    width / 2 - viewportWidth / 2,
                    height / 2 - viewportHeight / 2
                ),
                Size = new System.Drawing.Size(viewportWidth, viewportHeight)
            };
        }

        public Point ClampViewportOffset(Point offset)
        {
            return new Point(
                offset.X.Clamp(0, Size.Width - Viewport.Size.Width),
                offset.Y.Clamp(0, Size.Height - Viewport.Size.Height)
            );
        }

        public void AddTileGroup(TileGroup tileGroup)
        {
            TileGroups.Add(tileGroup);
        }

        public bool CanTileGroupMoveToTile(
            TileGroup tileGroup,
            Tile tile,
            HashSet<TileGroup> excludedTileGroup = null
        )
        {
            excludedTileGroup = excludedTileGroup ?? new HashSet<TileGroup>();

            TileGroup newGroup = new TileGroup
            {
                Tile = tile,
                TileShape = tileGroup.TransformedShape,
                Rotatable = tileGroup.Rotatable,
                Layer = tileGroup.Layer
            };

            foreach (var g in this.TileGroups)
            {
                if (g != tileGroup &&
                    !excludedTileGroup.Contains(g) &&
                    TileGroup.Intersect(g, newGroup))
                {
                    return false;
                }

                foreach (var cg in tileGroup.Children)
                {
                    Tile newTile;
                    Tile offset;

                    Tile.Subtract(cg.Tile, tileGroup.Tile, out offset);
                    Tile.Add(tile, offset, out newTile);

                    if (!CanTileGroupMoveToTile(cg, newTile, tileGroup.Children))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
