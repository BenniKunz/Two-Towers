using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Enums;
using Tower_Defence.Sprites;

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
        private string _looseGameText = "You LOST to silly Monsters!!!";

        #endregion

        public event Action gameOverHandler;

        public EndGameHandler(SpriteFont spriteFont, SpriteFont menuFont)
        {
            _menuFont = menuFont;
            _gameFont = spriteFont;
            _maxLifes = 1;
            _stoppedEnemies = 0;
        }

        public void SetStoppedEnemies()
        {
            _stoppedEnemies++;
        }


        public void Update(GameTime gameTime, List<IGameParts> gameParts)
        {

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
                gameOverHandler?.Invoke();
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_menuFont, $"{_maxLifes} Lifes", new Vector2(30, 220), Color.White);
            spriteBatch.DrawString(_menuFont, $"{_stoppedEnemies} Kills", new Vector2(230, 220), Color.White);

            if (_maxLifes <= 0)
            {
                spriteBatch.DrawString(
                    _gameFont, _looseGameText,
                    new Vector2(Game1.ScreenWidth / 2 - _gameFont.MeasureString(_looseGameText).X /2, 300),
                    Color.Red);
            }
        }
    }
}
