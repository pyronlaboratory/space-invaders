using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Content
{
    public class Missile : Sprite
    {
        private float speed = 200;

        public Missile(int inScreenWidth, int inScreenHeight, Texture2D inSpriteTexture, int inDrawWidth, int inDrawHeight, float inResetX, float inResetXSpeed) :
              base(inScreenWidth, inScreenHeight, inSpriteTexture, inDrawWidth, inResetX, inDrawHeight)
        {
            Reset();
        }

        public void Init(Player player)
        {
            SetPosition(player.GetPosition().X + 20, player.GetPosition().Y);
        }

        public override void Update(float deltaTime)
        {
            yPosition = yPosition - (speed * deltaTime);
            base.Update(deltaTime);
        }

        
    }
}
