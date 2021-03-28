using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Sprites;

namespace Tower_Defence
{
    public class EndGameHandler : IGameParts
    {
        #region Fields
        private SpriteFont _spriteFont;
        private int _maxLifes;
        private int _stoppedEnemies;
        private string _looseGameText = "You LOST to silly Monsters";

        #endregion

        public event Action gameOverHandler;

        public EndGameHandler(SpriteFont spriteFont)
        {
            _spriteFont = spriteFont;
            _maxLifes = 10;
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
            if (_maxLifes <= 0)
            {
                _maxLifes = 0;
                gameOverHandler?.Invoke();
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_spriteFont, $"{_maxLifes} Lifes", new Vector2(30, 220), Color.White);
            spriteBatch.DrawString(_spriteFont, $"{_stoppedEnemies} Kills", new Vector2(230, 220), Color.White);

            if (_maxLifes <= 0)
            {
                spriteBatch.DrawString(
                    _spriteFont, _looseGameText,
                    new Vector2(Game1.ScreenWidth / 2 - _spriteFont.MeasureString(_looseGameText).X /2, Game1.ScreenHeight / 2),
                    Color.White);
            }
        }
    }
}
