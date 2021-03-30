using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tower_Defence.Sprites
{
    public class Weapon : Sprite
    {
        private float _lifeSpan;
        private float _lifeSpanTimer;
        private int _damagePoints;
        private int Speed;
        private AttackType _attackType;


        public Weapon(Texture2D texture, AttackType attackType) : base(texture)
        {
            _attackType = attackType;
            Color = Color.White;
            _lifeSpan = 2f;
            _damagePoints = 10;
            Speed = 10;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color, 0.5f, _zeroPosition, 1f, SpriteEffects.None, 0f);
            
        }

        public override void Update(GameTime gameTime, List<IGameParts> gameParts)
        {
            //float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Direction * Speed;

            _lifeSpanTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_lifeSpanTimer >= _lifeSpan)
            {
                gameParts.Remove(this);
            }


            foreach (IGameParts gamePart in gameParts.ToArray())
            {
                if (gamePart is Enemy enemy)
                {
                    bool collided = AnimatedSpriteCollision(Rectangle, null, _texture, enemy._animationManager.destinationRectangle, enemy._animationManager.sourceRectangle, enemy.Texture);
                    if (collided)
                    {
                        gameParts.Remove(this);
                        enemy.DealDamage(_damagePoints, _attackType);

                        float damagePercent = MathF.Round((float)_damagePoints / enemy.StartHealth, 2);
                        //System.Diagnostics.Debug.WriteLine("EnemyPosition:" + damagePercent);
                        enemy._healthBar.Offset -= damagePercent;
                    }
                }
            }
        }

        //idea of per pixel collision from: https://gamedev.stackexchange.com/questions/161854/2d-per-pixel-collision-detection-using-animated-sprite
        private bool AnimatedSpriteCollision(
            Rectangle destinationWeapon, Rectangle? sourceWeapon, Texture2D textureWeapon, Rectangle destinationEnemy, Rectangle sourceEnemy, Texture2D textureEnemy)
        {

            if(sourceWeapon == null)
            {
                sourceWeapon = new Rectangle(0, 0, textureWeapon.Width, textureWeapon.Height);
            }

            Color[] textureDataWeapon = new Color[sourceWeapon.Value.Width * sourceWeapon.Value.Height];
            textureWeapon.GetData(0, sourceWeapon, textureDataWeapon,0, textureDataWeapon.Length);

            if(sourceEnemy == null)
            {
                sourceEnemy = new Rectangle(0, 0, textureEnemy.Width, textureEnemy.Height);
            }

            
            if(sourceEnemy == null || sourceEnemy.Width == 0) { return false; }

            Color[] textureEnemyData = new Color[sourceEnemy.Width * sourceEnemy.Height];
            textureEnemy.GetData(0, sourceEnemy, textureEnemyData, 0, textureEnemyData.Length);

            int top = Math.Max(destinationWeapon.Top, destinationEnemy.Top);
            int bottom = Math.Min(destinationWeapon.Bottom, destinationEnemy.Bottom);
            int left = Math.Max(destinationWeapon.Left, destinationEnemy.Left);
            int right = Math.Min(destinationWeapon.Right, destinationEnemy.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Code Review
                    Color colorA = textureDataWeapon[(x - destinationWeapon.Left) +
                                            (y - destinationWeapon.Top) * destinationWeapon.Width];
                    Color colorB = textureEnemyData[(x - destinationEnemy.Left) +
                                            (y - destinationEnemy.Top) * destinationEnemy.Width];

                    
                    if (colorA.A > 0 && colorB.A > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
