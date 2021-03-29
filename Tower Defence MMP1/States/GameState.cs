using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Buttons;
using Tower_Defence.Sprites;
using Microsoft.Xna.Framework.Input;
using Tower_Defence.Enums;
using Microsoft.Xna.Framework.Media;

namespace Tower_Defence.States
{
    public class GameState : State
    {
        #region Fields

        #region Miscallenous textures
        private Texture2D _levelMap;
        private Texture2D _statsTable;
        private Texture2D _tableGUI;
        private Texture2D _healthBar;
        private Texture2D _healthBarBackground;
        #endregion

        #region Buttons/Weapons textures
        private Texture2D _backToMenuButton;
        private Texture2D _pauseButton;
        private Texture2D _restartButton;
        private Texture2D _archerTowerButton;
        private Texture2D _archerTower;
        private Texture2D _archerWeapon;
        private Texture2D _fireTowerButton;
        private Texture2D _fireTower;
        private Texture2D _fireWeapon;
        private Texture2D _mathOperationButton;
        #endregion

        #region Cursor textures
        private Texture2D _mouseCursor;
        private Texture2D _mouseCursorStandard;
        private Texture2D _mouseCursorSubtraction;
        private Texture2D _mouseCursorAddition;
        private Texture2D _mouseCursorDivision;
        private Texture2D _mouseCursorSquareRoot;
        #endregion

        private Difficulty _difficulty;
        private MathOperation _mathOperation;
        private MouseState _currentMouse;
        private Song _gameSong;

        private SpriteFont _gameFont;
        private SpriteFont _menuFont;
        private bool _pauseGame;
        private bool _isGameOver;

        public static bool _towerButtonIsClicked;
        public static bool _mathOperationButtonIsClicked;


        private List<IGameParts> _gameParts;
        private Texture2D[] _enemyTextureArray;
        #region Dictionaries
        private Dictionary<AttackType, Texture2D> _towerTextures = new Dictionary<AttackType, Texture2D>();
        private Dictionary<AttackType, Texture2D> _weaponTextures = new Dictionary<AttackType, Texture2D>();
        private Dictionary<MathOperation, Texture2D> _mathOperations = new Dictionary<MathOperation, Texture2D>();

        #endregion

        private Queue<Tower> _towerQueue = new Queue<Tower>();

        private EndGameHandler endgameHandler;

        public static event Action<bool> TowerButtonIsClicked;


        #region Enemy
        private Texture2D _enemy01_walkTile;
        private Texture2D _enemy02_walkTile;
        #endregion

        #endregion

        public GameState(Game1 game1, GraphicsDeviceManager graphics, ContentManager content, Difficulty difficulty) : base(game1, graphics, content)
        {
            _difficulty = difficulty;
            _mathOperation = MathOperation.subtraction;
        }
        public List<IGameParts> GetGamePartsList()
        {
            return _gameParts;
        }

