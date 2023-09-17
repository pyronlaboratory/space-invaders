using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.Text;
using static SpaceInvaders.Content.Alien;
using System.Linq;

namespace SpaceInvaders.Content
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceInvaders : Game
    {
        GraphicsDeviceManager graphics;
        
        int screenWidth;
        int screenHeight;

        // Game World
        // These variables define the world 
        Player          _player;
        Alien           _alien;
        UFO             _ufo;
        Sprite          _universe;
        Sprite          _gameTitle;

        List<Sprite>    _gameSprites        = new List<Sprite>();
        List<Alien>     _mobAliens          = new List<Alien>();
        List<Missile>   _missilesLaunched   = new List<Missile>();

        SoundEffect      _explosionSound;
        SoundEffect      _inGameSound;

        string           _message           = "Space Invaders";

        float            _deltaTime         = 1.0f / 60.0f;

        // Add a new field to track UFO appearance
        float           _xSpeed             = 100;
        float           _ySpeed             = 500;
        int             _score;
        int             _timer;
        bool            _ufoVisible         = false;
        bool            _spritesLoaded      = false;
        
        public enum GameState { START, PLAYING, PAUSE, END };
        GameState _currentGameState;

        public enum AlienMovementState { MoveRight, MoveDownRight, MoveLeft, MoveDownLeft };
        private AlienMovementState _direction = AlienMovementState.MoveRight;

        SpriteBatch     _spriteBatch;
        SpriteFont      _messageFont;

        public SpaceInvaders()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }


        /// <summary>
        /// LoadSprite will be called if the load flag is turned on
        /// </summary>
        private void LoadSprites()
        {
            
            // Create player ship
            int shipWidth = screenWidth / 15;
            int shipHeight = screenHeight - (shipWidth * 2);
            Texture2D shipTexture = Content.Load<Texture2D>("Ship2");
            _player = new Player(screenWidth, screenHeight, shipTexture, shipWidth, shipHeight, screenWidth / 2, screenHeight / 2);

            Texture2D missileTexture = Content.Load<Texture2D>("Missile");
            _player.InitData(this, missileTexture);

            _gameSprites.Add(_player);

            // Create mob aliens
            // Define the number of rows and columns
            int numRows = 3; // 3 rows
            int numCols = 10; // 10 aliens in each row

            // Define the width and height of each alien
            int alienWidth = screenWidth / 20; // Adjust the width to make them closer
            int alienHeight = screenHeight / (4 * numRows); // Adjusted alienHeight

            // Define the vertical spacing between rows
            int verticalSpacing = 1; // Adjust the vertical spacing between rows (reduce this value)

            // Calculate the initial Y position for the first row
            int initialY = screenHeight / 2 - (numRows * (alienHeight + verticalSpacing));

            // Define the spacing between aliens within each row
            int colSpacing = 10; // Adjust the horizontal spacing

            // Loop through each row
            for (int row = 0; row < numRows; row++)
            {
                // Define the texture name for this row (A1, A2, A3)
                string textureName = "Alien" + (row + 1);

                // Load the texture for this row
                Texture2D alienTexture = Content.Load<Texture2D>(textureName);

                // Loop through each column in the current row
                for (int col = 0; col < numCols; col++)
                {
                    // Calculate the X position based on the column, adding horizontal spacing
                    int alienX = col * (alienWidth + colSpacing);

                    // Calculate the Y position based on the row, accounting for vertical spacing
                    int alienY = initialY + row * (alienHeight + verticalSpacing);

                    // Create the alien object and add it to the list
                    _alien = new Alien(screenWidth, screenHeight, alienTexture, alienWidth, alienX, alienY);
                    _mobAliens.Add(_alien);
                    _gameSprites.Add(_alien);
                }
            }

            // Create enemy ufo
            int ufoWidth = screenWidth / 8;
            int ufoHeight = screenHeight - (shipWidth * 2);
            Texture2D ufoTexture = Content.Load<Texture2D>("UFO");
            _ufo = new UFO(screenWidth, screenHeight, ufoTexture, ufoWidth, ufoHeight, 20, 5);
            _gameSprites.Add(_ufo);

            _spritesLoaded = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _messageFont = Content.Load<SpriteFont>("MessageFont");

            // Create Background
            Texture2D universe = Content.Load<Texture2D>("Space");
            _universe = new Sprite(screenWidth, screenHeight, universe, screenWidth, 0, 0);
            _gameSprites.Add(_universe);

            Texture2D titleTexture = Content.Load<Texture2D>("Main");
            _gameTitle = new Sprite(screenWidth, screenHeight, titleTexture, screenWidth / 2, screenWidth / 4, 50);

            // Load sprites separately
            LoadSprites();

            // Sound effects
            _inGameSound = Content.Load<SoundEffect>("Background");
            _explosionSound = Content.Load<SoundEffect>("Explosion");

            // go to the start screen state
            _message = "Press Space to Start";
            _currentGameState = GameState.START;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (_currentGameState)
            {
                case GameState.START:
                    UpdateMenu();
                    break;

                case GameState.PLAYING:
                    UpdateGame();
                    break;

                case GameState.END:
                    UpdateMenu();
                    break;

                default:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (_currentGameState)
            {
                case GameState.START:
                    DrawMenu();
                    break;

                case GameState.PLAYING:
                    DrawGame();
                    break;

                case GameState.END:
                    DrawMenu();
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }


        /// <summary>
        /// This is game start logic.
        /// </summary>
        private void StartGame()
        {
            if (_spritesLoaded == false)
            {
                LoadSprites();
            }

            foreach (Sprite s in _gameSprites)
            {
                s.Reset();
            }


            _timer = 6000;
            _score = 0;
            _message = "";
            _currentGameState = GameState.PLAYING;
            _direction = AlienMovementState.MoveRight;
            _inGameSound.Play();
        }

        /// <summary>
        /// This is game over logic.
        /// </summary>
        private void GameOver()
        {
            _currentGameState = GameState.END;
            _message = "Game Over\n\n\nScore: " + _score.ToString() + "\nPress Space to Restart";

            _mobAliens.Clear();
            _missilesLaunched.Clear();

            // Remove all sprites
            _gameSprites.RemoveRange(1, _gameSprites.Count - 1);
            _spritesLoaded = false;
        }

        private void UpdateMenu()
        {
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Space))
            {
                StartGame();
            }
        }

        private void UpdateGame()
        {
            // Increase alien movement speed every 10 seconds
            if (_timer % (10 * 60) == 0) // Increase speed every 10 seconds (assuming 60 FPS)
            {
                _xSpeed += 50.0f; // Increase the speed by 10 units per second
            }

            foreach (var alien in _mobAliens)
            {
                Vector2 newPosition = alien.Position;
                switch (_direction)
                {
                    case AlienMovementState.MoveRight:
                        newPosition.X += _xSpeed * _deltaTime;
                        break;

                    case AlienMovementState.MoveDownRight:
                        newPosition.X += _xSpeed * _deltaTime;
                        newPosition.Y += _ySpeed * _deltaTime;
                        break;

                    case AlienMovementState.MoveLeft:
                        newPosition.X -= _xSpeed * _deltaTime;
                        break;

                    case AlienMovementState.MoveDownLeft:
                        newPosition.X -= _xSpeed * _deltaTime;
                        newPosition.Y += _ySpeed * _deltaTime;
                        break;
                }
                alien.Position = newPosition;
            }

            if (_mobAliens.Count > 0)
            {
                // Check if the aliens need to change direction and move down
                float rightmostPosition = _mobAliens.Max(alien => alien.Position.X + (screenWidth / 20));
                float leftmostPosition = _mobAliens.Min(alien => alien.Position.X);
                if (rightmostPosition >= screenWidth || leftmostPosition <= 0)
            {
                foreach (var alien in _mobAliens)
                {
                    alien.Position = new Vector2(alien.Position.X, alien.Position.Y + _ySpeed * _deltaTime);
                }

                // Change direction
                if (_direction == AlienMovementState.MoveRight || 
                        _direction == AlienMovementState.MoveDownRight)
                {
                    _direction = AlienMovementState.MoveLeft;
                }
                else
                {
                    _direction = AlienMovementState.MoveRight;
                }
            }
            }

            for (int i = 0; i < _gameSprites.Count; i++)
            {
                _gameSprites[i].Update(_deltaTime);
            }

            for (int i = 0; i < _mobAliens.Count; i++)
            {
                for (int j = 0; j < _missilesLaunched.Count; j++)
                {
                    if (_mobAliens[i] != null)
                    {
                        if (_missilesLaunched[j].IntersectsWith(_mobAliens[i]))
                        {
                            _explosionSound.Play(volume: 0.1f, pitch: 0.0f, pan: 0.0f);

                            _gameSprites.Remove(_missilesLaunched[j]);
                            _missilesLaunched.RemoveAt(j);

                            _gameSprites.Remove(_mobAliens[i]);
                            _mobAliens.RemoveAt(i);

                            _score += 10;
                        }
                    }
                }
            }

            _timer--;
            int secsLeft = _timer / 60;
            _message = "Time: " + secsLeft.ToString() + " Score: " + _score;

            if (_timer == 0)
            {
                GameOver();
            }

            // Check if the aliens have reached the player's ship
            int playerBase = screenHeight - 100;

            if (_mobAliens.Any(alien => alien.Position.Y + screenHeight / (4 * 3) >= playerBase))
            {
                GameOver();
            }

        }

        private void DrawMenu()
        {
            _spriteBatch.Begin();

            _gameSprites[0].Draw(_spriteBatch);
            
            _gameTitle.Draw(_spriteBatch);   

            // Display instructions
            float xPos = (screenWidth - _messageFont.MeasureString(_message).X) / 2;
            Vector2 messagePos = new Vector2(5, 5);
            _spriteBatch.DrawString(_messageFont, _message, messagePos, Color.White);
            _spriteBatch.End();
        }
        
        private void DrawGame()
        {
            _spriteBatch.Begin();

            if (_currentGameState != GameState.END)
            {
                foreach (Sprite s in _gameSprites)
                {
                    s.Draw(_spriteBatch);
                }

                // Display player health
                _player.DrawHealthBar(_spriteBatch, _messageFont);

                // Display _timer and _score
                Vector2 messagePos = new Vector2(5, 5);
                _spriteBatch.DrawString(_messageFont, _message, messagePos, Color.White);
            }
            _spriteBatch.End();
        }

        // TO DO: Move to player class
        public void LaunchMissile(Missile missile)
        {
            _missilesLaunched.Add(missile);
            _gameSprites.Add(missile);
        }

    }
}
