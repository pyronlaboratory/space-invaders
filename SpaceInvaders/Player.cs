using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Content
{
    public class Player : Sprite
    {

        SpaceInvaders game;
        
        // Health
        public Health health;

        // Movement
        protected bool MovingLeft;
        protected bool MovingRight;

        protected float resetXSpeed;
        protected float resetYSpeed;

        protected float xSpeed;
        protected float ySpeed;

        // Weapons
        Missile missile;
        Texture2D missileTexture;
        bool isShooting;

        public Player(int inScreenWidth, int inScreenHeight, Texture2D inSpriteTexture, int inDrawWidth, int inDrawHeight, float inResetX, float inResetXSpeed) :
            base(inScreenWidth, inScreenHeight, inSpriteTexture, inDrawWidth, inResetX, inDrawHeight)
        {
            health = new Health(100);
            resetXSpeed = inResetXSpeed;
            Reset();
        }
        
        public void InitData(SpaceInvaders gameref, Texture2D texture)
        {
            game = gameref;
            missileTexture = texture;
        }

        // Handle keyboard controls
        private void ProcessInput()
        {
            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.Left))
            {
                StartMovingLeft();
            }
            else
            {
                StopMovingLeft();
            }

            if (keys.IsKeyDown(Keys.Right))
            {
                StartMovingRight();
            }
            else
            {
                StopMovingRight();
            }

            if (keys.IsKeyDown(Keys.LeftControl))
            {
                if(!isShooting)
                {
                    StartShooting();
                    System.Diagnostics.Debug.WriteLine("Start shooting..");
                    isShooting = true;
                }
            }
            else
            {
                isShooting = false;
            }
        }

        // Update
        public override void Update(float deltaTime)
        {
            if(!IsAlive) return;

            ProcessInput();

            if (MovingLeft)
            {
                xPosition -= (xSpeed * deltaTime);
            }
            if (MovingRight)
            {
                xPosition += (xSpeed * deltaTime);
            }

            if (xPosition < 0)
            {
                xPosition = screenWidth;
            } 
            else if (xPosition > screenWidth)
            {
                xPosition = 0;
            }

            base.Update(deltaTime);
        }
        
        // Reset
        public override void Reset()
        {
            health?.Reset();
            MovingLeft = false;
            MovingRight = false;
            SetSpeed(resetXSpeed);
            base.Reset();
        }

        // Draw player health bar
        public void DrawHealthBar(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            string message = "Player Health: " + health.GetHealth().ToString();
            float xPos = (screenWidth - spriteFont.MeasureString(message).X) / 2;
            Vector2 messagePos = new Vector2(550, 5);
            spriteBatch.DrawString(spriteFont, message, messagePos, Color.White);
        }

        // Movement
        public void StartMovingLeft()
        {
            MovingLeft = true;
        }
        public void StopMovingLeft()
        {
            MovingLeft = false;
        }
        public void StartMovingRight()
        {
            MovingRight = true;
        }
        public void StopMovingRight()
        {
            MovingRight = false;
        }
        public void SetSpeed(float inXSpeed)
        {
            xSpeed = inXSpeed;
        }

        // Shooting
        public void StartShooting()
        {
            missile = new Missile(screenWidth, screenHeight, missileTexture, 10, 10, screenWidth / 2, screenHeight / 2);
            missile.Init(this);
            game.LaunchMissile(missile);
        }

        // Extra
        public bool IsAlive
        {
            get
            {
                return health.GetHealth() > 0;
            }
        }
    }
}
