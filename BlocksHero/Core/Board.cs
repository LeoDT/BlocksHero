using System;

using Microsoft.Xna.Framework;

using BlocksHero.Tiles;

namespace BlocksHero.Core
{
    public class Board
    {
        public BoardType Type { get; private set; }
        public Guid Id { get; private set; }

        public TileGroup TileGroup { get; set; }

        public Node[] UsedTiles { get; private set; }
        public int[] Durability { get; private set; }

        public Board(BoardType type, Point tile) : this(type, tile, Guid.NewGuid())
        {
        }

        public Board(BoardType type, Point tile, Guid id)
        {
            Id = id;
            Type = type;
            TileGroup = new TileGroup
            (
                tile,
                new TileShape(Type.Width, Type.Height)
            );

            UsedTiles = new Node[TileGroup.TileShape.Length];
            Durability = new int[TileGroup.TileShape.Length];
        }

        public void AddNode(Node node)
        {
            var tileOffset = node.TileGroup.Tile - this.TileGroup.Tile;
            var nShape = node.TileGroup.TileShape;
            var bShape = this.TileGroup.TileShape;

            for (int y = 0; y < nShape.Height; y++)
            {
                int rowStart = y * nShape.Pitch;

                for (int x = 0; x < nShape.Pitch; x++)
                {
                    byte cell = nShape.Shape[rowStart + x];

                    if (cell == 0) continue;

                    int bx = x + tileOffset.X;
                    int by = y + tileOffset.Y;
                    int index = y * bShape.Pitch + x;

                    if (this.UsedTiles[index] != null)
                    {
                        this.UsedTiles[index] = node;
                    }
                    else
                    {
                        throw new Exception("board slot not empty");
                    }
                }
            }

            this.TileGroup.Children.Add(node.TileGroup);
        }

        public void RemoveNode(Node node)
        {
            var nShape = node.TileGroup.TileShape;
            var bShape = this.TileGroup.TileShape;

            for (int y = 0; y < nShape.Height; y++)
            {
                int rowStart = y * nShape.Pitch;

                for (int x = 0; x < nShape.Pitch; x++)
                {
                    int index = y * bShape.Pitch + x;

                    if (this.UsedTiles[index] == node)
                    {
                        this.UsedTiles[y * bShape.Pitch + x] = null;
                    }
                }
            }

            this.TileGroup.Children.Remove(node.TileGroup);
        }

        public void UpdateDurability(int x, int y, int durability)
        {
            var index = y * this.TileGroup.TileShape.Pitch + x;
            var oldValue = this.Durability[index];
            var newValue = (oldValue + durability).Clamp(0, 100);

            this.Durability[index] = newValue;

            if (newValue == 0)
            {
                this.TileGroup.MaskTileShape(x, y, 0);
            }
            else
            {
                this.TileGroup.MaskTileShape(x, y, 1);
            }
        }
    }
}
