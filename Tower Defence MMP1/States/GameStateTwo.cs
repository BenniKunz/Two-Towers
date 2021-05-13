//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

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
using Tower_Defence.Interfaces;
using Microsoft.Xna.Framework.Audio;
using Tower_Defence_MMP1.Enums;

namespace Tower_Defence.States
{
    public class GameStateTwo : State, IUnsubscribable
    {
        #region Fields

        #region Miscellaneous textures
        private Texture2D _levelMap;
        private Texture2D _statsTable;
        private Texture2D _tableGUI;
        private Texture2D _endTable;
        private Texture2D _failed;
        private Texture2D _win;
        private Texture2D _healthBar;
        private Texture2D _healthBarBackground;
        #endregion

        #region Buttons/Weapons textures
        private Texture2D _backToMenuButton;
        private Texture2D _backButton;
        private Texture2D _pauseButton;
        private Texture2D _restartButton;
        private Texture2D _nextLevelButton;
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

        #region Dictionaries
        private Dictionary<AttackType, Texture2D> _towerTextures = new Dictionary<AttackType, Texture2D>();
        private Dictionary<AttackType, Texture2D> _weaponTextures = new Dictionary<AttackType, Texture2D>();
        private Dictionary<MathOperation, Texture2D> _mathOperations = new Dictionary<MathOperation, Texture2D>();

        #endregion

        #region EnemyTextures
        private Texture2D _enemy01_walkTile;
        private Texture2D _enemy02_walkTile;
        private Texture2D _enemy03_walkTile;
        #endregion

        private MouseState _currentMouse;
        private Song _gameSong;
        private SoundEffect _mathOperationSound;
        private SoundEffect _enemyHitSound;

        private SpriteFont _gameFont;
        private SpriteFont _menuFont;

        private Vector2 _zeroPosition = new Vector2(0, 0);
        private Vector2 _statsTablePosition = new Vector2(10, 200);
        private Vector2 _endTablePosition;
        private Vector2 _failedPosition;
        private Rectangle _tableGUIRectangle = new Rectangle(0, 0, Game1.ScreenWidth, 100);
        private Rectangle _currentMouseRectangle = new Rectangle();

        private float _towerStartRangeArcher = 250f;
        private float _towerMaxRangeArcher = 520f;
        private float _towerStartRangeFire = 0f;
        private float _towerMaxRangeFire = 350f;

        private bool _pauseGame;
        private bool _isGameOver;
        private bool _gameWon;

        private List<IGameParts> _gameParts;
        private List<Tower> _backgroundTowers = new List<Tower>();
        private Texture2D[] _enemyTextureArray;

        private Queue<Tower> _towerQueue = new Queue<Tower>();

        private EndGameHandler endgameHandler;

        public static MathOperation _mathOperation;
        public static bool _towerButtonIsClicked;
        public static bool _mathOperationButtonIsClicked;
        public static event Action<bool> TowerButtonIsClicked;

        #endregion

        #region Buttons
        MenuButton backToMenuButton;
        MenuButton backButton;
        MenuButton pauseButton;
        TowerButton archerButton;
        TowerButton fireButton;
        MathOperationButton subtractionButton;
        MathOperationButton additionButton;
        MathOperationButton divisionButton;
        MathOperationButton squareRootButton;
        #endregion

        public GameStateTwo(Game1 game1, GraphicsDeviceManager graphics, ContentManager content, Difficulty difficulty) : base(game1, graphics, content)
        {
            GameManager.GameManagerInstance.Difficulty = difficulty;
            
            _mathOperation = MathOperation.setDefault;
        }
        public List<IGameParts> GetGamePartsList()
        {
            return _gameParts;
        }

