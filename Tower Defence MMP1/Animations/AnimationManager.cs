using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tower_Defence.Animations
{
    public class AnimationManager
    {
        private Animation _animation;

        private float _timer;
        private float _deltaTime;

        public Vector2 Position { get; set; }
        public Vector2 AttackPosition { get { return Position + new Vector2(_animation.FrameWidth / 2, _animation.FrameHeight / 2); } }
        public Rectangle sourceRectangle;
        public Rectangle destinationRectangle;

        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //_deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0;
                _animation.CurrentColumn++;

                if (_animation.CurrentColumn >= _animation.ColumnCount)
                {
                    _animation.CurrentColumn = 0;
                    _animation.CurrentRow++;
                }

                if(_animation.CurrentRow >= _animation.RowCount)
                {
                    _animation.CurrentRow = 0;
                }
            }

            sourceRectangle = new Rectangle(
                _animation.FrameWidth * _animation.CurrentColumn,
                _animation.FrameHeight * _animation.CurrentRow,
                _animation.FrameWidth, _animation.FrameHeight);

            destinationRectangle = new Rectangle(
                (int)Position.X, (int)Position.Y,
                _animation.FrameWidth,
                _animation.FrameHeight);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.Texture, destinationRectangle, sourceRectangle, Color.White, 0f, new Vector2(0,0), SpriteEffects.None, 0f);
        }


        public void Stop()
        {
            _timer = 0f;

            _animation.CurrentColumn = 0;

        }
    }
}

