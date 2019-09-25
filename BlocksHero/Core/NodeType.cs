using BlocksHero.Tiles;

namespace BlocksHero.Core
{
    public class NodeType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BlokusShape.ShapeType Shape { get; set; }
        public int BoardTypeId { get; set; }
        public int Cycle { get; set; }
        public ResourceDescriptor[] Output { get; set; }

        public bool Validate()
        {
            return true;
        }
    }
}
