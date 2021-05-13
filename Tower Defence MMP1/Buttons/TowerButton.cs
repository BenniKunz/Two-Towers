//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Sprites;
using Tower_Defence.States;

namespace Tower_Defence.Buttons
{
    public class TowerButton : Button, IGameParts
    {
        public bool isPreviewOn;

        public event Action<AttackType> PreviewTowerEventHandler;
        public event Action TurnTowerPreviewOffEventHandler;
        public event Action<TowerButton> PlaceTowerEventHandler;
        

        public TowerButton(Texture2D texture, AttackType InputTowerType)
        {
            _texture = texture;
            PenColour = Color.White;
            TowerType = InputTowerType;
            if(GameManager.GameManagerInstance.CurrentLevel == Tower_Defence_MMP1.Enums.Level.LevelOne)
            {
                GameStateOne.TowerButtonIsClicked += HandleTowerButtonIsClicked;
            }
            else if(GameManager.GameManagerInstance.CurrentLevel == Tower_Defence_MMP1.Enums.Level.LevelTwo)
            {
                GameStateTwo.TowerButtonIsClicked += HandleTowerButtonIsClicked;
            }
        }

        private void HandleTowerButtonIsClicked(bool isClicked)
        {
            Clicked = isClicked;
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, PenColour, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
        }

        public void Update(GameTime gameTime, List<IGameParts> gameParts, List<Tower> backgroundTowers)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);



            if (mouseRectangle.Intersects(this.Rectangle))
            {
                _isHovering = true;
                PenColour = Color.Gray;
            }
            else if (!mouseRectangle.Intersects(this.Rectangle))
            {
                PenColour = Color.White;
                _isHovering = false;
            }


            if (!_isHovering && isPreviewOn && _previousMouse.LeftButton == ButtonState.Pressed && _currentMouse.LeftButton == ButtonState.Released)
            {
                isPreviewOn = false;
                PlaceTowerEventHandler?.Invoke(this);
            }

            if (!_isHovering && isPreviewOn && _previousMouse.RightButton == ButtonState.Pressed && _currentMouse.RightButton == ButtonState.Released)
            {
                isPreviewOn = false;
                TurnTowerPreviewOffEventHandler?.Invoke();
            }

            
            if (Clicked) { return; }

            if (_isHovering)
            {
                if (_currentMouse.LeftButton == ButtonState.Released)
                {
                    PenColour = Color.Gray;
                }

                if(isPreviewOn) { return; }

                if (_previousMouse.LeftButton == ButtonState.Pressed && _currentMouse.LeftButton == ButtonState.Released)
                {
                    isPreviewOn = true;
                    Clicked = true;
                    PreviewTowerEventHandler?.Invoke(this.TowerType);
                    
                }
            }
        }
    }
}
