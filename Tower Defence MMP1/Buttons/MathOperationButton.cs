using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Enums;
using Tower_Defence.Sprites;
using Tower_Defence.States;

namespace Tower_Defence.Buttons
{
    public class MathOperationButton : Button, IGameParts
    {
        private MathOperation _mathOperation;
        private bool _towerButtonIsClicked;
        private bool _mathOperationIsUsed;
        private float _coolDownTimer;
        private int counter = 0;
        public string MathButtonText { get; set; }
        public float CoolDownTime { get; set; }

        public static event Action<MathOperation> changeMathOperationHandler;
        public static event Action<MathOperation, bool> mathOperationButtonIsClicked;
        
        public MathOperationButton(Texture2D texture, MathOperation mathOperation)
        {
            _texture = texture;
            _mathOperation = mathOperation;
            PenColour = Color.Black;

            GameState.TowerButtonIsClicked += HandleTowerButtonIsClicked;
            MathEnemy.MathOperationUsedHandler += HandleMathOperationUsed;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Colour, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Font, MathButtonText, new Vector2(Rectangle.X + 40, Rectangle.Y + 40), PenColour);
            
        }

        public void Update(GameTime gameTime, List<IGameParts> gameParts)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);


            if (mouseRectangle.Intersects(this.Rectangle))
            {
                _isHovering = true;
                Colour = Color.Gray;
            }
            else if (!mouseRectangle.Intersects(this.Rectangle))
            {
                Colour = Color.White;
                _isHovering = false;
            }

            if(!_towerButtonIsClicked && !_mathOperationIsUsed)
            {

                if (_isHovering && _currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Clicked = true;
                    changeMathOperationHandler?.Invoke(this._mathOperation);
                    mathOperationButtonIsClicked?.Invoke(this._mathOperation, Clicked);
                    
                }

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.RightButton == ButtonState.Pressed)
                {
                    Clicked = false;
                    mathOperationButtonIsClicked?.Invoke(this._mathOperation, Clicked);
                    
                }
            }

            if(_mathOperationIsUsed && (_mathOperation == MathOperation.division || _mathOperation == MathOperation.squareroot))
            {
               _coolDownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Colour = Color.Red;
                
                if(counter < 1)
                {
                    Clicked = false;
                    mathOperationButtonIsClicked?.Invoke(this._mathOperation, Clicked);
                    counter++;
                }

                if (_coolDownTimer >= CoolDownTime)
                {
                    _coolDownTimer = 0f;
                    Colour = Color.White;
                    _mathOperationIsUsed = false;
                    counter--;
                }
            }

        }

        private void HandleMathOperationUsed(MathOperation mathOperation)
        {
            if (mathOperation != _mathOperation) { return; }

            _mathOperationIsUsed = !_mathOperationIsUsed;

        }

        private void HandleTowerButtonIsClicked(bool isClicked)
        {
            _towerButtonIsClicked = isClicked;
        }
    }
}

