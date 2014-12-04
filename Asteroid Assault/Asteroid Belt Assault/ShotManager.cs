using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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
        GunManager gunManager;

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
                InitialFrame = new Rectangle(0, 311, 25, 46);
                extraCollide = 30;
            }
            Sprite thisShot = new Sprite(
                location,
                Texture,
                InitialFrame,
                velocity);

            thisShot.Velocity *= shotSpeed;

            for (int x = 1; x < 4; x++)
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
