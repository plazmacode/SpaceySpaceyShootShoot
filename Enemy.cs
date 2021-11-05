using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceySpaceyShootShoot
{
    class Enemy : GameObject
    {
        private Random random;
        private SoundEffectInstance explosion;

        public Enemy()
        {
            random = new Random();
        }

        public override void LoadContent(ContentManager content)
        {
            explosion = content.Load<SoundEffect>("8bit_bomb_explosion").CreateInstance();

            sprites = new Texture2D[4];

            sprites[0] = content.Load<Texture2D>("SpaceShooterSprites/enemyBlack1");
            sprites[1] = content.Load<Texture2D>("SpaceShooterSprites/enemyBlue1");
            sprites[2] = content.Load<Texture2D>("SpaceShooterSprites/enemyGreen1");
            sprites[3] = content.Load<Texture2D>("SpaceShooterSprites/enemyRed1");

            Respawn();
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            if (position.Y > GameWorld.ScreenSize.Y)
            {
                Respawn();
            }
        }

        /// <summary>
        /// Reset the enemy position so that it respawns
        /// </summary>
        public void Respawn()
        {
            int index = random.Next(0, 4);
            sprite = sprites[index];

            velocity = new Vector2(0, 1);

            position = new Vector2(random.Next(0, (int)GameWorld.ScreenSize.X - sprite.Width), 0);
            speed = random.Next(100, 250);
        }

        public override void OnCollision(GameObject other)
        {
            //If enemy collides with player, respawn the enemy
            if (other is Player)
            {
                Respawn();
                Explode();
            }

            if (other is Laser)
            {
                GameWorld.Points++;
                Respawn();
                Explode();
            }
        }
        /// <summary>
        /// Plays explosion sound effect
        /// </summary>
        public void Explode()
        {
            if (explosion.State != SoundState.Playing)
            {
                explosion.Play();
            }
        }
    }
}
