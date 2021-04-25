using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Animations;
using Tower_Defence.Enums;
using Tower_Defence.Interfaces;
using Tower_Defence.States;

namespace Tower_Defence.Sprites
{
    public class Enemy : Sprite, IAttackable, IReachable
    {

        #region Fields
        private float _timer;
        private Vector2 _healthBarOffset;
        #endregion
        public SpriteFont font;

        public int HealthPoints { get; set; }
        public int StartHealth { get; set; }
        public bool HasReachedTarget { get ; set ; }
        public bool idle { get; set; }

        public HealthBar _healthBar;
        public HealthBar _healthBarBackground;

        private Dictionary<Difficulty, int> healthPointsDictionary = new Dictionary<Difficulty, int>()
        {
            { Difficulty.easy, 120 },
            { Difficulty.normal, 140 },
            { Difficulty.hard, 160}
        };
        
        public static event Action<Enemy> EnemyDeathHandler;

        public Enemy(Texture2D texture, int tileRowCount, int tileColumnCount, Texture2D healthBar, Texture2D healthBarBackground) : base(texture)
        {
            _animation = new Animation(texture, tileRowCount, tileColumnCount);
            _animationManager = new AnimationManager(_animation);

            HealthPoints = GetHealthPoints();
            StartHealth = GetHealthPoints();

            _healthBar = new HealthBar(healthBar)
            {
                Position = this.Position
            };
            _healthBarBackground = new HealthBar(healthBarBackground)
            {
                Position = this.Position
            };

            Color = Color.White;
            _healthBarOffset = new Vector2(50, 0);

        }

        private int GetHealthPoints()
        {
           if(GameManager.GameManagerInstance.Difficulty != 0)
            {
                return healthPointsDictionary[GameManager.GameManagerInstance.Difficulty];
            }

            return 0;
        }

        public override void Update(GameTime gameTime, List<IGameParts> gameParts)
        {
            if(!idle)
            {
                EnemyMovement(gameTime);
            }
            _animationManager.Update(gameTime);
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch);

        }

        public void DealDamage(int damagePoints, AttackType attackType)
        {
            HealthPoints -= damagePoints;
            if(HealthPoints <= 0)
            {
                EnemyDeathHandler?.Invoke(this);
                GameManager.GameManagerInstance.StoppedEnemies++;
            }
        }
        private void EnemyMovement(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(GameManager.GameManagerInstance.CurrentLevel == Tower_Defence_MMP1.Enums.Level.LevelOne)
            {
                if (_timer > 0.01f)
                {
                    _timer = 0;
                    this._healthBar.Position = this.Position + _healthBarOffset;
                    this._healthBarBackground.Position = this.Position + _healthBarOffset;

                    if (this.Position.X >= 350 && this.Position.X < 700 && this.Position.Y >= 270)
                    {
                        Position += new Vector2(1, -1);
                        return;
                    }
                    if (this.Position.X >= 700 && this.Position.X <= 1000)
                    {
                        Position += new Vector2(1, 1);
                        return;
                    }

                    Position += new Vector2(1, 0);
                }
            }
            else if (GameManager.GameManagerInstance.CurrentLevel == Tower_Defence_MMP1.Enums.Level.LevelTwo)
            {
                if (_timer > 0.01f)
                {
                    _timer = 0;
                    this._healthBar.Position = this.Position + _healthBarOffset;
                    this._healthBarBackground.Position = this.Position + _healthBarOffset;
                    
                    if (this.Position.X >= 1050 && this.Position.X <= 1150)
                    {
                        Position += new Vector2(1, 1);
                        return;
                    }
                    if (this.Position.X >= 1280 && this.Position.X <= 1600)
                    {
                        Position += new Vector2(1, -1);
                        return;
                    }

                    Position += new Vector2(1, 0);
                }
            }


            //this._healthBar.Position = (this.Position + _healthBarOffset);
            //this._healthBarBackground.Position = (this.Position + _healthBarOffset);

            //if (this.Position.X >= 350 && this.Position.X < 700 && this.Position.Y >= 270)
            //{
            //    Position += new Vector2(100, -100) * deltaTime;
            //    return;
            //}
            //if (this.Position.X >= 700 && this.Position.X <= 1000)
            //{
            //    Position += new Vector2(100, 100) * deltaTime;
            //    return;
            //}

            //Position += new Vector2(100, 0) * deltaTime;

        }

    }
}
