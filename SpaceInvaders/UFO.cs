using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Content;
using System;

namespace SpaceInvaders
{
    internal class UFO : Enemy
    {
        private Vector2 position;
        private new readonly int screenWidth;
        private int drawWidth;
        public bool movingLeft = false; // Add a flag to track direction

        public UFO(int inScreenWidth, int inScreenHeight, Texture2D inSpriteTexture, int inDrawWidth, int inDrawHeight, float inResetX, float inResetY) :
            base(inScreenWidth, inScreenHeight, inSpriteTexture, inDrawWidth, inResetX, inResetY)
        {
            screenWidth = inScreenWidth;
            drawWidth = inDrawWidth;
            position = new Vector2(inResetX, inResetY);
        }

        // Public property to get and set the position as a Vector2
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Check if UFO should change direction based on its position
            if (xPosition <= 0)
            {
                movingLeft = false;
            }
            else if (xPosition >= screenWidth - drawWidth)
            {
                movingLeft = true;
            }

            // Update the UFO's position based on direction
            if (movingLeft)
            {
                xPosition -= 200 * deltaTime;
            }
            else
            {
                xPosition += 200 * deltaTime;
            }
        }
    }
}
