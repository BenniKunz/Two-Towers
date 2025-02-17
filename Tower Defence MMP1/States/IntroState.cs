﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tower_Defence.Enums;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Tower_Defence.States
{
    public class IntroState : State
    {
        private float _timer;
        private float _titleTimer;
        private SpriteFont _menuFont;
        private Texture2D _logo;
        private SoundEffect _soundEffect;
        private SoundEffect _bowSound;
        private string _title = "Two Towers";
        private string _tempTitle = "";
        private int counter = 0;
        private char[] _titleArray;

        public IntroState(Game1 game1, GraphicsDeviceManager graphics, ContentManager content) : base(game1, graphics, content)
        {

        }
        public override void LoadContent()
        {
            _logo = _content.Load<Texture2D>("IntroItems/logo");
            _menuFont = _content.Load<SpriteFont>("IntroItems/IntroFont");
            _soundEffect = _content.Load<SoundEffect>("IntroItems/introSound");
            _bowSound = _content.Load<SoundEffect>("IntroItems/bowSound");
            _titleArray = _title.ToCharArray();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(_logo, new Vector2(Game1.ScreenWidth / 2 - _logo.Width / 2, Game1.ScreenHeight / 2 - _logo.Height / 2), Color.White);

            spritebatch.DrawString(_menuFont, _tempTitle, new Vector2(Game1.ScreenWidth / 2  - _menuFont.MeasureString(_title).X / 2,
                Game1.ScreenHeight - 300), Color.White);
        }


        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _titleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            if(_titleTimer >= 0.5f && counter < _titleArray.Length)
            {
                
                _tempTitle += _titleArray[counter].ToString();
                
                if (_titleArray[counter].ToString() != " ")
                {

                    _soundEffect.Play();
                }
                counter++;
                _titleTimer = 0f;
            }
            
            if(_timer >= 5.5f && counter == _titleArray.Length)
            {
                _bowSound.Play();
                counter++;
            }

            if(_timer >= 6.0f || state.IsKeyDown(Keys.Escape))
            {
                _game1.ChangeState(new MenuState(_game1, _graphics, _content, Difficulty.easy));
            }
        }
    }
}