//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tower_Defence.Enums;
using Tower_Defence.Sprites;


namespace Tower_Defence
{
    public sealed class EnemySpawner : IGameParts
    {
        private float _timer;
        private float _mathTimer;
        private float _enemySpawnTime;
        private float _mathEnemySpawnTime;
        private float _countdownTime;
        private bool _startEnemySpawned;
        private int _enemyCounter;
        private int _countdown = 5;
        private Vector2 _enemySpawnPointOne = new Vector2(0, 600);
        private Vector2 _enemySpawnPointTwo = new Vector2(0, 350);
        private List<Texture2D> _levelOneEnemyList = new List<Texture2D>();

        private Texture2D _healthBarTexture;
        private Texture2D _healthBarBackgroundTexture;
        private SpriteFont _spriteFont;
        private SpriteFont _countdownFont;
        private Random random = new Random();

        public static event Action AllEnemiesSpawned; 


        private Dictionary<Difficulty, int> _numberOfEnemies = new Dictionary<Difficulty, int>()
        {
            { Difficulty.easy, 20 },
            { Difficulty.normal, 25 },
            { Difficulty.hard, 30 },

        };

        private Dictionary<Level, Vector2> _spawnPointDictionary = new Dictionary<Level, Vector2>();

        public EnemySpawner(Texture2D[] enemyTextureArray, Texture2D healthBar, Texture2D healthBarBackground, SpriteFont spriteFont, SpriteFont countdownFont)
        {
            foreach (Texture2D texture in enemyTextureArray)
            {
                _levelOneEnemyList.Add(texture);
            }
            _enemySpawnTime = 13f;
            _mathEnemySpawnTime = 11f;
            _healthBarTexture = healthBar;
            _healthBarBackgroundTexture = healthBarBackground;
            _spriteFont = spriteFont;
            _countdownFont = countdownFont;

            _spawnPointDictionary.Add(Level.LevelOne, _enemySpawnPointOne);
            _spawnPointDictionary.Add(Level.LevelTwo, _enemySpawnPointTwo);
        }
        public void Update(GameTime gameTime, List<IGameParts> gameParts, List<Tower> backgroundTowers)
        {
            _countdownTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _mathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_timer >= 1.0 && _countdownTime < 5.0f)
            {
                _countdown--;
                _timer = 0;
            }

            if(_countdownTime >= 5.0f)
            {
                if (_enemyCounter < _numberOfEnemies[GameManager.GameManagerInstance.Difficulty])
                {
                    if (_timer >= _enemySpawnTime || _startEnemySpawned == false)
                    {
                        _timer = 0f;
                        _startEnemySpawned = true;
                        var enemy = SpawnEnemies(gameTime);

                        gameParts.Add(enemy);
                        gameParts.Add(enemy._healthBarBackground);
                        gameParts.Add(enemy._healthBar);
                        _enemyCounter++;
                    }
                    if (_mathTimer >= _mathEnemySpawnTime)
                    {
                        _mathTimer = 0f;

                        var mathEnemy = SpawnMathEnemy(gameTime);

                        gameParts.Add(mathEnemy);
                        _enemyCounter++;
                    }
                }
                else
                {
                    AllEnemiesSpawned?.Invoke();
                }
            }
            
            
        }

        private MathEnemy SpawnMathEnemy(GameTime gameTime)
        {
            int num = random.Next(0, _levelOneEnemyList.Count);

            MathEnemy mathEnemy = null;
            if (num == 0)
            {
                mathEnemy = new MathEnemy(_levelOneEnemyList[0], 5, 4, _healthBarTexture, _healthBarBackgroundTexture, _spriteFont)
                {
                    Position = _spawnPointDictionary[GameManager.GameManagerInstance.CurrentLevel],
                    
                };

            }
            else if( num == 1)
            {
                mathEnemy = new MathEnemy(_levelOneEnemyList[1], 2, 5, _healthBarTexture, _healthBarBackgroundTexture, _spriteFont)
                {
                    Position = _spawnPointDictionary[GameManager.GameManagerInstance.CurrentLevel],                 
                    
                    
                };
            }
            else if (num == 2)
            {
                mathEnemy = new MathEnemy(_levelOneEnemyList[2], 2, 5, _healthBarTexture, _healthBarBackgroundTexture, _spriteFont)
                {
                    Position = _spawnPointDictionary[GameManager.GameManagerInstance.CurrentLevel],
                          

                };
            }

            return mathEnemy;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(_countdownTime < 5.0f)
            {
                spriteBatch.DrawString(_countdownFont, _countdown.ToString(),
                new Vector2(Game1.ScreenWidth / 2 - _countdownFont.MeasureString(_countdown.ToString()).X, Game1.ScreenHeight / 2 - _countdownFont.MeasureString(_countdown.ToString()).Y), Color.White);
            }
            
        }


        private Enemy SpawnEnemies(GameTime gameTime)
        {
            
            int num = random.Next(0, _levelOneEnemyList.Count);

            Enemy enemy = null;
            if (num == 0)
            {
                enemy = new Enemy(_levelOneEnemyList[0], 5, 4, _healthBarTexture, _healthBarBackgroundTexture)
                {
                    Position = _spawnPointDictionary[GameManager.GameManagerInstance.CurrentLevel]
                };
            }
            else if( num == 1)
            {
                enemy = new Enemy(_levelOneEnemyList[1], 2, 5, _healthBarTexture, _healthBarBackgroundTexture)
                {
                    Position = _spawnPointDictionary[GameManager.GameManagerInstance.CurrentLevel]
                };
            }
            else if (num == 2)
            {
                enemy = new Enemy(_levelOneEnemyList[2], 2, 5, _healthBarTexture, _healthBarBackgroundTexture)
                {
                    Position = _spawnPointDictionary[GameManager.GameManagerInstance.CurrentLevel]
                };
            }
            return enemy;
        }
    }
}

