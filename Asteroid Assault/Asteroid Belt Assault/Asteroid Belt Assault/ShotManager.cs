using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Belt_Assault
{
    class ShotManager
    {
        public List<Sprite> Shots = new List<Sprite>();
        private Rectangle screenBounds;

        private static Texture2D Texture;
        private static Rectangle InitialFrame;
        private static Rectangle InitialFrameOG;
        private static int FrameCount;
        private float shotSpeed;
        private static int CollisionRadius;

        public ShotManager(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            int collisionRadius,
            float shotSpeed,
            Rectangle screenBounds)
        {
           
            Texture = texture;
            InitialFrame = initialFrame;
            InitialFrameOG = initialFrame;
            FrameCount = frameCount;
            CollisionRadius = collisionRadius;
            this.shotSpeed = shotSpeed;
            this.screenBounds = screenBounds;
        }
        public void FireShot(
            Vector2 location,
            Vector2 velocity,
            GunManager.CurrentGun gun)
        {
            InitialFrame = InitialFrameOG;
            int extraCollide = 0;
            if (gun == GunManager.CurrentGun.Rocket)
            {
                InitialFrame = new Rectangle(0, 367, 47, 23);
                extraCollide = 30;
              
            }
            /*if (gun == GunManager.CurrentGun.Donut)
            {
                InitialFrame = new Rectangle(0, 311, 25, 46);
                extraCollide = 30;
            }*/
            Sprite thisShot = new Sprite(
                location,
                Texture,
                InitialFrame,
                velocity);
            
            thisShot.Velocity *= shotSpeed+10;
            if (gun == GunManager.CurrentGun.Rocket)
            {
                thisShot.isRocket = true;
                thisShot.Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
                MouseState ms = Mouse.GetState();
                Vector2 msCollide = new Vector2(ms.X, ms.Y);
                for (int i =0; i < EnemyManager.Enemies.Count-1;i++)
                {
                    if (EnemyManager.Enemies[i].EnemySprite.IsCircleColliding(msCollide, 20))
                        thisShot.tracking = i;
                        
                }
            }
            for (int x = 1; x < 3; x++)
            {
                thisShot.AddFrame(new Rectangle(
                    InitialFrame.X + (InitialFrame.Width * x),
                    InitialFrame.Y,
                    InitialFrame.Width,
                    InitialFrame.Height));
                 
            }
            thisShot.CollisionRadius = CollisionRadius+extraCollide;
            Shots.Add(thisShot);

            SoundManager.PlayPlayerShot();
            
        }
        public void FireShot(
          Vector2 location,
          Vector2 velocity,
          bool playerFired)
        {
            InitialFrame = InitialFrameOG;
            Sprite thisShot = new Sprite(
                location,
                Texture,
                InitialFrame,
                velocity);

            thisShot.Velocity *= shotSpeed;

            for (int x = 1; x < FrameCount; x++)
            {
                thisShot.AddFrame(new Rectangle(
                    InitialFrame.X + (InitialFrame.Width * x),
                    InitialFrame.Y,
                    InitialFrame.Width,
                    InitialFrame.Height));
            }
            thisShot.CollisionRadius = CollisionRadius;
            Shots.Add(thisShot);

            if (playerFired)
            {

                SoundManager.PlayPlayerShot();
            }
            else
            {

                SoundManager.PlayEnemyShot();
            }
        }
        public void Update(GameTime gameTime)
        {
            for (int x = Shots.Count - 1; x >= 0; x--)
            {
                Shots[x].Update(gameTime);
                if (Shots[x].isRocket)
                {

                    EffectManager.Effect("MeteroidCollision").Trigger(new Vector2(Shots[x].Center.X, Shots[x].Center.Y));
                    if (Shots[x].tracking > -1)
                    {
                        Vector2 Location = Shots[x].Location;
                       
                        Vector2 target = Enemy.previousLocation;

                        Vector2 vel = target - Location;
                        vel.Normalize();
                        float dif = Vector2.Distance(target, Shots[x].Location) / 2;
                        float modifier = 100 - dif;
                        vel *= 100 + dif;
                        Vector2 Velocity = vel;
                        Shots[x].Velocity = Velocity;


                        Shots[x].Rotation = (float)Math.Atan2(vel.Y, vel.X);
                        Shots[x].Update(gameTime);
                    }
                }
                else
                    EffectManager.Effect("Enemy Cannon Fire").Trigger(Shots[x].Location);
                if (!screenBounds.Intersects(Shots[x].Destination))
                {
                    Shots.RemoveAt(x);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite shot in Shots)
            {
                shot.Draw(spriteBatch);

            }
        }

    
    }
}
