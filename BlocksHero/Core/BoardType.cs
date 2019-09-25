namespace BlocksHero.Core
{
    public class BoardType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool Validate()
        {
            return true;
        }
    }
}
