using System;

namespace BlocksHero.Core
{
    public class ResourceDescriptor
    {
        public int ResourceTypeId { get; set; }
        public int Amount { get; set; }
    }

    public class Resource
    {
        public ResourceType Type { get; private set; }
        public int Amount { get; set; }

        public Resource(ResourceType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }
}
