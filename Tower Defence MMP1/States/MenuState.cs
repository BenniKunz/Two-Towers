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
using System.Collections.Generic;
using Tower_Defence.Buttons;
using Tower_Defence.Enums;
using Tower_Defence.Interfaces;

namespace Tower_Defence.States
{
    public class MenuState : State, IUnsubscribable
    {
        #region Fields
        private Texture2D _menuBackground;
        private Texture2D _playButton;
        private Texture2D _settingsButton;
        private Texture2D _closeGameButton;
        private Texture2D _musicButton;
        private Texture2D _musicOffButton;
        private Texture2D _easyButton;
        private Texture2D _normalButton;
        private Texture2D _hardButton;
        private Texture2D _manualButton;
        private Texture2D _impressumButton;
        private Texture2D _ropeSmall;
        private Texture2D _descriptionBubble;
        private Texture2D _title;
        private Texture2D _impressumTable;
        private Texture2D _fullScreenOn;
        private Texture2D _fullScreenOff;
        private string _manualText = "Read the manual\nbefore your first\nbattle.";
        private string _impressumText = "MultiMediaTechnology\nFHS 45891\nMultiMediaProjekt 1\n\nBenjamin Kunz\n\nAssets:\nhttps://craftpix.net";
        private Difficulty _difficulty;
        private Song _titleSong;
        private SoundEffect _buttonSound;
        private bool _impressumOn;

        private Vector2 zeroPosition = new Vector2(0, 0);
        private Vector2 _ropeSmallPosition_1 = new Vector2(1400, -70);
        private Vector2 _ropeSmallPosition_2 = new Vector2(1450, -70);
        private Vector2 _ropeSmallPosition_3 = new Vector2(1600, -70);
        private Vector2 _ropeSmallPosition_4 = new Vector2(1650, -70);
        private Vector2 _descriptionBubblePosition = new Vector2(250, 500);
        private Vector2 _descriptionBubbleTextPosition = new Vector2(330, 550);
        private Vector2 _difficultyTextPosition = new Vector2(Game1.ScreenWidth - 300, Game1.ScreenHeight - 90);
        private Vector2 _impressumTextPosition;
        private Rectangle _currentMouseRectangle = new Rectangle();

        private Texture2D _mouseCursor;

        private SpriteFont _menuFont;

        private List<IGameParts> _gameParts;

        #region Buttons
        MenuButton playButton;
        MenuButton closeGameButton;
        MenuButton settingsButton;
        MenuButton musicButton;
        MenuButton manualButton;
        MenuButton impressumButton;
        MenuButton fullScreenButton;
        #endregion

        #endregion
        public MenuState(Game1 game1, GraphicsDeviceManager graphics, ContentManager content, Difficulty difficulty) : base(game1, graphics, content)
        {
            _difficulty = difficulty;
        }

