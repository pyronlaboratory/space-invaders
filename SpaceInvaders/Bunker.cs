using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    class Bunker : Sprite
    {

        Health health;

        public Bunker(int inScreenWidth, int inScreenHeight, Texture2D inSpriteTexture, int inDrawWidth, float inResetX, float inResetY) :
          base(inScreenWidth, inScreenHeight, inSpriteTexture, inDrawWidth, inResetX, inResetY)
        {
            health = new Health(5);
            Reset();
        }

        public override void Reset()
        {
            health.Reset();
            base.Reset();
        }

        bool SufferDamage()
        {
            health.Damage();

            return health.GetHealth() == 0;
        }
    }

}