        public override void LoadContent()
        {

            _levelMap = _content.Load<Texture2D>("Background/levelMapTwo");
            _statsTable = _content.Load<Texture2D>("GameItems/statsTable");
            _tableGUI = _content.Load<Texture2D>("GameItems/tableGUI");
            _endTable = _content.Load<Texture2D>("GameItems/table");
            _failed = _content.Load<Texture2D>("GameItems/failed");
            _win = _content.Load<Texture2D>("GameItems/win");

            _backToMenuButton = _content.Load<Texture2D>("MenuButtons/closeButton");
            _pauseButton = _content.Load<Texture2D>("MenuButtons/pauseButton");
            _restartButton = _content.Load<Texture2D>("MenuButtons/restartButton");
            _backButton = _content.Load<Texture2D>("MenuButtons/backButton");
            _nextLevelButton = _content.Load<Texture2D>("MenuButtons/nextLevelButton");

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
            _enemy03_walkTile = _content.Load<Texture2D>("Enemies/enemy03_walkTile");

            _gameFont = _content.Load<SpriteFont>("MenuFont/endGameFont");
            _menuFont = _content.Load<SpriteFont>("MenuFont/menuFont");

            _mouseCursorStandard = _content.Load<Texture2D>("MenuButtons/mouse");
            _mouseCursorSubtraction = _content.Load<Texture2D>("GameItems/subtractionMouse");
            _mouseCursorAddition = _content.Load<Texture2D>("GameItems/additionMouse");
            _mouseCursorDivision = _content.Load<Texture2D>("GameItems/divisionMouse");
            _mouseCursorSquareRoot = _content.Load<Texture2D>("GameItems/squareMouse");
            _gameSong = _content.Load<Song>("GameSound/levelTwoSong");
            _mathOperationSound = _content.Load<SoundEffect>("GameSound/mathOperationSound");
            _enemyHitSound = _content.Load<SoundEffect>("GameSound/enemyHitSound");

            MediaPlayer.Play(_gameSong);
            MediaPlayer.IsRepeating = true;
            _mouseCursor = _mouseCursorStandard;
            _endTablePosition = new Vector2(Game1.ScreenWidth / 2 - _endTable.Width / 2, Game1.ScreenHeight / 2 - _endTable.Height / 2);
            _failedPosition = new Vector2(_endTablePosition.X + _failed.Width / 3, _endTablePosition.Y);

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
              _enemy02_walkTile,
              _enemy03_walkTile
            };

            Enemy.EnemyDeathHandler += HandleEnemyDeath;
            MathEnemy.MathEnemyDeathHandler += HandleMathEnemyDeath;
            MathOperationButton.mathOperationButtonIsClicked += HandleMathOperationButtonIsClicked;

            GameManager.GameManagerInstance.CurrentLevel = Level.LevelTwo;

            backToMenuButton = new MenuButton(_backToMenuButton, _gameFont)
            {
                Position = new Vector2(Game1.ScreenWidth - _backToMenuButton.Width, 0)
            };

            backToMenuButton.menuButtonEventHandler += HandleBackToMenuButtonClicked;

            backButton = new MenuButton(_backButton, _menuFont)
            {
                Position = new Vector2(Game1.ScreenWidth - _backButton.Width - 350, 30),
                Scale = 0.7f
            };

            backButton.menuButtonEventHandler += HandleBackToMenuButtonClicked;

            pauseButton = new MenuButton(_pauseButton, _gameFont)
            {
                Position = new Vector2(Game1.ScreenWidth - 300, 30),
                Scale = 0.7f
            };

            pauseButton.menuButtonEventHandler += HandlePauseButtonClicked;

            archerButton = new TowerButton(_archerTowerButton, AttackType.archer)
            {
                Position = new Vector2(50, 30),
                Scale = 0.7f
            };

            archerButton.PreviewTowerEventHandler += HandleTowerPreview;
            archerButton.PlaceTowerEventHandler += HandlePlaceTower;
            archerButton.TurnTowerPreviewOffEventHandler += HandleTurnPreviewOff;

            fireButton = new TowerButton(_fireTowerButton, AttackType.fire)
            {
                Position = new Vector2(250, 30),
                Scale = 0.7f,
                Font = _menuFont
            };

            fireButton.PreviewTowerEventHandler += HandleTowerPreview;
            fireButton.PlaceTowerEventHandler += HandlePlaceTower;
            fireButton.TurnTowerPreviewOffEventHandler += HandleTurnPreviewOff;

            subtractionButton = new MathOperationButton(_mathOperationButton, MathOperation.subtraction)
            {
                Position = new Vector2(450, 30),
                Scale = 0.7f,
                Font = _menuFont,
                MathButtonText = "-1"

            };

            divisionButton = new MathOperationButton(_mathOperationButton, MathOperation.division)
            {
                Position = new Vector2(650, 30),
                Scale = 0.7f,
                Font = _menuFont,
                MathButtonText = "/2",
                CoolDownTime = 5f,

            };

