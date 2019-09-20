using System;

using SharpFont;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlocksHero
{
    internal class FontService : IDisposable
    {
        private Library lib;
        private Game Game;

        #region Properties

        internal Face FontFace { get { return _fontFace; } }
        private Face _fontFace;

        internal uint Size
        {
            get { return _size; }
        }
        private uint _size;

        #endregion // Properties

        #region Constructor

        internal FontService(Game game)
        {
            Game = game;
            lib = new Library();
            _size = 16;
        }

        #endregion

        #region Setters

        internal void SetFont(String filename)
        {
            _fontFace = new Face(lib, filename);
            SetSize(Size);
        }

        internal void SetSize(uint size)
        {
            _size = size;

            if (FontFace != null)
            {
                uint times = Environment.GetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI") == "1" ? 2u : 1u;
                FontFace.SetPixelSizes(0, _size * times);
            }
        }

        #endregion // Setters

        #region Caches

        public Texture2D toTexture2D(String text, int wrapWidth)
        {
            Face face = FontFace;

            float penX = 0, penY = 0;
            float stringWidth = 0; // the measured width of the string
            float stringHeight = 0; // the measured height of the string
            float overrun = 0;
            float underrun = 0;
            float kern = 0;
            int spacingError = 0;
            bool trackingUnderrun = true;
            int rightEdge = 0; // tracking rendered right side for debugging

            // Bottom and top are both positive for simplicity.
            // Drawing in .Net has 0,0 at the top left corner, with positive X to the right
            // and positive Y downward.
            // Glyph metrics have an origin typically on the left side and at baseline
            // of the visual data, but can draw parts of the glyph in any quadrant, and
            // even move the origin (via kerning).
            float top = 0, bottom = 0;

            for (int i = 0; i < text.Length; i++)
            {
                #region Load character
                char c = text[i];

                // Look up the glyph index for this character.
                uint glyphIndex = face.GetCharIndex(c);

                // Load the glyph into the font's glyph slot. There is usually only one slot in the font.
                face.LoadGlyph(glyphIndex, LoadFlags.Default, LoadTarget.Normal);

                // Refer to the diagram entitled "Glyph Metrics" at http://www.freetype.org/freetype2/docs/tutorial/step2.html.
                // There is also a glyph diagram included in this example (glyph-dims.svg).
                // The metrics below are for the glyph loaded in the slot.
                float gAdvanceX = (float)face.Glyph.Advance.X; // same as the advance in metrics
                float gBearingX = (float)face.Glyph.Metrics.HorizontalBearingX;
                float gWidth = face.Glyph.Metrics.Width.ToSingle();

                #endregion
                #region Underrun
                // Negative bearing would cause clipping of the first character
                // at the left boundary, if not accounted for.
                // A positive bearing would cause empty space.
                underrun += -(gBearingX);
                if (stringWidth == 0)
                    stringWidth += underrun;
                if (trackingUnderrun && underrun <= 0)
                {
                    underrun = 0;
                    trackingUnderrun = false;
                }
                #endregion
                #region Overrun
                // Accumulate overrun, which coould cause clipping at the right side of characters near
                // the end of the string (typically affects fonts with slanted characters)
                if (gBearingX + gWidth > 0 || gAdvanceX > 0)
                {
                    overrun -= Math.Max(gBearingX + gWidth, gAdvanceX);
                    if (overrun <= 0) overrun = 0;
                }
                overrun += (float)(gBearingX == 0 && gWidth == 0 ? 0 : gBearingX + gWidth - gAdvanceX);
                // On the last character, apply whatever overrun we have to the overall width.
                // Positive overrun prevents clipping, negative overrun prevents extra space.
                if (i == text.Length - 1)
                    stringWidth += overrun;
                #endregion

                #region Top/Bottom
                // If this character goes higher or lower than any previous character, adjust
                // the overall height of the bitmap.
                float glyphTop = (float)face.Glyph.Metrics.HorizontalBearingY;
                float glyphBottom = (float)(face.Glyph.Metrics.Height - face.Glyph.Metrics.HorizontalBearingY);
                if (glyphTop > top)
                    top = glyphTop;
                if (glyphBottom > bottom)
                    bottom = glyphBottom;
                #endregion

                // Accumulate the distance between the origin of each character (simple width).
                stringWidth += gAdvanceX;

                #region Kerning (for NEXT character)
                // Calculate kern for the NEXT character (if any)
                // The kern value adjusts the origin of the next character (positive or negative).
                if (face.HasKerning && i < text.Length - 1)
                {
                    char cNext = text[i + 1];
                    kern = (float)face.GetKerning(glyphIndex, face.GetCharIndex(cNext), KerningMode.Default).X;
                    // sanity check for some fonts that have kern way out of whack
                    if (kern > gAdvanceX * 5 || kern < -(gAdvanceX * 5))
                        kern = 0;
                    stringWidth += kern;
                }

                #endregion
            }

            stringHeight = top + bottom;

            // If any dimension is 0, we can't create a bitmap
            if (stringWidth == 0 || stringHeight == 0)
                return null;

            int tWidth = (int)stringWidth;
            int tHeight = (int)stringHeight;

            if (wrapWidth != 0)
            {
                tHeight = (int)Math.Ceiling(stringWidth / wrapWidth) * tHeight;
                tWidth = wrapWidth;
            }

            // Create a new bitmap that fits the string.
            Texture2D tx = new Texture2D(
                Game.GraphicsDevice,
                tWidth,
                tHeight,
                false,
                SurfaceFormat.Bgra4444
            );
            trackingUnderrun = true;
            underrun = 0;
            overrun = 0;
            stringWidth = 0;

            // Draw the string into the bitmap.
            // A lot of this is a repeat of the measuring steps, but this time we have
            // an actual bitmap to work with (both canvas and bitmaps in the glyph slot).
            for (int i = 0; i < text.Length; i++)
            {
                #region Load character
                char c = text[i];

                // Same as when we were measuring, except RenderGlyph() causes the glyph data
                // to be converted to a bitmap.
                uint glyphIndex = face.GetCharIndex(c);
                face.LoadGlyph(glyphIndex, LoadFlags.Default, LoadTarget.Normal);
                face.Glyph.RenderGlyph(RenderMode.Normal);
                FTBitmap ftbmp = face.Glyph.Bitmap;

                float gAdvanceX = (float)face.Glyph.Advance.X;
                float gBearingX = (float)face.Glyph.Metrics.HorizontalBearingX;
                float gWidth = (float)face.Glyph.Metrics.Width;

                #endregion
                #region Underrun
                // Underrun
                underrun += -(gBearingX);
                if (penX == 0)
                    penX += underrun;
                if (trackingUnderrun && underrun <= 0)
                {
                    underrun = 0;
                    trackingUnderrun = false;
                }
                #endregion
                #region Draw glyph
                // Whitespace characters sometimes have a bitmap of zero size, but a non-zero advance.
                // We can't draw a 0-size bitmap, but the pen position will still get advanced (below).
                if ((ftbmp.Width > 0 && ftbmp.Rows > 0))
                {
                    var length = ftbmp.Width * ftbmp.Rows;
                    var data = new ushort[length];

                    // 8-bit to 16-bit
                    for (int j = 0; j < length; j++)
                    {
                        var d = ftbmp.BufferData[j] >> 4;
                        data[j] = (ushort)((d << 4) | (d << 8) | (d << 12) | d);
                    }

                    // wrap
                    if (penX + face.Glyph.BitmapLeft + ftbmp.Width >= wrapWidth)
                    {
                        penX = 0;
                        penY += stringHeight;
                    }

                    int x = (int)Math.Round(penX + face.Glyph.BitmapLeft);
                    int y = (int)Math.Round(penY + top - (float)face.Glyph.Metrics.HorizontalBearingY);

                    //Not using g.DrawImage because some characters come out blurry/clipped. (Is this still true?)
                    var rect = new Rectangle(x, y, ftbmp.Width, ftbmp.Rows);

                    tx.SetData(0, rect, data, 0, data.Length);

                    // Check if we are aligned properly on the right edge (for debugging)
                    rightEdge = Math.Max(rightEdge, x + ftbmp.Width);
                    spacingError = ftbmp.Width - rightEdge;
                }
                else
                {
                    rightEdge = (int)(penX + gAdvanceX);
                    spacingError = ftbmp.Width - rightEdge;
                }
                #endregion

                #region Overrun
                if (gBearingX + gWidth > 0 || gAdvanceX > 0)
                {
                    overrun -= Math.Max(gBearingX + gWidth, gAdvanceX);
                    if (overrun <= 0) overrun = 0;
                }
                overrun += (float)(gBearingX == 0 && gWidth == 0 ? 0 : gBearingX + gWidth - gAdvanceX);
                if (i == text.Length - 1) penX += overrun;
                #endregion

                // Advance pen positions for drawing the next character.
                penX += (float)face.Glyph.Advance.X; // same as Metrics.HorizontalAdvance?
                penY += (float)face.Glyph.Advance.Y;

                #region Kerning (for NEXT character)
                // Adjust for kerning between this character and the next.
                if (face.HasKerning && i < text.Length - 1)
                {
                    char cNext = text[i + 1];
                    kern = (float)face.GetKerning(glyphIndex, face.GetCharIndex(cNext), KerningMode.Default).X;
                    if (kern > gAdvanceX * 5 || kern < -(gAdvanceX * 5))
                        kern = 0;
                    penX += (float)kern;
                }
                #endregion


            }

            return tx;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.FontFace != null && !FontFace.IsDisposed)
                        try
                        {
                            FontFace.Dispose();
                        }
                        catch { }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FontService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

}
