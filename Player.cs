using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceySpaceyShootShoot
{
    class Player : GameObject
    {
        private Vector2 spawnOffset;
        private Texture2D laserSprite;
        private SoundEffectInstance laserSound;

        private bool canFire;
        private int fireCooldown;
        
        public Player()
        {
            speed = 1500;
            fps = 10;
            velocity = Vector2.Zero;
            spawnOffset = new Vector2(50,-50);
            position = new Vector2(GameWorld.ScreenSize.X / 2 - 50, GameWorld.ScreenSize.Y - 150);
            canFire = true;
            fireCooldown = 0;
        }


        public override void LoadContent(ContentManager content)
        {
            laserSound = content.Load<SoundEffect>("laser").CreateInstance();

            sprites = new Texture2D[4];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = content.Load<Texture2D>("SpaceShooterSprites/" + (i + 1) + "fwd");
            }

            sprite = sprites[0];

            laserSprite = content.Load<Texture2D>("SpaceShooterSprites/laserGreen1");
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            Move(gameTime);
            ScreenWrap();
            ScreenLimits();
            Animate(gameTime);
        }

        private void HandleInput()
        {
            //Reset velocity when no keys are pressed
            velocity = Vector2.Zero;

            KeyboardState keyState = Keyboard.GetState();

            if(keyState.IsKeyDown(Keys.W))
            {
                velocity += new Vector2(0, -1);
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                velocity += new Vector2(0, 1);
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                velocity += new Vector2(-1, 0);
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
            }
            //Fire lasers
            if (keyState.IsKeyDown(Keys.Space) && canFire == true)
            {
                canFire = false;
                GameWorld.Instantiate(new Laser(laserSprite, new Vector2(position.X+spawnOffset.X, position.Y+spawnOffset.Y)));
                PlayLaser();
            }

            ///Firetrigger < x, changes fire rate
            if (!canFire && fireCooldown < 40)
            {
                fireCooldown++;
            }
            else
            {
                canFire = true;
                fireCooldown = 0;
            }

            //If key is pressed normalize the vector
            //If this is not done we will move faster when two keys are pressed
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
        }

        /// <summary>
        /// Play laser sound effect
        /// </summary>
        private void PlayLaser()
        {
            laserSound.Play();
        }

        /// <summary>
        /// Wrap the screen, so the player can go around the edges to the other side
        /// </summary>
        private void ScreenWrap()
        {
            if (position.X > GameWorld.ScreenSize.X)
            {
                position.X = 0 - sprite.Width;
            }

            if (position.X < 0 - sprite.Width)
            {
                position.X = GameWorld.ScreenSize.X;
            }
        }

        /// <summary>
        /// Limit the player position to stay inside the game window
        /// </summary>
        private void ScreenLimits()
        {
            if (position.Y < 0)
            {
                position.Y = 0;
            }

            if (position.Y > GameWorld.ScreenSize.Y - sprite.Height / 2)
            {
                position.Y = GameWorld.ScreenSize.Y - sprite.Height / 2;
            }
        }
        public override void OnCollision(GameObject other)
        {
            
        }
    }
}
