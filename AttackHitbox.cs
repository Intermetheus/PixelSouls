using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace PixelSouls
{
    class AttackHitbox : GameObject
    {
        private int windupMS;
        private int attackDamage;
        private int attackWidth;
        private int attackHight;

        AttackHitbox(int windupMS, int attackDamage, int attackWidth, int attackHight)
        {
            this.windupMS = windupMS;
            this.attackDamage = attackDamage;
            this.attackWidth = attackWidth;
            this.attackHight = attackHight;
        }

        public override void LoadContent(ContentManager content)
        {
            // Empty to allow the hitbox to be a game object, though it has no content of its own
        }

        public override void OnCollision(GameObject other)
        {
        }
        public override void OnCollision(Character other)
        {
            DealDamage(other);
            //if (other is Character)
            //{
                
            //}
        }

        private void DealDamage(Character target)
        {
            target.HealthProp -= attackDamage;
        }
    }
}
