//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Buttons;
using Tower_Defence.Enums;
using Tower_Defence.Sprites;

namespace Tower_Defence.States
{
    public class ManualState : State
    {
        #region Fields
        private Texture2D _menuBackground;
        private Texture2D _closeGameButton;
        private Texture2D _playButton;
        private Texture2D _backButton;
        private Texture2D _ropeSmall;
        private Texture2D _table;
        private Texture2D _standardTower;
        private Texture2D _fireTower;
        private Texture2D _mathButton;
        private Texture2D _enemyTexture;
        private Texture2D _mathEnemyTexture;

        private Texture2D _healthBar;
        private Texture2D _healthBarBackground;

        private string _standardTowerText = "High range Tower with a minimum range.\nCan't hit enemies too close to the tower.\nUse this tower against standard enemies.";
        private string _fireTowerText = "Low range tower without a minimum range.\nCan hit enemies very close to the tower.\nUse this tower against standard enemies.";
        private string _mathOperation = "Mathematical operations:\n +1, -1, /2, and square root.\nUse these operations to\n attack math enemies.";
        private string _enemyText = "Standard enemy.\nAttack it with either the high\n or low range tower.";
        private string _explanation = "Left click: Select/place tower or math operation.\nRight click: Deselect tower or math operation.\nYou can only have 2 towers at the same time\nChoose your tower and building spot wisely";
        private string _mathEnemyText = "Math enemy.\nTowers are useless here.\nUse mathematical operations to attack this foe.";

        private Song _titleSong;
        private SoundEffect _buttonSound;

        private Vector2 zeroPosition = new Vector2(0, 0);
        private Vector2 _ropeSmallPosition_1 = new Vector2(1350, -70);
        private Vector2 _ropeSmallPosition_2 = new Vector2(1400, -70);
        private Vector2 _tablePosition = new Vector2(100, 50);
        private Vector2 _tablePosition1 = new Vector2(100, 350);
        private Vector2 _tablePosition2 = new Vector2(100, 650);
        private Vector2 _tablePosition3 = new Vector2(700, 50);
        private Vector2 _tablePosition4 = new Vector2(700, 350);
        private Vector2 _tablePosition5 = new Vector2(700, 650);

        private Vector2 _standardTowerPosition = new Vector2(120, 90);
        private Vector2 _fireTowerPosition = new Vector2(120, 390);
        private Vector2 _mathButtonPosition = new Vector2(710, 90);
        private Vector2 _standardTextPosition = new Vector2(270, 90);
        private Vector2 _fireTextPosition = new Vector2(270, 390);
        private Vector2 _enemyTexPosition = new Vector2(270, 700);
        private Vector2 _mathButtonTextPosition = new Vector2(900, 90);
        private Vector2 _explanationTextPosition = new Vector2(740, 400);
        private Vector2 _mathEnemyTextPosition = new Vector2(850, 700);
        private Vector2 _difficultyTextPosition = new Vector2(Game1.ScreenWidth - 300, Game1.ScreenHeight - 90);



        private Rectangle _currentMouseRectangle = new Rectangle();
        private Difficulty _difficulty;

        private Texture2D _mouseCursor;

        private SpriteFont _menuFont;
        private SpriteFont _explanationFont;

        private List<IGameParts> _gameParts;

        #endregion
        public ManualState(Game1 game1, GraphicsDeviceManager graphics, ContentManager content, Difficulty difficulty) : base(game1, graphics, content)
        {
            _difficulty = difficulty;
        }

