using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Enums;
using Tower_Defence.Sprites;
using Tower_Defence.States;

namespace Tower_Defence
{
    public class EndGameHandler : IGameParts
    {
        #region Fields
        private SpriteFont _gameFont;
        private SpriteFont _menuFont;
        private int _maxLifes;
        private int _stoppedEnemies;
        private bool _died;
        private bool _won;
        private bool _allEnemiesSpawned;

        private Dictionary<Difficulty, int> _maxLifesDictionary = new Dictionary<Difficulty, int>()
        {
            { Difficulty.easy, 5 },
            { Difficulty.normal, 4 },
            { Difficulty.hard, 2 }
        };

        #endregion

        public event Action<bool> endGameHandler;

        public EndGameHandler(SpriteFont spriteFont, SpriteFont menuFont)
        {
            _menuFont = menuFont;
            _gameFont = spriteFont;
            _maxLifes = GetMaxLifes();
            _stoppedEnemies = 0;

            EnemySpawner.AllEnemiesSpawned += HandleAllEnemiesSpawned;
        }

        private int GetMaxLifes()
        {
            return _maxLifesDictionary[GameManager.GameManagerInstance.Difficulty];
        }

        private void HandleAllEnemiesSpawned()
        {
            _allEnemiesSpawned = true;
        }

        public void Update(GameTime gameTime, List<IGameParts> gameParts)
        {
            _stoppedEnemies = GameManager.GameManagerInstance.StoppedEnemies;

            if(_allEnemiesSpawned & !_won)
            {
                CheckIfEnemiesLeft(gameParts);
            }

            foreach (IGameParts gamePart in gameParts.ToArray())
            {
                if (gamePart is Enemy enemy)
                {
                    if (enemy.Position.X == Game1.ScreenWidth && !enemy.HasReachedTarget)
                    {
                        enemy.HasReachedTarget = true;
                        _maxLifes--;
                        gameParts.Remove(enemy);
                        gameParts.Remove(enemy._healthBar);
                        gameParts.Remove(enemy._healthBarBackground);
                    }
                }
                if (gamePart is MathEnemy mathEnemy)
                {
                    if (mathEnemy.Position.X == Game1.ScreenWidth && !mathEnemy.HasReachedTarget)
                    {
                        mathEnemy.HasReachedTarget = true;
                        _maxLifes--;
                        gameParts.Remove(mathEnemy);
                    }
                }
            }
            if (_maxLifes <= 0 && _died == false)
            {
                _died = true;
                _maxLifes = 0;
                endGameHandler?.Invoke(_won);
            }
        }

        private void CheckIfEnemiesLeft(List<IGameParts> gameParts)
        {
            foreach (IGameParts gamePart in gameParts)
            {
                if(gamePart is Enemy || gamePart is MathEnemy) { return; }
            }
            _won = true;
            endGameHandler?.Invoke(_won);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_menuFont, $"{_maxLifes} Lifes", new Vector2(40, 220), Color.White);
            spriteBatch.DrawString(_menuFont, $"{_stoppedEnemies} Kills", new Vector2(40, 300), Color.White);
        }
    }
}
