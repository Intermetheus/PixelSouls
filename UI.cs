using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    /// <summary>
    /// Manages UI elements
    /// </summary>
    static class UI
    {
        private static QuantityDisplayBar healthBar;
        private static QuantityDisplayBar staminaBar;
        private static QuantityDisplayBar bossHealthBar;

        /// <summary>
        /// Instantiates UI elements
        /// </summary>
        /// <param name="content">Content reference for loading sprites</param>
        public static void LoadContent(ContentManager content)
        {
            healthBar = new QuantityDisplayBar(content, new Vector2(20, 20), Color.DarkRed, new Vector2(200,20));
            staminaBar = new QuantityDisplayBar(content, new Vector2(20, 40), Color.DarkGreen, new Vector2(100, 20));
            bossHealthBar = new QuantityDisplayBar(content, new Vector2(GameWorld.ScreenSizeProp.X/2-300, GameWorld.ScreenSizeProp.Y - 50), Color.DarkRed, new Vector2(600, 20));
        }

        /// <summary>
        /// Runs update methods on all UI elements
        /// </summary>
        public static void Update()
        {
            healthBar.Update(GameWorld.PlayerProp.HealthProp, GameWorld.PlayerProp.MaxHealthProp);
            staminaBar.Update(GameWorld.PlayerProp.Stamina, GameWorld.PlayerProp.MaxStamina);
            bossHealthBar.Update(GameWorld.BossProp.HealthProp, GameWorld.BossProp.MaxHealthProp);
        }

        /// <summary>
        /// Runs draw methods on all UI elements and draws text to screen
        /// </summary>
        /// <param name="spritebatch">Spritebatch reference for drawing</param>
        public static void Draw(SpriteBatch spritebatch)
        {
            healthBar.Draw(spritebatch);
            staminaBar.Draw(spritebatch);
            bossHealthBar.Draw(spritebatch);
            spritebatch.DrawString(GameWorld.ArialProp, GameWorld.BossProp.Name, new Vector2(GameWorld.ScreenSizeProp.X / 2 - 200, GameWorld.ScreenSizeProp.Y - 100), Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
        }
    }
}
