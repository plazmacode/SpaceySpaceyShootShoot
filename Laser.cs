using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceySpaceyShootShoot
{
    class Laser : GameObject
    {
        public Laser(Texture2D sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
            speed = 800;
            velocity = new Vector2(0,-1);
        }

        public override void LoadContent(ContentManager content)
        {

        }

        public override void OnCollision(GameObject other)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            if (position.Y < -50)
            {
                GameWorld.Destroy(this);
            }
        }
    }
}
