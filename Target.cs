using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    /// <summary>
    /// Only used for debugging purposes
    /// Creates a target which shows where the boss i attacking.
    /// The target is based on where the player was, when the boss starts attacking.
    /// The time it takes before the boss attacks is determined by attackCooldown in the Boss Class
    /// </summary>
    class Target : GameObject
    {
        /// <summary>
        /// Loads the target texture
        /// </summary>
        /// <param name="content">Content reference for loading assets</param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("target");
            CreateOrigin();

            scale = 1f;
            layerDepth = 0.8f;
        }

        /// <summary>
        /// The target is only for debug, therefore the draw is inside an if DEBUG statement
        /// </summary>
        /// <param name="spriteBatch">Spritebatch reference for drawing sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, layerDepth);
#endif
        }
        /// <summary>
        /// Sets the position of the target to the TargetedPosition, which is updated in the Player Move() method
        /// </summary>
        /// <param name="gameTime"> Time reference for running update code at a fixed interval</param>
        public override void Update(GameTime gameTime)
        {
            //The position of the TargetedPosition is updated in the Player Class Move() method
            position = GameWorld.PlayerProp.TargetedPosition;
        }

        public override void OnCollision(GameObject other)
        {

        }
    }
}
