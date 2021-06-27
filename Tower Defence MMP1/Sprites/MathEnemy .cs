//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Tower_Defence.Animations;
using Tower_Defence.Buttons;
using Tower_Defence.Enums;
using Tower_Defence.Interfaces;
using Tower_Defence.States;


namespace Tower_Defence.Sprites
{
    public class MathEnemy : Sprite, IReachable, IUnsubscribable
    {

        public int HealthPoints { get; set; }
        public bool HasReachedTarget { get; set; }
        public bool TowerButtonIsClicked { get; set; }
        public bool MathOperationButtonIsClicked { get; set; }
        public bool idle { get; set; }

        private Random random = new Random();
        private SpriteFont _spriteFont;
        private Vector2 _healthPointsOffset = new Vector2(50,0);
        private MathOperation _mathOperation;
        private float Speed;
        private float _timer;

        private readonly Dictionary<Difficulty, int[]> healthPointsDictionary = new Dictionary<Difficulty, int[]>()
        {
            {Difficulty.easy,  new int[] { 21, 16, 9, 10, 15, 20, 30, 35, 40, 50, 49, 64, 81} },
            {Difficulty.normal, new int[] { 70, 51, 49, 100, 66, 36, 63, 77, 16, 91, 81, 121, 90, 85, 55, 64, 69, 102, 105 } },
            {Difficulty.hard,  new int[] { 88, 99, 98, 53, 63, 122, 78, 89, 130, 145, 1000, 1001} },

        };
        

        public static event Action<MathEnemy> MathEnemyDeathHandler;
        public static event Action<MathOperation> MathOperationUsedHandler;

        public MathEnemy(Texture2D texture, int tileRowCount, int tileColumnCount, Texture2D healthBar, Texture2D healthBarBackground, SpriteFont spriteFont) : base(texture)
        {
            _animation = new Animation(texture, tileRowCount, tileColumnCount);
            _animationManager = new AnimationManager(_animation);
            _spriteFont = spriteFont;
            HealthPoints = RandomHealthPoints();
            Color = Color.White;
            MathOperationButton.ChangeMathOperationHandler += HandleMathOperation;
            MathOperationButton.MathOperationButtonIsClicked += HandleMathOperationButtonIsClicked;
            Speed = 65;

            if (GameManager.GameManagerInstance.CurrentLevel == Level.LevelOne)
            {
                GameStateOne.TowerButtonIsClicked += HandleTowerButtonClicked;
                _mathOperation = GameStateOne._mathOperation;
                MathOperationButtonIsClicked = GameStateOne._mathOperationButtonIsClicked;
                TowerButtonIsClicked = GameStateOne._towerButtonIsClicked;
            }
            else if (GameManager.GameManagerInstance.CurrentLevel == Level.LevelTwo)
            {
                GameStateTwo.TowerButtonIsClicked += HandleTowerButtonClicked;
                _mathOperation = GameStateTwo._mathOperation;
                MathOperationButtonIsClicked = GameStateTwo._mathOperationButtonIsClicked;
                TowerButtonIsClicked = GameStateTwo._towerButtonIsClicked;
            }
        }

        public override void Update(GameTime gameTime, List<IGameParts> gameParts, List<Tower> backgroundTowers)
        {
           
            if (!idle)
            {
                EnemyMovement(gameTime);
            }

            _animationManager.Update(gameTime);

            
            if (TowerButtonIsClicked == false)
            {
                CheckMathEnemyClicked();
            }

            if (HealthPoints <= 0)
            {
                Unsubscribe();
                MathEnemyDeathHandler?.Invoke(this);
                GameManager.GameManagerInstance.StoppedEnemies++;
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch);
            spriteBatch.DrawString(_spriteFont, HealthPoints.ToString(), this.Position + _healthPointsOffset, Color);

        }

        private void CheckMathEnemyClicked()
        {
         
            if (!MathOperationButtonIsClicked) { return; }

            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var mouseRec = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            
            if (mouseRec.X > Position.X + 50f && mouseRec.X < (Position.X + 200f) && mouseRec.Y > Position.Y + 50f && _currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
            {
                if(_mathOperation == MathOperation.addition)
                {
                    this.HealthPoints++;
                    GameManager.GameManagerInstance.MathOperationIsUsed = true;
                }
                else if (_mathOperation == MathOperation.subtraction)
                {
                    this.HealthPoints--;
                    GameManager.GameManagerInstance.MathOperationIsUsed = true;
                }
                else if (_mathOperation == MathOperation.division)
                {
                    if(HealthPoints % 2 == 0)
                    {
                        HealthPoints /= 2 ;
                        MathOperationUsedHandler?.Invoke(_mathOperation);
                        GameManager.GameManagerInstance.MathOperationIsUsed = true;
                    }
                }
                else if(_mathOperation == MathOperation.squareroot)
                {
                    double result = Math.Sqrt(HealthPoints);
                    bool isSquare = result % 1 == 0;
                    if(isSquare)
                    {
                        this.HealthPoints = (int)Math.Sqrt(this.HealthPoints);
                        MathOperationUsedHandler?.Invoke(_mathOperation);
                        GameManager.GameManagerInstance.MathOperationIsUsed = true;
                    }
                }
            }
        }

        private int RandomHealthPoints()
        {
            int[] healthPointsArray = null;

            if (GameManager.GameManagerInstance.Difficulty != 0)
            {
              healthPointsArray = healthPointsDictionary[GameManager.GameManagerInstance.Difficulty];
            }
            else
            {
                healthPointsArray = new int[] { 100 };
            }

         
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


        private void EnemyMovement(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GameManager.GameManagerInstance.CurrentLevel == Level.LevelOne)
            {
                if (_timer > 0.01f)
                {
                    _timer = 0;

                    if (this.Position.X >= 350 && this.Position.X < 700 && this.Position.Y >= 270)
                    {
                        Position += new Vector2(1, -1) * Speed * deltaTime;
                        return;
                    }
                    if (this.Position.X >= 1000 && this.Position.X <= 1230)
                    {
                        Position += new Vector2(1, -1) * Speed * deltaTime;
                        return;
                    }

                    Position += new Vector2(1, 0) * Speed * deltaTime;
                }
            }
            else if (GameManager.GameManagerInstance.CurrentLevel == Level.LevelTwo)
            {
                if (_timer > 0.01f)
                {
                    _timer = 0;

                    if (this.Position.X >= 1050 && this.Position.X <= 1150)
                    {
                        Position += new Vector2(1, 1) * Speed * deltaTime;
                        return;
                    }
                    if (this.Position.X >= 1280 && this.Position.X <= 1600)
                    {
                        Position += new Vector2(1, -1) * Speed * deltaTime;
                        return;
                    }

                    Position += new Vector2(1, 0) * Speed * deltaTime;
                }
            }

        }

        public void Unsubscribe()
        {
            MathOperationButton.ChangeMathOperationHandler -= HandleMathOperation;
            MathOperationButton.MathOperationButtonIsClicked -= HandleMathOperationButtonIsClicked;
            GameStateOne.TowerButtonIsClicked -= HandleTowerButtonClicked;
        }
    }
}
