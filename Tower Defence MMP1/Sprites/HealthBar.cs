﻿//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tower_Defence.Sprites
{
    public class HealthBar : Sprite, IGameParts
    {
        private Rectangle sourceRectangle;
        public float Offset { get; set; }

        public HealthBar(Texture2D texture = null) : base(texture)
        {
            Offset = 1f;
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sourceRectangle = new Rectangle(0, 0, (int)(_texture.Width * Offset), _texture.Height);
         
            spriteBatch.Draw(_texture, Position, sourceRectangle, Color.White, 0f, _zeroPosition, 0.15f, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime, List<IGameParts> gameParts, List<Tower> backgroundTowers)
        {
            
        }
    }
}
