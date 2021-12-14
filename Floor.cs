using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    /// <summary>
    /// Tileable background construction.
    /// </summary>
    class Floor : GameObject
    {
        /// <summary>
        /// Uses the supplied coordinates and sprite to construct one tile.
        /// </summary>
        /// <param name="x">x coordinate of tile position.</param>
        /// <param name="y">y coordinate of tile position.</param>
        /// <param name="sprite">The image being used.</param>
        public Floor(int x, int y, Texture2D sprite)
        {
            this.position = new Vector2(x, y);
            this.sprite = sprite;
            CreateOrigin();

            scale = 1f;
            layerDepth = 0.1f;
        }

        /// <summary>
        /// Floor Class should not load content
        /// </summary>
        /// <param name="content">Content reference for loading assets</param>
        public override void LoadContent(ContentManager content)
        {
            // This should be loaded in Stage
        }

        /// <summary>
        /// Updates the screenposition of floors based on camera position and actual position in the gameWorld.
        /// screenPosition variable is used to draw the sprites correctly, dependent on where the camera has moved.
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
        public override void Update(GameTime gameTime)
        {
            screenPosition = position - GameWorld.CameraPositionProp;
        }

        public override void OnCollision(GameObject other)
        {
            
        }
    }
}
