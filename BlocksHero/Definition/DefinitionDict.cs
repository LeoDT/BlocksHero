using System.Collections.Generic;

namespace BlocksHero.Definition
{
    public class DefinitionDict<T>
    {
        public Dictionary<int, T> Defs = new Dictionary<int, T>();

        public T Get(int id)
        {
            return this.Defs[id];
        }
    }
}
