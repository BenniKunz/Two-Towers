using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Animations;
using Tower_Defence.Interfaces;

namespace Tower_Defence.Sprites
{
    public class Enemy : Sprite, IAttackable, IReachable
    {

        #region Fields
        private float _timer;
        #endregion
        public SpriteFont font;

        public int HealthPoints { get; set; }
        public int StartHealth { get; set; }
        public bool HasReachedTarget { get ; set ; }

        public HealthBar _healthBar;
        public HealthBar _healthBarBackground;
        private Vector2 _healthBarOffset;
        
        public static event Action<Enemy> EnemyDeathHandler;

        public Enemy(Texture2D texture, int tileRowCount, int tileColumnCount, Texture2D healthBar, Texture2D healthBarBackground) : base(texture)
        {
            _animation = new Animation(texture, tileRowCount, tileColumnCount);
            _animationManager = new AnimationManager(_animation);
            
            HealthPoints = 100;
            StartHealth = 100;

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


        public override void Update(GameTime gameTime, List<IGameParts> gameParts)
        {
            EnemyMovement(gameTime);
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
            }
        }
        private void EnemyMovement(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

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
