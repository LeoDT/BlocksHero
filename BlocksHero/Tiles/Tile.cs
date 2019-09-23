namespace BlocksHero.Tiles
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Tile()
        {
        }

        public static void Add(Tile value1, Tile value2, out Tile result)
        {
            result = new Tile();

            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        public static void Subtract(Tile value1, Tile value2, out Tile result)
        {
            result = new Tile();

            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }
    }
}