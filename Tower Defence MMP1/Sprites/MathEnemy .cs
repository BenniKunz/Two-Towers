using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Animations;
using Tower_Defence.Buttons;
using Tower_Defence.Enums;
using Tower_Defence.Interfaces;
using Tower_Defence.States;

namespace Tower_Defence.Sprites
{
    public class MathEnemy : Sprite, IReachable
    {

        public int HealthPoints { get; set; }
        public bool HasReachedTarget { get; set; }
        public bool TowerButtonIsClicked { get; set; }
        public bool MathOperationButtonIsClicked { get; set; }

        private Random random = new Random();
        private SpriteFont _spriteFont;
        private MathOperation _mathOperation = MathOperation.subtraction;
        private int[] healthPointsArray = new int[] { 7, 51, 49, 100, 66, 36, 3, 77, 16, 9, 81, 121, 90, 85, 55, 64, 69 };

        #region Fields
        private float _timer;
        #endregion

        public static event Action<MathEnemy> MathEnemyDeathHandler;
        public static event Action<MathOperation> MathOperationUsedHandler;

        public MathEnemy(Texture2D texture, int tileRowCount, int tileColumnCount, Texture2D healthBar, Texture2D healthBarBackground, SpriteFont spriteFont) : base(texture)
        {
            _animation = new Animation(texture, tileRowCount, tileColumnCount);
            _animationManager = new AnimationManager(_animation);
            _spriteFont = spriteFont;
            HealthPoints = RandomHealthPoints();
            Color = Color.White;
            MathOperationButton.changeMathOperationHandler += HandleMathOperation;
            MathOperationButton.mathOperationButtonIsClicked += HandleMathOperationButtonIsClicked;
            GameState.TowerButtonIsClicked += HandleTowerButtonClicked;
        }

        private int RandomHealthPoints()
        {
            int num = random.Next(0, healthPointsArray.Length);

            return healthPointsArray[num];
        }

        private void HandleMathOperationButtonIsClicked(MathOperation mathOperation, bool clicked)
        {
            MathOperationButtonIsClicked = clicked;
        }

        private void HandleTowerButtonClicked(bool isClicked)
        {
            TowerButtonIsClicked = isClicked;
        }

        private void HandleMathOperation(MathOperation mathoperation)
        {
            _mathOperation = mathoperation;
        }

        public override void Update(GameTime gameTime, List<IGameParts> gameParts)
        {
            EnemyMovement(gameTime);
            _animationManager.Update(gameTime);

            if (TowerButtonIsClicked == false)
            {
                CheckMathEnemyClicked();
            }

            if (HealthPoints <= 0)
            {
                MathEnemyDeathHandler?.Invoke(this);
            }

        }

        private void CheckMathEnemyClicked()
        {
            if(!MathOperationButtonIsClicked) { return; }

            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var mouseRec = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (mouseRec.X > Position.X + 50f && mouseRec.X < (Position.X + 200f) && mouseRec.Y > Position.Y + 50f && _currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
            {
                switch (_mathOperation)
                {
                    case MathOperation.addition:
                        this.HealthPoints++;
                        break;
                    case MathOperation.subtraction:
                        this.HealthPoints--;
                        break;
                    case MathOperation.division:
                        this.HealthPoints /= 2;
                        MathOperationUsedHandler?.Invoke(_mathOperation);
                        break;
                    case MathOperation.squareroot:
                        this.HealthPoints = (int)Math.Sqrt(this.HealthPoints);
                        MathOperationUsedHandler?.Invoke(_mathOperation);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch);
            spriteBatch.DrawString(_spriteFont, HealthPoints.ToString(), this.Position, Color);

        }


        private void EnemyMovement(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > 0.01f)
            {
                if (this.Position.X >= 350 && this.Position.X < 700 && this.Position.Y >= 270)
                {
                    Position += new Vector2(1, -1);
                    return;
                }
                if (this.Position.X >= 1000 && this.Position.X <= 1230)
                {
                    Position += new Vector2(1, -1);
                    return;
                }

                Position += new Vector2(1, 0);
            }
        }
    }
}
