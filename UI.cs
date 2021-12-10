using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    static class UI
    {
        private static QuantityDisplayBar healthBar;
        private static QuantityDisplayBar staminaBar;
        private static QuantityDisplayBar bossHealthBar;

        public static void LoadContent(ContentManager content)
        {
            healthBar = new QuantityDisplayBar(content, new Vector2(20, 20), Color.DarkRed, new Vector2(200,20));
            staminaBar = new QuantityDisplayBar(content, new Vector2(20, 40), Color.DarkGreen, new Vector2(100, 20));
            bossHealthBar = new QuantityDisplayBar(content, new Vector2(GameWorld.ScreenSize.X/2-300, GameWorld.ScreenSize.Y - 50), Color.DarkRed, new Vector2(600, 20));
        }

        public static void Update(GameTime gameTime)
        {
            healthBar.Update(GameWorld.player.HealthProp, GameWorld.player.MaxHealthProp);
            staminaBar.Update(GameWorld.player.Stamina, GameWorld.player.MaxStamina);
            bossHealthBar.Update(GameWorld.boss.HealthProp, GameWorld.boss.MaxHealthProp);
        }

        public static void Draw(SpriteBatch spritebatch)
        {
            healthBar.Draw(spritebatch);
            staminaBar.Draw(spritebatch);
            bossHealthBar.Draw(spritebatch);
            spritebatch.DrawString(GameWorld.arial, GameWorld.boss.Name, new Vector2(GameWorld.ScreenSize.X / 2 - 200, GameWorld.ScreenSize.Y - 100), Color.Red, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
        }
    }
}
