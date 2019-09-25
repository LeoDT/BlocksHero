using System.Collections.Generic;

namespace BlocksHero.Core
{
    public class ResourceType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ResourceDescriptor[] ResourceRequirements { get; set; }

        public bool Validate()
        {
            return true;
        }

        static public Resource MakeResource(ResourceType resourceType, int amount)
        {
            return new Resource(resourceType, amount);
        }

        static public bool ResourceRequirementsFullfilled(ResourceType resourceType, List<Resource> resources)
        {
            if (resourceType.ResourceRequirements != null)
            {
                foreach (var rr in resourceType.ResourceRequirements)
                {
                    var hit = resources.Find(r => r.Type.Id == rr.ResourceTypeId);

                    if (hit != null && hit.Amount >= rr.Amount)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static public List<Resource> MergeResources(List<Resource> resources)
        {
            List<Resource> result = new List<Resource>();

            foreach (var resource in resources)
            {
                var hit = result.Find(r => r.Type.Id == resource.Type.Id);

                if (hit != null)
                {
                    hit.Amount += resource.Amount;
                }
                else
                {
                    result.Add(new Resource(resource.Type, resource.Amount));
                }
            }

            return result;
        }
    }
}
