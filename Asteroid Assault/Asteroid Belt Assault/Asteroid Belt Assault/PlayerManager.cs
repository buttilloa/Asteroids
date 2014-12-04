using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Belt_Assault
{
    class PlayerManager
    {
        public Sprite playerSprite;
        private float playerSpeed = 160.0f;
        private Rectangle playerAreaLimit;
        GunManager gunManager;
        public long PlayerScore = 0;
        public int LivesRemaining = 3;
        public bool Destroyed = false;
        bool canSwitch = true;
        //public int Shots = 10;
        
        //public enum CurrentGun {Machine, Rocket, Laser, TriGun, Donut};
       // public CurrentGun currentGun = CurrentGun.Machine;
        
        public Vector2 gunOffset = new Vector2(25, 10);
        public float shotTimer = 0.0f;
        public float minShotTimer = 0.2f;
        private int playerRadius = 15;
        public ShotManager PlayerShotManager;
       
        public PlayerManager(
            Texture2D texture,  
            Rectangle initialFrame,
            int frameCount,
            Rectangle screenBounds)
        {
            playerSprite = new Sprite(
                new Vector2(500, 500),
                texture,
                initialFrame,
                Vector2.Zero);

            PlayerShotManager = new ShotManager(
                texture,
                new Rectangle(0, 300, 5, 5),
                4,
                2,
                250f,
                screenBounds);

            playerAreaLimit =
                new Rectangle(
                    0,
                    screenBounds.Height / 2,
                    screenBounds.Width,
                    screenBounds.Height / 2);

            for (int x = 1; x < frameCount; x++)
            {
                playerSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X + (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            playerSprite.CollisionRadius = playerRadius;
        }

        public void HandleGuns(GunManager guns)
        {
            gunManager = guns;
        }
        private void HandleKeyboardInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.Up))
            {
                playerSprite.Velocity += new Vector2(0, -1);
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                playerSprite.Velocity += new Vector2(0, 1);
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                playerSprite.Velocity += new Vector2(-1, 0);
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                playerSprite.Velocity += new Vector2(1, 0);
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                gunManager.FireShot(-1, this);
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                gunManager.FireShot(1, this);
            }
            if (gunManager.Shots > 0)
            {
                if (keyState.IsKeyDown(Keys.Space) || keyState.IsKeyDown(Keys.S))
                {
                    //FireShot(0);
                    gunManager.FireShot(0, this);
                }
           }
            if (keyState.IsKeyDown(Keys.R))
                gunManager.Shots = 30;
            if (keyState.IsKeyDown(Keys.Z) && canSwitch)
            {
                gunManager.cycleGun(-1);
                canSwitch = false;
            }
            else if (keyState.IsKeyDown(Keys.X) && canSwitch)
            {
                gunManager.cycleGun(1);
                canSwitch = false;
            }
            else if (keyState.IsKeyUp(Keys.X) && keyState.IsKeyUp(Keys.Z) && !canSwitch)
                canSwitch = true;
           
        }
        
        private void HandleGamepadInput(GamePadState gamePadState)
        {
            playerSprite.Velocity +=
                new Vector2(
                    gamePadState.ThumbSticks.Left.X,
                    -gamePadState.ThumbSticks.Left.Y);

            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                gunManager.FireShot(0,this);
            }
        }

        private void imposeMovementLimits()
        {
            Vector2 location = playerSprite.Location;

            if (location.X < playerAreaLimit.X)
                location.X = playerAreaLimit.X;

            if (location.X >
                (playerAreaLimit.Right - playerSprite.Source.Width))
                location.X =
                    (playerAreaLimit.Right - playerSprite.Source.Width);

            if (location.Y < playerAreaLimit.Y)
                location.Y = playerAreaLimit.Y;

            if (location.Y >
                (playerAreaLimit.Bottom - playerSprite.Source.Height))
                location.Y =
                    (playerAreaLimit.Bottom - playerSprite.Source.Height);

            playerSprite.Location = location;
        }

        public void Update(GameTime gameTime)
        {
            PlayerShotManager.Update(gameTime);

            if (!Destroyed)
            {
                playerSprite.Velocity = Vector2.Zero;

                shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                HandleKeyboardInput(Keyboard.GetState());
                HandleGamepadInput(GamePad.GetState(PlayerIndex.One));

                playerSprite.Velocity.Normalize();
                playerSprite.Velocity *= playerSpeed;

                playerSprite.Update(gameTime);
                imposeMovementLimits();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerShotManager.Draw(spriteBatch);

            if (!Destroyed)
            {
                playerSprite.Draw(spriteBatch);
            }
        }

    }
}
