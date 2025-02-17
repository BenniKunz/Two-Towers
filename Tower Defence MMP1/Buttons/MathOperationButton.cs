﻿//MultiMediaTechnology 
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
        private string _tempMathButtonText;
        public string MathButtonText { get; set; }
        public float CoolDownTime { get; set; }

        public static event Action<MathOperation> ChangeMathOperationHandler;
        public static event Action<MathOperation, bool> MathOperationButtonIsClicked;
        
        public MathOperationButton(Texture2D texture, MathOperation mathOperation)
        {
            _texture = texture;
            _mathOperation = mathOperation;
            PenColour = Color.Black;

            if (GameManager.GameManagerInstance.CurrentLevel == Level.LevelOne)
            {
                GameStateOne.TowerButtonIsClicked += HandleTowerButtonIsClicked;
            }
            else if (GameManager.GameManagerInstance.CurrentLevel == Level.LevelTwo)
            {
                GameStateTwo.TowerButtonIsClicked += HandleTowerButtonIsClicked;
            }

            MathEnemy.MathOperationUsedHandler += HandleMathOperationUsed;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Colour, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Font, MathButtonText, new Vector2(Rectangle.X + 40, Rectangle.Y + 40), PenColour);
            
        }

        public void Update(GameTime gameTime, List<IGameParts> gameParts, List<Tower> backgroundTowers)
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
                    
                    ChangeMathOperationHandler?.Invoke(this._mathOperation);
                    MathOperationButtonIsClicked?.Invoke(this._mathOperation, Clicked);
                    
                    
                }

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.RightButton == ButtonState.Pressed)
                {
                    Clicked = false;
                    MathOperationButtonIsClicked?.Invoke(this._mathOperation, Clicked);
                    

                }
            }

            if(_mathOperationIsUsed && (_mathOperation == MathOperation.division || _mathOperation == MathOperation.squareroot))
            {
                
               _coolDownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Colour = Color.Red;
                
                if(counter < 1)
                {
                    Clicked = false;
                    MathOperationButtonIsClicked?.Invoke(this._mathOperation, Clicked);
                    _tempMathButtonText = MathButtonText;
                    counter++;
                }
                MathButtonText = (CoolDownTime - (int)_coolDownTimer).ToString();

                if (_coolDownTimer >= CoolDownTime)
                {
                    _coolDownTimer = 0f;
                    MathButtonText = _tempMathButtonText;
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

