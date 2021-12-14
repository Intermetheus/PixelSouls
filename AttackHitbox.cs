using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PixelSouls
{
    /// <summary>
    /// Game object that can be instantiated into the world and damage Character class objects other than the creator.
    /// This class is a form of game object deliberately bending the rules of of the game object class to be more simplistic.
    /// </summary>
    class AttackHitbox : GameObject
    {
        private int attackDamage;
        private int attackWidth;
        private int attackHeight;

        private GameObject attacker;   // Field used to store the identity of the creator, ensuring an attacker can never damage itself.

        /// <summary>
        /// Lacking a sprite, this game object uses custom values to establish its collision box, requiring an override for the property.
        /// </summary>
        public override Rectangle CollisionBoxProp
        {
            get { return new Rectangle(collisionBox.X, collisionBox.Y, attackWidth, attackHeight); }
        }

        /// <summary>
        /// Constructor for attack hitbox sets local fields and updates its own collision box.
        /// </summary>
        /// <param name="attacker">Identity of the attacker.</param>
        /// <param name="position">Position of the attack.</param>
        /// <param name="attackDamage">Damage dealt by the attack</param>
        /// <param name="attackWidth">Width of the attack</param>
        /// <param name="attackHeight">Height of the attack</param>
        public AttackHitbox(GameObject attacker, Vector2 position, int attackDamage, int attackWidth, int attackHeight)
        {
            this.attacker = attacker;
            this.attackDamage = attackDamage;
            this.attackWidth = attackWidth;
            this.attackHeight = attackHeight;

            collisionBox.X = (int)position.X;
            collisionBox.Y = (int)position.Y;
            collisionBox.Width = attackWidth;
            collisionBox.Height = attackHeight;
        }

        /// <summary>
        /// Empty to allow the hitbox to be a game object, though it has no content of its own.
        /// </summary>
        /// <param name="content">Content reference for loading assets</param>
        public override void LoadContent(ContentManager content)
        {
            
        }

        /// <summary>
        /// Update immidiately destroys this object, ensuring it exists for only one run through of the code.
        /// This ensures damage can only be dealt once per attack.
        /// </summary>
        /// <param name="gameTime">Time reference for running update code at a fixed interval</param>
        public override void Update(GameTime gameTime)
        {
            GameWorld.Destroy(this);
        }

        /// <summary>
        /// Empty to allow the hitbox to be a game object, though it has no sprite of its own
        /// </summary>
        /// <param name="spriteBatch">Spritebatch reference for drawing sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        /// <summary>
        /// On collision override for the attack hitbox, purely concerned with dealing damage to objects it intersects with.
        /// </summary>
        /// <param name="other">The game object being hit by the attack</param>
        public override void OnCollision(GameObject other)
        {
            if (other is Character && attacker != other)   // Looks to make sure the GameObject being hit is actually a character and that it is not the attacker themselves.
            {
                other.TakeDamage(attackDamage);   // If these conditions are met, the GameObject that was hit is instructed to run its TakeDamage method.
            }
        }
    }
}
