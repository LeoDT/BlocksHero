using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using BlocksHero.Core;

namespace BlocksHero.Components
{
    public class DrawableBoard : IComponent
    {
        Game Game;
        Board Board;

        public DrawableBoard(Game game, Board board)
        {
            Game = game;
            Board = board;
        }

        public void Update(GameTime gameTime)
        {
            Board.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var shape = Board.TileGroup.TransformedShape.Shape;
            var tileScene = Board.TileGroup.TileScene;

            var offsetX = tileScene.Bounds.X + Board.TileGroup.Tile.X * tileScene.TilePixelSize;
            var offsetY = tileScene.Bounds.Y + Board.TileGroup.Tile.Y * tileScene.TilePixelSize;

            var dest = new Rectangle(
                offsetX,
                offsetY,
                tileScene.TilePixelSize,
                tileScene.TilePixelSize
            );

            for (int i = 0; i < shape.Length; i++)
            {
                var cell = shape[i];

                if (cell == 0) continue;

                var durability = Board.Durability[i];

                dest.X = offsetX + (i % Board.TileGroup.TransformedShape.Pitch) * tileScene.TilePixelSize;
                dest.Y = offsetY + i / Board.TileGroup.TransformedShape.Pitch * tileScene.TilePixelSize;

                if (durability != 100)
                {
                    dest.Height = dest.Height * durability / 100;
                }

                Primitives.DrawBox(spriteBatch, dest, Color.Red);

                dest.Height = tileScene.TilePixelSize;
            }
        }
    }
}
