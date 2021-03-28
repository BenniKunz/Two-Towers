using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Sprites;
using Tower_Defence.States;

namespace Tower_Defence
{
    public class EnemySpawner : IGameParts
    {
        private float _timer;
        private float _mathTimer;
        private float _enemySpawnTime;
        private float _mathEnemySpawnTime;
        private Vector2 _enemySpawnPoint = new Vector2(0, 600);
        private List<Texture2D> _levelOneEnemyList = new List<Texture2D>();
        private int counter = 0;

        private Texture2D _healthBarTexture;
        private Texture2D _healthBarBackgroundTexture;
        private SpriteFont _spriteFont;

        public EnemySpawner(Texture2D[] enemyTextureArray, Texture2D healthBar, Texture2D healthBarBackground, SpriteFont spriteFont)
        {
            foreach (Texture2D texture in enemyTextureArray)
            {
                _levelOneEnemyList.Add(texture);
            }
            _enemySpawnTime = 12f;
            _mathEnemySpawnTime = 7f;
            _healthBarTexture = healthBar;
            _healthBarBackgroundTexture = healthBarBackground;
            _spriteFont = spriteFont;
        }
        public void Update(GameTime gameTime, List<IGameParts> gameParts)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _mathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= _enemySpawnTime && counter < 1 )
            {
                _timer = 0f;
                var enemy = SpawnEnemies(gameTime);
                
                gameParts.Add(enemy);
                gameParts.Add(enemy._healthBarBackground);
                gameParts.Add(enemy._healthBar);
                counter++;
            }
            if (_mathTimer >= _mathEnemySpawnTime)
            {
                _mathTimer = 0f;
                
                var mathEnemy = SpawnMathEnemy(gameTime);

                gameParts.Add(mathEnemy);

            }
        }

        private MathEnemy SpawnMathEnemy(GameTime gameTime)
        {
            MathEnemy mathEnemy = new MathEnemy(_levelOneEnemyList[0], 5, 4, _healthBarTexture, _healthBarBackgroundTexture, _spriteFont)
            {
                Position = _enemySpawnPoint,
                TowerButtonIsClicked = GameState._towerButtonIsClicked,
                MathOperationButtonIsClicked = GameState._mathOperationButtonIsClicked
            };

            return mathEnemy;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }


        private Enemy SpawnEnemies(GameTime gameTime)
        {
            Random random = new Random();
            int num = random.Next(0, 2);

            Enemy enemy = null;
            if (num == 0)
            {
                enemy = new Enemy(_levelOneEnemyList[0], 5, 4, _healthBarTexture, _healthBarBackgroundTexture)
                {
                    Position = _enemySpawnPoint
                };
            }
            else if( num == 1)
            {
                enemy = new Enemy(_levelOneEnemyList[1], 2, 5, _healthBarTexture, _healthBarBackgroundTexture)
                {
                    Position = _enemySpawnPoint
                };
            }
            return enemy;
        }
    }
}

