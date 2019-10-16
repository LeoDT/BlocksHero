using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlocksHero.Components
{
    public class Scene : IComponent
    {
        public Game Game { get; private set; }

        private List<IComponent> _components;

        public Scene(Game game)
        {
            Game = game;

            _components = new List<IComponent>();
        }

        public void AddComponent(IComponent c)
        {
            this._components.Add(c);
        }

        public void RemoveComponent(IComponent c)
        {
            this._components.Remove(c);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var c in this._components)
            {
                c.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var c in this._components)
            {
                c.Draw(gameTime, spriteBatch);
            }
        }
    }
}
