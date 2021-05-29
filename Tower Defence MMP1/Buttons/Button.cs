//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tower_Defence.Enums;

namespace Tower_Defence.Buttons
{
    public abstract class Button
    {
        #region Fields
        protected MouseState _currentMouse;
        protected MouseState _previousMouse;
        protected Texture2D _texture;
        protected bool _isHovering;

        #endregion

        #region Properties

        public SpriteFont Font { get; set; }
        public AttackType TowerType { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get { return _texture; } set { _texture = value; } }
        public Color PenColour { get; set; }
        public Color Colour { get; set; }
        public float Scale { get; set; }
        public bool Clicked { get; set; }
        public string Text { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
        #endregion

    }
}