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
            CreateOrigin();

            scale = 1f;
            layerDepth = 0.8f;
        }

        /// <summary>
        /// The target is only for debug
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, layerDepth);
#endif
        }
        /// <summary>
        /// Sets the position of the target to the TargetedPosition, which is updated in the Player Move() method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            position = GameWorld.PlayerProp.TargetedPosition;
        }

        public override void OnCollision(GameObject other)
        {

        }
    }
}
