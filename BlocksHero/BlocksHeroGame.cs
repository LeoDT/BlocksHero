using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BlocksHero.Definition;

namespace BlocksHero
{
    class BlocksHeroGame : Game
    {
        GraphicsDeviceManager graphics;
        FontService fontService;
        DefinitionService defService;
        Components.Scene ActiveScene;

        public BlocksHeroGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";

            fontService = new FontService(this);
            fontService.addFontFace("normal", 16, @"/System/Library/Fonts/PingFang.ttc");

            defService = new DefinitionService(this);

            Window.AllowUserResizing = true;
            IsMouseVisible = true;
        }

        SpriteBatch spriteBatch;
        Texture2D font;
        Texture2D smile;
        Effect exampleEffect;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            defService.Load();

            font = fontService.toTexture2D("normal", "一二三d四五dd六2七八123九", 100);

            smile = Content.Load<Texture2D>("Smile");

            // Effects need to be loaded from files built by fxc.exe from the DirectX SDK (June 2010)
            // (Note how each .fx file has the Build Action "CompileShader", which produces a .fxb file.)
            exampleEffect = new Effect(GraphicsDevice, TitleContainer.ReadAllBytes(@"Effects/ExampleEffect.fxb"));

            this.ActiveScene = new Components.BattleScene(this);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            Content.Unload();

            spriteBatch.Dispose();

            if (exampleEffect != null)
            {
                exampleEffect.Dispose();
            }

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update(IsActive);

            //
            // Asset Rebuilding:
#if DEBUG
            if (Input.KeyWentDown(Keys.F5))
            {
                if (AssetRebuild.Run())
                {
                    UnloadContent();
                    LoadContent();
                }
            }
            else if (Input.KeyWentDown(Keys.Escape))
            {
                Exit();
            }
#endif

            //
            // Insert your game update logic here.
            //

            if (this.ActiveScene != null)
            {
                this.ActiveScene.Update(gameTime);
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            //
            // Replace this with your own drawing code.
            //

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(font, new Vector2(20, 20), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            Primitives.DrawBox(spriteBatch,
                               new Rectangle(20, 20 + font.Height, 30, 30),
                               Color.Blue);
            spriteBatch.End();

            if (this.ActiveScene != null)
            {
                spriteBatch.Begin();
                this.ActiveScene.Draw(gameTime, spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

    }
}