            additionButton = new MathOperationButton(_mathOperationButton, MathOperation.addition)
            {
                Position = new Vector2(850, 30),
                Scale = 0.7f,
                Font = _menuFont,
                MathButtonText = "+1"
            };

            squareRootButton = new MathOperationButton(_mathOperationButton, MathOperation.squareroot)
            {
                Position = new Vector2(1050, 30),
                Scale = 0.7f,
                Font = _menuFont,
                MathButtonText = "sqrt",
                CoolDownTime = 10f,

            };

            EnemySpawner enemySpawner = new EnemySpawner(_enemyTextureArray, _healthBar, _healthBarBackground, _menuFont);
            endgameHandler = new EndGameHandler(_gameFont, _menuFont);

            endgameHandler.endGameHandler += HandleEndGame;

            _gameParts = new List<IGameParts>()
            {
                backToMenuButton,
                backButton,
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

        public override void Update(GameTime gameTime)
        {
            if (_pauseGame || _isGameOver)
            {
                foreach (IGameParts gamePart in _gameParts.ToArray())
                {
                    if (gamePart is MenuButton)
                        gamePart.Update(gameTime, _gameParts);

                }
                return;
            }
            if(_isGameOver == false)
            {
                if (_backgroundTowers.Count > 0)
                {
                    foreach (Tower tower in _backgroundTowers)
                    {
                        tower.Update(gameTime, _gameParts, _backgroundTowers);
                    }
                }

                foreach (IGameParts gamePart in _gameParts.ToArray())
                {
                    if (gamePart is null) { continue; }
                    gamePart.Update(gameTime, _gameParts, _backgroundTowers);
                }
                CheckIfMathOperationUsed();
                CheckIfEnemyIsHit();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {

            int x, y;
            DrawUI(spritebatch, out x, out y);

            //spritebatch.DrawString(_menuFont, $"{x}:{y}", new Vector2(50, 600), Color.White);

            if (_pauseGame)
            {
                foreach (IGameParts gamePart in _gameParts)
                {
                    if (gamePart is MenuButton)
                    {
                        gamePart.Draw(gameTime, spritebatch);
                    }
                }
                spritebatch.Draw(_mouseCursor, _currentMouseRectangle, null, Color.White, -2.0f, _zeroPosition, SpriteEffects.None, 0f);
                return;
            }

            if (_isGameOver == false)
            {
                if(_backgroundTowers.Count > 0)
                {
                    foreach (Tower tower in _backgroundTowers)
                    {
                        tower.Draw(gameTime, spritebatch);
                    }
                }

                foreach (IGameParts gamePart in _gameParts)
                {
                    if(gamePart is null) { continue;}
                    gamePart.Draw(gameTime, spritebatch);
                }
            }
            else if(_gameWon || _isGameOver)
            {
               
                Texture2D header;
                header = (!_gameWon) ? _failed : _win;
                spritebatch.Draw(_endTable, _endTablePosition, Color.White);
                spritebatch.Draw(header, _failedPosition, Color.White);

                foreach (IGameParts gamePart in _gameParts)
                {
                    if (gamePart is MenuButton || gamePart is EndGameHandler)
                    {
                        gamePart.Draw(gameTime, spritebatch);
                    }
                }

            }

            spritebatch.Draw(_mouseCursor, _currentMouseRectangle, null, Color.White, -2.0f, _zeroPosition, SpriteEffects.None, 0f);
        }

        private void CheckIfEnemyIsHit()
        {
            if (GameManager.GameManagerInstance.EnemyIsHit)
            {
                _enemyHitSound.Play();
                GameManager.GameManagerInstance.EnemyIsHit = false;
            }
        }

        private void CheckIfMathOperationUsed()
        {
            if (GameManager.GameManagerInstance.MathOperationIsUsed)
            {
                _mathOperationSound.Play();
                GameManager.GameManagerInstance.MathOperationIsUsed = false;
            }
        }

        private void DrawUI(SpriteBatch spritebatch, out int x, out int y)
        {
            x = Mouse.GetState().X;
            y = Mouse.GetState().Y;
            _currentMouseRectangle.X = x;
            _currentMouseRectangle.Y = y;
            _currentMouseRectangle.Width = _mouseCursor.Width;
            _currentMouseRectangle.Height = _mouseCursor.Height;

            spritebatch.Draw(_levelMap, _zeroPosition, Color.White);
            spritebatch.Draw(_tableGUI, _tableGUIRectangle, Color.White);
            spritebatch.Draw(
                _statsTable,
                _statsTablePosition,
                null, Color.White,
                0f,
                _zeroPosition,
                0.7f,
                SpriteEffects.None, 0f);
        }

        private void HandleMathOperationButtonIsClicked(MathOperation mathOperation, bool clicked)
        {
            _mathOperation = mathOperation;
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

        }

        private void HandleMathEnemyDeath(MathEnemy enemy)
        {
            _gameParts.Remove(enemy);

        }

        private void HandleEndGame(bool won)
        {
            if(!won)
            {
                _isGameOver = true;
            }
            else
            {
                _gameWon = true;
                _isGameOver = true;
            }

            MenuButton restartButton = new MenuButton(_restartButton, _menuFont)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - _restartButton.Width / 2, Game1.ScreenHeight / 2 - _restartButton.Height / 2)
            };

            restartButton.menuButtonEventHandler += HandleRestartButtonClicked;

            _gameParts.Add(restartButton);
            
        }

        private void HandleRestartButtonClicked(bool clicked)
        {
            Unsubscribe();
            UnsubscribeListElements();
            //_gameParts.Clear();
            GameManager.GameManagerInstance.StoppedEnemies = 0;
            _game1.ChangeState(new GameStateTwo(_game1, _graphics, _content, GameManager.GameManagerInstance.Difficulty));
        }

        private void UnsubscribeListElements()
        {
            foreach (IGameParts gamePart in _gameParts)
            {
                if(gamePart is IUnsubscribable iUnsubsribable)
                {
                    iUnsubsribable.Unsubscribe();
                }
            }
        }

        private void HandleBackToMenuButtonClicked(bool clicked)
        {
            Unsubscribe();
            //_gameParts.Clear();
            endgameHandler.Unsubscribe();
            GameManager.GameManagerInstance.StoppedEnemies = 0;
            _game1.ChangeState(new MenuState(_game1, _graphics, _content, GameManager.GameManagerInstance.Difficulty));
           
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
                towerStartRange = _towerStartRangeArcher;
                towerMaxRange = _towerMaxRangeArcher;

             }
            else if(towerType == AttackType.fire)
            {
                towerStartRange = _towerStartRangeFire;
                towerMaxRange = _towerMaxRangeFire;
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

                if(tower.CheckIfTowerIsBackgroundTower())
                {
                    _gameParts.Remove(tower);
                    _backgroundTowers.Add(tower);
                }

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
                if(_gameParts.Contains(removeTower))
                {
                    _gameParts.Remove(removeTower);
                }
                else if (_backgroundTowers.Contains(removeTower))
                {
                    _backgroundTowers.Remove(removeTower);
                }
            }

        }

        public List<Tower> GetBackgroundTowers()
        {
            return _backgroundTowers;
        }

        public void Unsubscribe()
        {
            Enemy.EnemyDeathHandler -= HandleEnemyDeath;
            MathEnemy.MathEnemyDeathHandler -= HandleMathEnemyDeath;
            MathOperationButton.mathOperationButtonIsClicked -= HandleMathOperationButtonIsClicked;
            endgameHandler.endGameHandler -= HandleEndGame;
            backToMenuButton.menuButtonEventHandler -= HandleBackToMenuButtonClicked;
            backButton.menuButtonEventHandler -= HandleBackToMenuButtonClicked;
            pauseButton.menuButtonEventHandler -= HandlePauseButtonClicked;
            archerButton.PreviewTowerEventHandler -= HandleTowerPreview;
            archerButton.PlaceTowerEventHandler -= HandlePlaceTower;
            archerButton.TurnTowerPreviewOffEventHandler -= HandleTurnPreviewOff;
            fireButton.PreviewTowerEventHandler -= HandleTowerPreview;
            fireButton.PlaceTowerEventHandler -= HandlePlaceTower;
            fireButton.TurnTowerPreviewOffEventHandler -= HandleTurnPreviewOff;

        }
    }
}
