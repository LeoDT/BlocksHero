using System;
using BlocksHero.Tiles;
using Microsoft.Xna.Framework;

namespace BlocksHero.Components
{
    public class BattleScene : Scene
    {
        TileScene TileScene;

        public BattleScene(Game game) : base(game)
        {
            var graphics = (GraphicsDeviceManager)game.Services.GetService(typeof(IGraphicsDeviceManager));
            var rect = new Rectangle(
                100,
                100,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight
            );
            TileScene = new TileScene(30, 40, 40, rect);

            this.DemoInit();
        }

        public void DemoInit()
        {
            var defService = (Definition.DefinitionService)this.Game.Services.GetService(typeof(Definition.DefinitionService));

            var bt1 = defService.BoardTypeDefs.Get(1);
            var b1 = new Core.Board(bt1, new Point(0, 0));
            TileScene.AddTileGroup(b1.TileGroup);

            var b2 = new Core.Board(bt1, new Point(0, 6));
            TileScene.AddTileGroup(b2.TileGroup);

            this.AddComponent(new DrawableBoard(Game, b1));
            this.AddComponent(new DrawableBoard(Game, b2));
        }
    }
}
