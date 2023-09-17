using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;


namespace SpaceInvaders.Content
{
    class Enemy : Sprite
    {
        protected float speed;
        readonly public Health Health;

        public Enemy(int inScreenWidth, int inScreenHeight, Texture2D inSpriteTexture, int inDrawWidth, float inResetX, float inResetY) :
            base(inScreenWidth, inScreenHeight, inSpriteTexture, inDrawWidth, inResetX, inResetY)
        {
            Health = new Health(1);
        }
        
        public bool IsAlive
        {
            get
            {
                return Health.GetHealth() > 0;
            }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
    }
}
