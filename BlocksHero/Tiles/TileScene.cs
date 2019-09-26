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

        public Rectangle Size { get; set; }

        public Viewport()
        {
        }
    }

    public class TileScene
    {
        public int TilePixelSize { get; private set; }
        public Rectangle Size { get; private set; }
        public Viewport Viewport { get; private set; }
        public HashSet<TileGroup> TileGroups { get; private set; }

        public TileScene(int tilePixelSize, int tileWidth, int tileHeight, int viewportWidth, int viewportHeight)
        {
            int width = tileWidth * tilePixelSize;
            int height = tileHeight * tilePixelSize;

            TilePixelSize = tilePixelSize;
            Size = new Rectangle(0, 0, width, height);
            Viewport = new Viewport
            {
                TileScene = this,
                XY = new Point(
                    width / 2 - viewportWidth / 2,
                    height / 2 - viewportHeight / 2
                ),
                Size = new Rectangle(0, 0, viewportWidth, viewportHeight)
            };
        }

        public Point ClampViewportOffset(Point offset)
        {
            return new Point(
                offset.X.Clamp(0, Size.Width - Viewport.Size.Width),
                offset.Y.Clamp(0, Size.Height - Viewport.Size.Height)
            );
        }

        public bool AddTileGroup(TileGroup tileGroup)
        {
            return TileGroups.Add(tileGroup);
        }

        public bool CanTileGroupMoveToTile(
            TileGroup tileGroup,
            Point tile,
            HashSet<TileGroup> excludedTileGroup = null
        )
        {
            excludedTileGroup = excludedTileGroup ?? new HashSet<TileGroup>();

            TileGroup newGroup = new TileGroup
            (
                tile,
                tileGroup.TransformedShape,
                tileGroup.Rotatable,
                tileGroup.Layer
            );

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
                    var offset = cg.Tile - tileGroup.Tile;
                    var newTile = tile + offset;

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
