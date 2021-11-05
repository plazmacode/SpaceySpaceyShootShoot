using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceySpaceyShootShoot
{
    class Star : GameObject
    {
        private Random random;

        public Star()
        {
            random = new Random();
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("SpaceShooterSprites/star");
            Respawn();
        }

        public override void OnCollision(GameObject other)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            if (position.Y > GameWorld.ScreenSize.Y)
            {
                Respawn();
            }
        }

        public void Respawn()
        {
            velocity = new Vector2(0, 1);

            position = new Vector2(random.Next(0, (int)GameWorld.ScreenSize.X - sprite.Width), 0);
            speed = random.Next(300, 500);
        }
    }
}
