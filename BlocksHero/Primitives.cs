using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlocksHero
{
    public static class Primitives
    {
        private static Texture2D Pixel;

        private static void InitializePixel(SpriteBatch spriteBatch)
        {
            Pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] { Color.White });
        }

        public static void DrawBox(this SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            if (Pixel == null)
                InitializePixel(spriteBatch);

            spriteBatch.Draw(Pixel, rect, color);
        }
    }
}
