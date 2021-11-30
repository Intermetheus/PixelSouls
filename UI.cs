using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    class UI
    {
        private QuantityDisplayBar healthBar;
        private QuantityDisplayBar staminaBar;
        private QuantityDisplayBar bossHealthBar;

        public virtual void LoadContent(ContentManager content)
        {
            healthBar = new PlayerHealth(content, new Vector2(20, 20), Color.DarkRed, new Vector2(200,20));
            staminaBar = new PlayerStamina(content, new Vector2(20, 40), Color.DarkGreen, new Vector2(100, 20));
            bossHealthBar = new BossHealth(content, new Vector2(GameWorld.ScreenSize.X/2-300, GameWorld.ScreenSize.Y - 50), Color.DarkRed, new Vector2(600, 20));
        }

        public void Update(GameTime gameTime)
        {
            healthBar.Update(100, 100);
            staminaBar.Update(GameWorld.player.Stamina, GameWorld.player.MaxStamina);
            bossHealthBar.Update(100, 100);
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            healthBar.Draw(spritebatch);
            staminaBar.Draw(spritebatch);
            bossHealthBar.Draw(spritebatch);
        }
    }
}
