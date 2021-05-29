//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Tower_Defence.Enums;
using Tower_Defence.Sprites;

namespace Tower_Defence.Buttons
{
    public class MenuButton : Button, IGameParts
    {
        #region Properties
        private Color _colour = Color.White;

        public bool IsMusicButton { get; set; }
        public bool MusicIsOn { get; set; }

        public Difficulty Difficulty { get; set; }
        #endregion

        #region Events
        public event Action<bool> menuButtonEventHandler;
        public event Action<MenuButton, bool> musicButtonEventHandler;
        public event Action<Difficulty> difficultyButtonEventHandler;
        #endregion

        #region

        public MenuButton(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            Font = font;
            PenColour = Color.White;
            Scale = 1f;
        }
        
        public void Update(GameTime gameTime, List<IGameParts> gameParts, List<Tower> backgroundTowers)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

           
            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                _colour = Color.Gray;
            }
            else
            {
                _isHovering = false;
                _colour = Color.White;
            }

            if (_isHovering == true && _currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
            {
                Clicked = !Clicked;
                if(Difficulty != 0)
                {
                    difficultyButtonEventHandler.Invoke(Difficulty);
                }

                if(IsMusicButton)
                {
                    musicButtonEventHandler?.Invoke(this, MusicIsOn);
                }
                menuButtonEventHandler?.Invoke(Clicked);

            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {       
            spriteBatch.Draw(_texture, Position, null, _colour, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (Font.MeasureString(Text).X / 2);

                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (Font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(Font, Text, new Vector2(x, y), PenColour);
            }
        }


        #endregion
    }
}
