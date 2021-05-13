//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tower_Defence.States
{
    public abstract class State
    {

        #region Fields
        protected Game1 _game1;
        protected GraphicsDeviceManager _graphics;
        protected ContentManager _content;
        protected SpriteBatch _spriteBatch;
        #endregion


        protected State(Game1 game1, GraphicsDeviceManager graphics, ContentManager content)
        {
            _game1 = game1;
            _graphics = graphics;
            _content = content;
            
        }
        #region Methods
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spritebatch);
        #endregion

    }
}
