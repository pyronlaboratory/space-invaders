using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;


namespace SpaceInvaders.Content
{
    class Alien : Enemy
    {
        private Vector2 position;

        public Alien(int inScreenWidth, int inScreenHeight, Texture2D inSpriteTexture, int inDrawWidth, float inResetX, float inResetY) :
            base(inScreenWidth, inScreenHeight, inSpriteTexture, inDrawWidth, inResetX, inResetY)
        {
            position = new Vector2(inResetX, inResetY);
        }

        // Public property to get the position as a Vector2
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        // Update
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            xPosition = position.X;
            yPosition = position.Y;
        }
    }
}
