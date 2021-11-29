using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    class PlayerStamina : QuantityDisplayBar
    {
        public PlayerStamina(ContentManager content, Vector2 position, Color color, Vector2 size) : base(content, position, color, size)
        {
        }
    }
}
