using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    /// <summary>
    /// Creates a target which shows where the boss i attacking.
    /// The target is based on where the player was, when the boss starts attacking.
    /// The time it takes before the boss attacks is determined by attackCooldown in the Boss Class
    /// </summary>
    class Target : GameObject
    {
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("target");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, origin, 1F, SpriteEffects.None, 1f);
#endif
        }

        public override void Update(GameTime gameTime)
        {
            position = GameWorld.player.TargetedPosition;
        }

        public override void OnCollision(GameObject other)
        {

        }
    }
}