        public override void LoadContent()
        {

            _levelMap = _content.Load<Texture2D>("Background/levelMap");
            _statsTable = _content.Load<Texture2D>("GameItems/statsTable");
            _tableGUI = _content.Load<Texture2D>("GameItems/tableGUI");

            _backToMenuButton = _content.Load<Texture2D>("MenuButtons/closeButton");
            _pauseButton = _content.Load<Texture2D>("MenuButtons/pauseButton");
            _restartButton = _content.Load<Texture2D>("MenuButtons/restartButton");

            _archerTowerButton = _content.Load<Texture2D>("TowerButtons/buildTower");
            _fireTowerButton = _content.Load<Texture2D>("TowerButtons/fireTowerButton");

            _archerTower = _content.Load<Texture2D>("Towers/archerTower");
            _archerWeapon = _content.Load<Texture2D>("Weapons/archerWeapon");
            _fireTower = _content.Load<Texture2D>("Towers/fireTower");
            _fireWeapon = _content.Load<Texture2D>("Weapons/fireWeapon");
            _mathOperationButton = _content.Load<Texture2D>("TowerButtons/mathButton");

            _healthBar = _content.Load<Texture2D>("GameItems/healthBar");
            _healthBarBackground = _content.Load<Texture2D>("GameItems/healthBarBackground");

            _enemy01_walkTile = _content.Load<Texture2D>("Enemies/enemy01_walkTile");
            _enemy02_walkTile = _content.Load<Texture2D>("Enemies/enemy02_walkTile");

            _gameFont = _content.Load<SpriteFont>("MenuFont/endGameFont");
            _menuFont = _content.Load<SpriteFont>("MenuFont/menuFont");

            _mouseCursorStandard = _content.Load<Texture2D>("MenuButtons/mouse");
            _mouseCursorSubtraction = _content.Load<Texture2D>("GameItems/subtractionMouse");
            _mouseCursorAddition = _content.Load<Texture2D>("GameItems/additionMouse");
            _mouseCursorDivision = _content.Load<Texture2D>("GameItems/divisionMouse");
            _mouseCursorSquareRoot = _content.Load<Texture2D>("GameItems/squareMouse");
            _gameSong = _content.Load<Song>("GameSound/gameSong");

            MediaPlayer.Play(_gameSong);
            MediaPlayer.IsRepeating = true;
            _mouseCursor = _mouseCursorStandard;

            _towerTextures.Add(AttackType.archer, _archerTower);
            _towerTextures.Add(AttackType.fire, _fireTower);

            _weaponTextures.Add(AttackType.archer, _archerWeapon);
            _weaponTextures.Add(AttackType.fire, _fireWeapon);

            _mathOperations.Add(MathOperation.subtraction, _mouseCursorSubtraction);
            _mathOperations.Add(MathOperation.addition, _mouseCursorAddition);
            _mathOperations.Add(MathOperation.division, _mouseCursorDivision);
            _mathOperations.Add(MathOperation.squareroot, _mouseCursorSquareRoot);

            _enemyTextureArray = new Texture2D[]
            { _enemy01_walkTile,
              _enemy02_walkTile
            };

            Enemy.EnemyDeathHandler += HandleEnemyDeath;
            MathEnemy.MathEnemyDeathHandler += HandleMathEnemyDeath;
            MathOperationButton.mathOperationButtonIsClicked += HandleMathOperationButtonIsClicked;

            MenuButton backToMenuButton = new MenuButton(_backToMenuButton, _gameFont)
            {
                Position = new Vector2(Game1.ScreenWidth - _backToMenuButton.Width, 0)
            };

            backToMenuButton.menuButtonEventHandler += HandleBackToMenuButtonClicked;

            MenuButton pauseButton = new MenuButton(_pauseButton, _gameFont)
            {
                Position = new Vector2(Game1.ScreenWidth - 300, 30),
                Scale = 0.7f
            };

            pauseButton.menuButtonEventHandler += HandlePauseButtonClicked;

            TowerButton archerButton = new TowerButton(_archerTowerButton, AttackType.archer)
            {
                Position = new Vector2(50, 30),
                Scale = 0.7f
            };

            archerButton.PreviewTowerEventHandler += HandleTowerPreview;
            archerButton.PlaceTowerEventHandler += HandlePlaceTower;
            archerButton.TurnTowerPreviewOffEventHandler += HandleTurnPreviewOff;

            TowerButton fireButton = new TowerButton(_fireTowerButton, AttackType.fire)
            {
                Position = new Vector2(250, 30),
                Scale = 0.7f,
                Font = _menuFont
            };

            fireButton.PreviewTowerEventHandler += HandleTowerPreview;
            fireButton.PlaceTowerEventHandler += HandlePlaceTower;
            fireButton.TurnTowerPreviewOffEventHandler += HandleTurnPreviewOff;

            MathOperationButton subtractionButton = new MathOperationButton(_mathOperationButton, MathOperation.subtraction)
            {
                Position = new Vector2(450, 30),
                Scale = 0.7f,
                Font = _menuFont,
                MathButtonText = "-1"

            };

            MathOperationButton divisionButton = new MathOperationButton(_mathOperationButton, MathOperation.division)
            {
                Position = new Vector2(650, 30),
                Scale = 0.7f,
                Font = _menuFont,
                MathButtonText = "/2",
                CoolDownTime = 5f,

            };

            MathOperationButton additionButton = new MathOperationButton(_mathOperationButton, MathOperation.addition)
            {
                Position = new Vector2(850, 30),
                Scale = 0.7f,
                Font = _menuFont,
                MathButtonText = "+1"
            };

            MathOperationButton squareRootButton = new MathOperationButton(_mathOperationButton, MathOperation.squareroot)
            {
                Position = new Vector2(1050, 30),
                Scale = 0.7f,
                Font = _menuFont,
                MathButtonText = "sqrt",
                CoolDownTime = 10f,

            };

            EnemySpawner enemySpawner = new EnemySpawner(_enemyTextureArray, _healthBar, _healthBarBackground, _menuFont);
            endgameHandler = new EndGameHandler(_gameFont, _menuFont);

            endgameHandler.gameOverHandler += HandleGameOver;

            _gameParts = new List<IGameParts>()
            {
                backToMenuButton,
                pauseButton,
                archerButton,
                fireButton,
                enemySpawner,
                endgameHandler,
                subtractionButton,
                divisionButton,
                additionButton,
                squareRootButton
            };
        }

