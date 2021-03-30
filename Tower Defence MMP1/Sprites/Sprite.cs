using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Animations;

namespace Tower_Defence.Sprites
{
    public class Sprite : IGameParts
    {
        #region Fields
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Vector2 _zeroPosition = new Vector2(0, 0);
        //protected Vector2 _origin;
        protected Vector2 _direction;
        //protected float _rotation;
        protected MouseState _currentMouse;
        protected MouseState _previousMouse;

        protected Animation _animation;
        public AnimationManager _animationManager;
        #endregion

        #region Properties
        public Color Color { get; set; }

        public Texture2D Texture 
        { get { return _texture; } }
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_animationManager != null)
                {
                    _animationManager.Position = _position;
                }
            }
        }
        public Vector2 Origin { get; set; }
        public Vector2 Direction { get; set; }
        //public float Rotation { get; set; }
        #endregion


        public Sprite(Texture2D texture = null)
        {
            _texture = texture;

            if (texture != null)
                Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public Rectangle Rectangle
        {
            get
            {
                if (_texture != null)
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
                }

                throw new Exception("no sprite texture");
            }
            set
            {
                new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
        public virtual void Update(GameTime gameTime, List<IGameParts> gameParts)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
