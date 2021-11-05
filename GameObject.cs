using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceySpaceyShootShoot
{
    public abstract class GameObject
    {
        protected Vector2 position;
        protected Texture2D sprite;
        protected Texture2D[] sprites;
        protected float fps;
        protected Vector2 origin;
        protected Vector2 velocity;
        protected float speed;
        private float timeElapsed;
        private int currentIndex;

        public GameObject()
        {

        }
        
        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        protected void Move(GameTime gameTime)
        {
            //Calculates the deltaTime based on the gameTime
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Moves the player based on the result from HandleInput, speed and deltaTime
            position += (velocity * speed * deltaTime);
        } 

        protected void Animate(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentIndex = (int)(timeElapsed * fps);

            sprite = sprites[currentIndex];

            if (currentIndex >= sprites.Length - 1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }
        }

        /// <summary>
        /// Is executed when a collision occurs
        /// </summary>
        /// <param name="other">The object we collided with</param>
        public abstract void OnCollision(GameObject other);

        /// <summary>
        /// Check if this GameObject has collided with another GameObject
        /// </summary>
        /// <param name="other">The Object we collided with</param>
        public void CheckCollision(GameObject other)
        {
            if(CollisionBox.Intersects(other.CollisionBox)) {
                OnCollision(other);
            }
        }

        /// <summary>
        /// Returns a Rectangle with the size and position of the sprite
        /// </summary>
        public virtual Rectangle CollisionBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); }
        }

    }
}