        #region Methods
        public override void LoadContent()
        {
            _game1.IsMouseVisible = false;

            _menuBackground = _content.Load<Texture2D>("Background/menuBackground");
            _playButton = _content.Load<Texture2D>("MenuButtons/playButton");
            _closeGameButton = _content.Load<Texture2D>("MenuButtons/closeButton");
            _settingsButton = _content.Load<Texture2D>("MenuButtons/settingsButton");
            _ropeSmall = _content.Load<Texture2D>("MenuItems/ropeSmall");
            _menuFont = _content.Load<SpriteFont>("MenuFont/menuFont");
            _musicButton = _content.Load<Texture2D>("MenuButtons/musicButton");
            _musicOffButton = _content.Load<Texture2D>("MenuButtons/musicOffButton");
            _easyButton = _content.Load<Texture2D>("MenuButtons/easyButton");
            _normalButton = _content.Load<Texture2D>("MenuButtons/normalButton");
            _hardButton = _content.Load<Texture2D>("MenuButtons/hardButton");
            _manualButton = _content.Load<Texture2D>("MenuButtons/manualButton");
            _impressumButton = _content.Load<Texture2D>("MenuButtons/impressumButton");
            _mouseCursor = _content.Load<Texture2D>("MenuButtons/mouse");
            _descriptionBubble = _content.Load<Texture2D>("MenuItems/descriptionBubble");
            _title = _content.Load<Texture2D>("MenuItems/title");
            _impressumTable = _content.Load<Texture2D>("GameItems/table");
            _fullScreenOn = _content.Load<Texture2D>("MenuItems/fullScreenOn");
            _fullScreenOff = _content.Load<Texture2D>("MenuItems/fullScreenOff");

            _buttonSound = _content.Load<SoundEffect>("MenuSound/buttonSound");

            _titleSong = _content.Load<Song>("MenuSound/titleSong");

            MediaPlayer.Play(_titleSong);
            MediaPlayer.IsRepeating = true;

            _impressumTextPosition = new Vector2(Game1.ScreenWidth / 2 - _menuFont.MeasureString(_impressumText).X / 2, 350);


            playButton = new MenuButton(_playButton, _menuFont)
            {
                Position = new Vector2(
                    Game1.ScreenWidth / 2 - _playButton.Width / 2,
                    Game1.ScreenHeight / 2 - _playButton.Height / 2)
            };

            playButton.menuButtonEventHandler += HandlePlayButtonClicked;

            closeGameButton = new MenuButton(_closeGameButton, _menuFont)
            {
                Position = new Vector2(Game1.ScreenWidth - _closeGameButton.Width, 0)
            };

            closeGameButton.menuButtonEventHandler += HandleCloseButtonClicked;

            settingsButton = new MenuButton(_settingsButton, _menuFont)
            {
                Position = new Vector2(1350, 210)

            };

            settingsButton.menuButtonEventHandler += HandleSettingsButtonClicked;

            musicButton = new MenuButton(_musicButton, _menuFont)
            {
                Position = new Vector2(10, 50),
                IsMusicButton = true,
                MusicIsOn = true

            };

            musicButton.musicButtonEventHandler += HandleMusicButtonClicked;

            manualButton = new MenuButton(_manualButton, _menuFont)
            {
                Position = new Vector2(150, 800),

            };

            manualButton.menuButtonEventHandler += HandleManualButtonClicked;

            impressumButton = new MenuButton(_impressumButton, _menuFont)
            {
                Position = new Vector2(1550, 210),
            };
            impressumButton.menuButtonEventHandler += HandleImpressumButtonClicked;

            fullScreenButton = new MenuButton(_fullScreenOn, _menuFont)
            {
                Position = _difficultyTextPosition -new Vector2(300,50)
            };

            fullScreenButton.menuButtonEventHandler += HandleFullScreenButtonClicked;

            _gameParts = new List<IGameParts>()
            {
                playButton,
                closeGameButton,
                settingsButton,
                musicButton,
                manualButton,
                impressumButton,
                fullScreenButton
    };
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
            spriteBatch.Draw(_title, new Vector2(180, 30), null, Color.White, 0f, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0f);

            foreach (IGameParts gamePart in _gameParts)
            {
                gamePart.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Draw(_ropeSmall, _ropeSmallPosition_1, Color.White);
            spriteBatch.Draw(_ropeSmall, _ropeSmallPosition_2, Color.White);
            spriteBatch.Draw(_ropeSmall, _ropeSmallPosition_3, Color.White);
            spriteBatch.Draw(_ropeSmall, _ropeSmallPosition_4, Color.White);
            spriteBatch.Draw(_descriptionBubble, _descriptionBubblePosition, null, Color.White, 0f, zeroPosition, 0.7f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_menuFont, _manualText, _descriptionBubbleTextPosition, Color.Black);
            string text = $"Difficulty: {_difficulty}";
            spriteBatch.DrawString(_menuFont, text, _difficultyTextPosition, Color.White);

            MouseState currentMouse = Mouse.GetState();
            _currentMouseRectangle.X = currentMouse.X;
            _currentMouseRectangle.Y = currentMouse.Y;
            _currentMouseRectangle.Width = _mouseCursor.Width;
            _currentMouseRectangle.Height = _mouseCursor.Height; 

            if (_impressumOn)
            {
                spriteBatch.Draw(_impressumTable, new Vector2(Game1.ScreenWidth / 2 - _impressumTable.Width / 2, 200), Color.White);
                spriteBatch.DrawString(_menuFont, _impressumText, _impressumTextPosition, Color.White);
            }

            spriteBatch.DrawString(_menuFont, "Fullscreen", _difficultyTextPosition - new Vector2(300, 0), Color.White);
            spriteBatch.Draw(_mouseCursor, new Vector2(_currentMouseRectangle.X, _currentMouseRectangle.Y + _mouseCursor.Height),null, Color.White, -2.0f, zeroPosition, 1.0f, SpriteEffects.None, 0f);
        }
        private void HandleFullScreenButtonClicked(bool obj)
        {
            if(_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferWidth = 1910;  // set this value to the desired width of your window
                _graphics.PreferredBackBufferHeight = 1070;   // set this value to the desired height of your window
                fullScreenButton.Texture = _fullScreenOff;
            }
            else
            {
                fullScreenButton.Texture = _fullScreenOn;
                _graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
                _graphics.PreferredBackBufferHeight = 1080;   // set this value to the desired height of your window
            }
            _graphics.ToggleFullScreen();
            
        }
        private void HandleImpressumButtonClicked(bool clicked)
        {
            _impressumOn = !_impressumOn;
        }
        private void HandleManualButtonClicked(bool clicked)
        {
            Unsubscribe();
            _game1.ChangeState(new ManualState(_game1, _graphics, _content, _difficulty));
        }
        private void HandlePlayButtonClicked(bool clicked)
        {
            MediaPlayer.Stop();
            Unsubscribe();
            _game1.ChangeState(new GameStateOne(_game1, _graphics, _content, _difficulty));
        }

        private void HandleCloseButtonClicked(bool clicked)
        {
            Unsubscribe();
            _game1.QuitGame();
        }

        private void HandleMusicButtonClicked(MenuButton menubutton, bool isOn)
        {
            _buttonSound.Play();
            if (isOn)
            {
                MediaPlayer.IsMuted = true;
                menubutton.Texture = _musicOffButton;
                menubutton.MusicIsOn = false;
                return;
            }
            MediaPlayer.IsMuted = false;
            menubutton.Texture = _musicButton;
            menubutton.MusicIsOn = true;
        }

        private void HandleSettingsButtonClicked(bool clicked)
        {
            _buttonSound.Play();
            MenuButton easyButton = null;
            MenuButton normalButton = null;
            MenuButton hardButton = null;
            if (clicked)
            {
                easyButton = new MenuButton(_easyButton, _menuFont)
                {
                    Position = new Vector2(1500, 400),
                    Difficulty = Difficulty.easy

                };
                easyButton.difficultyButtonEventHandler += HandleDifficultyButtonClicked;

                normalButton = new MenuButton(_normalButton, _menuFont)
                {
                    Position = new Vector2(1500, 600),
                    Difficulty = Difficulty.normal
                };
                normalButton.difficultyButtonEventHandler += HandleDifficultyButtonClicked;

                hardButton = new MenuButton(_hardButton, _menuFont)
                {
                    Position = new Vector2(1500, 800),
                    Difficulty = Difficulty.hard
                };
                hardButton.difficultyButtonEventHandler += HandleDifficultyButtonClicked;

                _gameParts.Add(easyButton);
                _gameParts.Add(normalButton);
                _gameParts.Add(hardButton);
                return;
            }
            foreach (IGameParts gamePart in _gameParts.ToArray())
            {
                if (gamePart is MenuButton)
                {
                    if (((MenuButton)gamePart).Difficulty == 0) { continue; }
                    _gameParts.Remove(gamePart);
                }
            }

        }

        private void HandleDifficultyButtonClicked(Difficulty difficulty)
        {
            _buttonSound.Play();
            this._difficulty = difficulty;

        }

        public void Unsubscribe()
        {
            playButton.menuButtonEventHandler -= HandlePlayButtonClicked;
            closeGameButton.menuButtonEventHandler -= HandleCloseButtonClicked;
            settingsButton.menuButtonEventHandler -= HandleSettingsButtonClicked;
            musicButton.musicButtonEventHandler -= HandleMusicButtonClicked;
            manualButton.menuButtonEventHandler -= HandleManualButtonClicked;
        }

        #endregion

    }
}