        #region Methods
        public override void LoadContent()
        {
            _game1.IsMouseVisible = false;

            _menuBackground = _content.Load<Texture2D>("Background/menuBackground");
            _playButton = _content.Load<Texture2D>("MenuButtons/playButton");
            _backButton = _content.Load<Texture2D>("MenuButtons/backButton");
            _closeGameButton = _content.Load<Texture2D>("MenuButtons/closeButton");
            _ropeSmall = _content.Load<Texture2D>("MenuItems/ropeSmall");
            _menuFont = _content.Load<SpriteFont>("MenuFont/menuFont");
            _explanationFont = _content.Load<SpriteFont>("MenuFont/explanationFont");
            _table = _content.Load<Texture2D>("GameItems/statsTable");
            _standardTower = _content.Load<Texture2D>("Tower/archerTower");
            _fireTower = _content.Load<Texture2D>("Tower/fireTower");
            _mathButton = _content.Load<Texture2D>("TowerButtons/mathButton");
            _enemyTexture = _content.Load<Texture2D>("Enemies/enemy01_walkTile");
            _mathEnemyTexture = _content.Load<Texture2D>("Enemies/enemy02_walkTile");

            _healthBar = _content.Load<Texture2D>("GameItems/healthBar");
            _healthBarBackground = _content.Load<Texture2D>("GameItems/healthBarBackground");


            _mouseCursor = _content.Load<Texture2D>("MenuButtons/mouse");

            _buttonSound = _content.Load<SoundEffect>("MenuSound/buttonSound");

            _titleSong = _content.Load<Song>("MenuSound/titleSong");

            MenuButton playButton = new MenuButton(_playButton, _menuFont)
            {
                Position = new Vector2(Game1.ScreenWidth - 610, 200),
                Scale = 0.5f
            };
            playButton.menuButtonEventHandler += HandlePlayButtonClicked;

            MenuButton backButton = new MenuButton(_backButton, _menuFont)
            {
                Position = new Vector2(Game1.ScreenWidth - 350, 10),
                Scale = 0.7f
            };

            backButton.menuButtonEventHandler += HandleBackButtonClicked;


            MenuButton closeGameButton = new MenuButton(_closeGameButton, _menuFont)
            {
                Position = new Vector2(Game1.ScreenWidth - _closeGameButton.Width, 0)
            };

            closeGameButton.menuButtonEventHandler += HandleCloseButtonClicked;

            Enemy enemy = new Enemy(_enemyTexture, 5, 4, _healthBar, _healthBarBackground)
            {
                Position = new Vector2(80, 600),
                idle = true
            };

            MathEnemy mathEnemy = new MathEnemy(_mathEnemyTexture, 2, 5, _healthBar, _healthBarBackground, _menuFont)
            {
                Position = new Vector2(680, 600),
                idle = true
                
            };


            _gameParts = new List<IGameParts>()
            {
                playButton,
                backButton,
                closeGameButton,
                enemy,
                mathEnemy
            };
        }

        private void HandleBackButtonClicked(bool clicked)
        {
            _game1.ChangeState(new MenuState(_game1, _graphics, _content, _difficulty));
        }

        public override void Update(GameTime gameTime)
        {       
            foreach (IGameParts gamePart in _gameParts.ToArray())
            {
                gamePart.Update(gameTime, _gameParts);
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuBackground, zeroPosition, Color.White);
            
            spriteBatch.Draw(_table, _tablePosition,null, Color.White);
            spriteBatch.Draw(_table, _tablePosition1, null, Color.White);
            spriteBatch.Draw(_table, _tablePosition2, null, Color.White);
            spriteBatch.Draw(_table, _tablePosition3, null, Color.White);
            spriteBatch.Draw(_table, _tablePosition4, null, Color.White);
            spriteBatch.Draw(_table, _tablePosition5, null, Color.White);

            spriteBatch.Draw(_standardTower, _standardTowerPosition, null, Color.White);
            spriteBatch.Draw(_fireTower, _fireTowerPosition, null, Color.White);
            spriteBatch.Draw(_mathButton, _mathButtonPosition, null, Color.White);

            spriteBatch.DrawString(_explanationFont, _standardTowerText, _standardTextPosition, Color.White);
            spriteBatch.DrawString(_explanationFont, _fireTowerText, _fireTextPosition, Color.White);
            spriteBatch.DrawString(_explanationFont, _enemyText, _enemyTexPosition, Color.White);
            spriteBatch.DrawString(_explanationFont, _mathOperation, _mathButtonTextPosition, Color.White);
            spriteBatch.DrawString(_explanationFont, _explanation, _explanationTextPosition, Color.White);
            spriteBatch.DrawString(_explanationFont, _mathEnemyText, _mathEnemyTextPosition, Color.White);

            foreach (IGameParts gamePart in _gameParts)
            {
                gamePart.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Draw(_ropeSmall, _ropeSmallPosition_1, Color.White);
            spriteBatch.Draw(_ropeSmall, _ropeSmallPosition_2, Color.White);

            string text = $"Difficulty: {_difficulty}";
            spriteBatch.DrawString(_menuFont, text, _difficultyTextPosition, Color.White);

            MouseState currentMouse = Mouse.GetState();
            _currentMouseRectangle.X = currentMouse.X;
            _currentMouseRectangle.Y = currentMouse.Y;
            _currentMouseRectangle.Width = _mouseCursor.Width;
            _currentMouseRectangle.Height = _mouseCursor.Height;

            spriteBatch.Draw(_mouseCursor, _currentMouseRectangle, null, Color.White, -2.0f, zeroPosition, SpriteEffects.None, 0f);
        }
        private void HandlePlayButtonClicked(bool clicked)
        {
            MediaPlayer.Stop();
            _game1.ChangeState(new GameStateOne(_game1, _graphics, _content, _difficulty));
        }

        private void HandleCloseButtonClicked(bool clicked)
        {
            _game1.QuitGame();
        }

        #endregion

    }
}
