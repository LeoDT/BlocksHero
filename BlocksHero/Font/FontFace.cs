using System;

using SharpFont;

namespace BlocksHero
{
    internal class FontFace : IDisposable
    {
        public SharpFont.Face Face;
        public uint Size;

        public FontFace(Library lib, string path, uint size)
        {
            this.Face = new SharpFont.Face(lib, path);
            this.Size = size;

            uint times = Environment.GetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI") == "1" ? 2u : 1u;
            Face.SetPixelSizes(0, size * times);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Face != null && !Face.IsDisposed)
                        try
                        {
                            Face.Dispose();
                        }
                        catch { }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


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
