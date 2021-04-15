using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Tower_Defence.States;

namespace Tower_Defence.Sprites
{
    public class Tower : Sprite
    {
        private Rectangle towerPlacableRectangleOne;
        private Rectangle towerPlacableRectangleTwo;
        private Rectangle towerPlacableRectangleThree;
        private Rectangle towerPlacableRectangleFour;
        private Rectangle towerPlacableRectangleFive;
        private Rectangle towerPlacableRectangleSix;
        private Rectangle towerPlacableRectangleSeven;
        private Rectangle towerPlacableRectangleEight;
        private Rectangle towerPlacableRectangleNine;
        private Enemy _targetEnemy = null;

        private List<Rectangle> _towerPlacableRectangles = new List<Rectangle>();

        private Vector2 _tempTowerPosition = new Vector2();

        public Vector2 _weaponSpawnPoint;

        public float TowerMaxRange { get; set; }
        public float TowerStartRange { get; set; }

        public bool isPreview = true;
        public bool isPlacable = false;
        public bool isPlaced = false;
        private bool _isFirstShot = true;
        AttackType _attackType;

        public Weapon weapon;
        private float _timer;

        public Tower(Texture2D texture = null) : base(texture)
        {
            towerPlacableRectangleOne = new Rectangle(80, 1050 - _texture.Height, _texture.Width, _texture.Height);
            towerPlacableRectangleTwo = new Rectangle(478, 1050 - _texture.Height, _texture.Width, _texture.Height);
            towerPlacableRectangleThree = new Rectangle(730, 700 - _texture.Height, _texture.Width, _texture.Height);
            towerPlacableRectangleFour = new Rectangle(965, 480 - _texture.Height, _texture.Width, _texture.Height);
            towerPlacableRectangleFive = new Rectangle(1110, 740 - _texture.Height, _texture.Width, _texture.Height);
            towerPlacableRectangleSix = new Rectangle(1330, 435 - _texture.Height, _texture.Width, _texture.Height);
            towerPlacableRectangleSeven = new Rectangle(1660, 515 - _texture.Height, _texture.Width, _texture.Height);
            towerPlacableRectangleEight = new Rectangle(1220, 1050 - _texture.Height, _texture.Width, _texture.Height);
            towerPlacableRectangleNine = new Rectangle(1615, 1050 - _texture.Height, _texture.Width, _texture.Height);

        }

        public override void Update(GameTime gameTime, List<IGameParts> gameParts)
        {

            if (_towerPlacableRectangles.Count == 0)
            {
                _towerPlacableRectangles = new List<Rectangle>
                {
                    towerPlacableRectangleOne,
                    towerPlacableRectangleTwo,
                    towerPlacableRectangleThree,
                    towerPlacableRectangleFour,
                    towerPlacableRectangleFive,
                    towerPlacableRectangleSix,
                    towerPlacableRectangleSeven,
                    towerPlacableRectangleEight,
                    towerPlacableRectangleNine
                };
            }

            if (_weaponSpawnPoint == _zeroPosition)
            {
                _weaponSpawnPoint = Position;
            }
            _currentMouse = Mouse.GetState();

            if (!isPreview)
            {
                SetTarget(gameParts);
                Shoot(gameTime, gameParts);
                Color = Color.White;
            }

            if (isPreview)
            {
                _tempTowerPosition.X = _currentMouse.X;
                _tempTowerPosition.Y = _currentMouse.Y;
                Position = _tempTowerPosition;

                foreach (var gamePart in gameParts)
                {
                    if (gamePart == this) { continue; }
                    if (gamePart is Tower tower)
                    {
                        if (this.Rectangle.Intersects(tower.Rectangle) && !this.isPlacable)
                        {
                            this.Color = Color.Red;
                            isPlacable = false;
                            return;
                        }
                    }
                }

                foreach (Rectangle towerPlacableRectangle in _towerPlacableRectangles)
                {
                    if (this.isPlaced) { return; }

                    var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
                    if (!mouseRectangle.Intersects(towerPlacableRectangle))
                    {
                        this.Color = Color.Peru;
                        isPlacable = false;
                    }
                    else if (mouseRectangle.Intersects(towerPlacableRectangle))
                    {
                        this.Color = Color.Green;
                        isPlacable = true;
                        return;
                    }
                }
            }
        }

        public bool CheckIfTowerIsBackgroundTower()
        {
            if (this.Rectangle.Intersects(towerPlacableRectangleFour) || this.Rectangle.Intersects(towerPlacableRectangleFive))
            {
                return true;
            }
            return false;
        }

        private void Shoot(GameTime gameTime, List<IGameParts> gameParts)
        {

            if (_targetEnemy != null)
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!gameParts.Contains(_targetEnemy))
                {
                    _targetEnemy = null;
                    _timer = 0;
                    return;
                }

                if (_timer >= 2f || _isFirstShot == true)
                {
                    _isFirstShot = false;
                    Vector2 enemyPosition = _targetEnemy._animationManager.AttackPosition;
                    Vector2 directionVector = enemyPosition - this._weaponSpawnPoint;

                    Weapon weapon = new Weapon(this.weapon.Texture, _attackType)
                    {
                        Position = _weaponSpawnPoint,
                        Direction = Vector2.Normalize(directionVector)
                    };
                    gameParts.Add(weapon);

                    _timer = 0f;
                }
            }
        }

        private void SetTarget(List<IGameParts> gameParts)
        {
            Vector2 towerPosition = this.Position;

            if (_targetEnemy != null)
            {
                Vector2 enemyPosition = _targetEnemy._animationManager.AttackPosition;
                Vector2 directionVector = enemyPosition - towerPosition;

                float distance = MathF.Sqrt(directionVector.X * directionVector.X + directionVector.Y * directionVector.Y);

                if (distance > TowerMaxRange || distance < TowerStartRange)
                {
                    _targetEnemy = null;
                    _isFirstShot = true;
                }
            }


            if (_targetEnemy == null)
            {
                foreach (IGameParts gamePart in gameParts.ToArray())
                {
                    if (gamePart is IAttackable)  // why doesnt this work?: gamePart.GetType() == typeof(IAttackable)
                    {
                        Vector2 enemyPosition = ((Enemy)(gamePart))._animationManager.AttackPosition;
                        
                        Vector2 directionVector = enemyPosition - towerPosition;
                        
                        float distance = MathF.Sqrt(directionVector.X * directionVector.X + directionVector.Y * directionVector.Y);
                       
                        if (distance > TowerMaxRange || distance < TowerStartRange) { continue; }
                        _targetEnemy = (Enemy)(gamePart);
                        return;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

        }
    }
}
