using System.IO;

using Nett;

using Microsoft.Xna.Framework;

using BlocksHero.Core;

namespace BlocksHero.Definition
{
    public class DefinitionService
    {
        Game Game;

        DefinitionDict<ResourceType> ResourceTypeDefs = new DefinitionDict<ResourceType>();
        DefinitionDict<NodeType> NodeTypeDefs = new DefinitionDict<NodeType>();
        DefinitionDict<BoardType> BoardTypeDefs = new DefinitionDict<BoardType>();

        public DefinitionService(Game game)
        {
            Game = game;

            Game.Services.AddService(typeof(DefinitionService), this);
        }

        public void Load()
        {
            this.LoadDefs(Path.Combine("Content", "Defs", "ResourceTypes.toml"));
            this.LoadDefs(Path.Combine("Content", "Defs", "NodeTypes.toml"));
            this.LoadDefs(Path.Combine("Content", "Defs", "BoardTypes.toml"));
        }

        public void LoadDefs(string path)
        {
            var stream = TitleContainer.OpenStream(path);

            var tt = Toml.ReadStream(stream);

            foreach (var p in tt)
            {
                this.AddDefByDefName(p.Key, p.Value);
            }

            stream.Close();
        }

        protected void AddDefByDefName(string defName, TomlObject tobj)
        {
            switch (defName)
            {
                case "ResourceType":
                    var rTypes = tobj.Get<ResourceType[]>();

                    foreach (var t in rTypes)
                    {
                        if (t.Validate())
                        {
                            this.ResourceTypeDefs.Defs.Add(t.Id, t);
                        }
                    }
                    break;

                case "BoardType":
                    var bTypes = tobj.Get<BoardType[]>();

                    foreach (var t in bTypes)
                    {
                        if (t.Validate())
                        {
                            this.BoardTypeDefs.Defs.Add(t.Id, t);
                        }
                    }
                    break;

                case "NodeType":
                    var nTypes = tobj.Get<NodeType[]>();

                    foreach (var t in nTypes)
                    {
                        if (t.Validate())
                        {
                            this.NodeTypeDefs.Defs.Add(t.Id, t);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