        private void HandleMathOperationButtonIsClicked(MathOperation mathOperation, bool clicked)
        {
            _mathOperationButtonIsClicked = clicked;
            if (clicked)
            {
                _mouseCursor = _mathOperations[mathOperation];
            }
            else
            {
                _mouseCursor = _mouseCursorStandard;
            }
        }

        private void HandleEnemyDeath(Enemy enemy)
        {
            _gameParts.Remove(enemy);
            _gameParts.Remove(enemy._healthBar);
            _gameParts.Remove(enemy._healthBarBackground);
            endgameHandler.SetStoppedEnemies();

        }

        private void HandleMathEnemyDeath(MathEnemy enemy)
        {
            _gameParts.Remove(enemy);
            endgameHandler.SetStoppedEnemies();

        }

        private void HandleGameOver()
        {
            _isGameOver = true;

            MenuButton restartButton = new MenuButton(_restartButton, _menuFont)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - _restartButton.Width / 2, Game1.ScreenHeight / 2 - _restartButton.Height / 2)
            };

            restartButton.menuButtonEventHandler += HandleRestartButtonClicked;

            _gameParts.Add(restartButton);
        }

        private void HandleRestartButtonClicked(bool clicked)
        {
            _game1.ChangeState(new GameState(_game1, _graphics, _content, _difficulty));
        }

        public override void Update(GameTime gameTime)
        {

            //System.Diagnostics.Debug.WriteLine(_towerButtonIsClicked);
            if (_pauseGame || _isGameOver)
            {
                foreach (IGameParts gamePart in _gameParts)
                {
                    if (gamePart is MenuButton)
                        gamePart.Update(gameTime, _gameParts);

                }
                return;
            }
            if(_isGameOver == false)
            {
                foreach (IGameParts gamePart in _gameParts.ToArray())
                {
                    gamePart.Update(gameTime, _gameParts);
                }

            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            var x = Mouse.GetState().X;
            var y = Mouse.GetState().Y;

            spritebatch.Draw(_levelMap, new Vector2(0, 0), Color.White);
            spritebatch.Draw(_tableGUI, new Rectangle(0, 0, Game1.ScreenWidth, 100), Color.White);
            spritebatch.Draw(
                _statsTable,
                new Vector2(10, 200),
                null, Color.White,
                0f,
                new Vector2(0, 0),
                0.7f,
                SpriteEffects.None, 0f);
            spritebatch.DrawString(_menuFont, $"{x}:{y}", new Vector2(50, 600), Color.White);

            if (_pauseGame)
            {
                foreach (IGameParts gamePart in _gameParts)
                {
                    if (gamePart is MenuButton)
                    {
                        gamePart.Draw(gameTime, spritebatch);
                    }
                }
                spritebatch.Draw(_mouseCursor, new Rectangle(x, y, _mouseCursor.Width, _mouseCursor.Height), null, Color.White, -2.0f, new Vector2(0, 0), SpriteEffects.None, 0f);
                return;
            }

            if (_isGameOver == false)
            {
                foreach (IGameParts gamePart in _gameParts)
                {
                    gamePart.Draw(gameTime, spritebatch);
                }
            }
            else
            {
                foreach (IGameParts gamePart in _gameParts)
                {
                    if (gamePart is MenuButton || gamePart is EndGameHandler)
                    {
                        gamePart.Draw(gameTime, spritebatch);
                    }
                }
            }

            spritebatch.Draw(_mouseCursor, new Rectangle(x, y, _mouseCursor.Width, _mouseCursor.Height), null, Color.White, -2.0f, new Vector2(0, 0), SpriteEffects.None, 0f);
        }

        private void HandleBackToMenuButtonClicked(bool clicked)
        {
            _game1.ChangeState(new MenuState(_game1, _graphics, _content));
           
        }

        private void HandlePauseButtonClicked(bool clicked)
        {
            _pauseGame = !_pauseGame;
        }

        private void HandleTowerPreview(AttackType towerType)
        {
            TowerButtonIsClicked?.Invoke(true);
            _towerButtonIsClicked = true;

            _currentMouse = Mouse.GetState();
            Texture2D towerTexture = _towerTextures[towerType];
            Texture2D weaponTexture = _weaponTextures[towerType];

            float towerStartRange = 0;
            float towerMaxRange = 0;

            if (towerType == AttackType.archer)
            {
               towerStartRange = 450f;
               towerMaxRange = 1500f;

             }
            else if(towerType == AttackType.fire)
            {
                towerStartRange = 0f;
                towerMaxRange = 350f;
            }

            Tower newTower = new Tower(towerTexture)
            {
                Position = new Vector2(_currentMouse.X, _currentMouse.Y),
                weapon = new Weapon(weaponTexture, towerType),
                TowerMaxRange = towerMaxRange,
                TowerStartRange = towerStartRange
                
        };


            _gameParts.Add(newTower);
        }
        private void HandleTurnPreviewOff()
        {
            _gameParts.Remove((Tower)_gameParts.FindLast(t => t is Tower));
            TowerButtonIsClicked?.Invoke(false);
            _towerButtonIsClicked = false;
        }

        private void HandlePlaceTower(TowerButton towerButton)
        {
            _currentMouse = Mouse.GetState();

            var tower = (Tower)_gameParts.FindLast(t => t is Tower);

            if (tower.isPlacable)
            {
                tower.Position = new Vector2(_currentMouse.X, _currentMouse.Y);
                tower.Rectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, tower.Texture.Width, tower.Texture.Height);
                tower._weaponSpawnPoint = new Vector2(0, 0);
                tower.isPreview = false;
                tower.isPlaced = true;

                CheckNumberOfTowers(tower);
                TowerButtonIsClicked?.Invoke(false);
                _towerButtonIsClicked = false;
                return;
            }
            towerButton.isPreviewOn = true;
        }

        private void CheckNumberOfTowers(Tower tower)
        {
            _towerQueue.Enqueue(tower);
            if (_towerQueue.Count > 2)
            {
                var removeTower = _towerQueue.Dequeue();
                _gameParts.Remove(removeTower);
            }

        }
    }
}
