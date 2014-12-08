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
        private float playerSpeed = 260.0f;
        private Rectangle playerAreaLimit;
        GunManager gunManager;
        public long PlayerScore = 0;
        public int LivesRemaining = 3;
        public bool Destroyed = false;
        bool canSwitch = true;
        public MouseState ms;
        Random rand = new Random();
       
        public Vector2 gunOffset = new Vector2(25, 10);
        public float shotTimer = 0.0f;
        public float minShotTimer = 0.2f;
        private int playerRadius = 15;
        public ShotManager PlayerShotManager;
        Sprite Sheild;
        Sprite Sheildbar;
        Sprite SheildPowerUP;
        int SheildRemaining = 999;
       
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
            Sheild = new Sprite(new Vector2(740, 100),
                texture,
                new Rectangle(0,399,42,45),
                Vector2.Zero);
            Sheildbar = new Sprite(new Vector2(762, 155),
               texture,
               new Rectangle(45, 400, 2, 46),
               Vector2.Zero);
            SheildPowerUP = new Sprite(new Vector2(0, 0),
               texture,
               new Rectangle(0, 399, 42, 45),
               Vector2.Zero);
            SheildPowerUP.isVisible = false;
            
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
            ms = Mouse.GetState();
           
            for (int i = 0; i <= gunManager.Shots.Length - 1; i++)
            {
                int temp = i + 1;
                string test = "D"+ temp;
                if (keyState.IsKeyDown((Keys)Enum.Parse(typeof(Keys), test)))  
                {
                    gunManager.currentGun = (GunManager.CurrentGun)i;
                }
            }
            if (keyState.IsKeyDown(Keys.Up)||keyState.IsKeyDown(Keys.W))
            {
                playerSprite.Velocity += new Vector2(0, -1);
            }

            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                playerSprite.Velocity += new Vector2(0, 1);
            }

            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                playerSprite.Velocity += new Vector2(-1, 0);
                if (playerSprite.Rotation > -.6f)
                    playerSprite.Rotation -= .1f;
            }
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                playerSprite.Velocity += new Vector2(1, 0);
                if (playerSprite.Rotation < .6f)
                    playerSprite.Rotation += .1f;
            }
            if (keyState.IsKeyUp(Keys.Left) && keyState.IsKeyUp(Keys.Right) && keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.D))
            {

                if (playerSprite.Rotation > 0f)
                    playerSprite.Rotation -= .1f;
                if (playerSprite.Rotation < 0f)
                    playerSprite.Rotation += .1f;
            }
            if (gunManager.Shots[(int)gunManager.currentGun] > 0)
            {
                if (keyState.IsKeyDown(Keys.Space) || ms.LeftButton == ButtonState.Pressed)
                {
                  
                    gunManager.FireShot(0, this, ms);
                }
           }
            if (keyState.IsKeyDown(Keys.R) || ms.RightButton == ButtonState.Pressed)
                gunManager.relode();
            
            if (keyState.IsKeyDown(Keys.Z) && canSwitch)
            {
                gunManager.cycleGun(-1);
                canSwitch = false;
            }
            else if ((keyState.IsKeyDown(Keys.X)||ms.MiddleButton == ButtonState.Pressed) && canSwitch)
            {
                gunManager.cycleGun(1);
                canSwitch = false;
            }
            else if (keyState.IsKeyUp(Keys.X) && keyState.IsKeyUp(Keys.Z)&&ms.MiddleButton == ButtonState.Released  && !canSwitch)
                canSwitch = true;
           
        }
        
        private void HandleGamepadInput(GamePadState gamePadState)
        {
          
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
        public bool SheildVisible
        {
            get { return Sheild.isVisible; }
            set { Sheild.isVisible = value; Sheildbar.isVisible = value;}
        }
        public void Update(GameTime gameTime)
        {
            PlayerShotManager.Update(gameTime);
            for (int i = 0; i < 17;i++)
                if (PlayerScore > i*400)
                    SoundManager.Music[i].Volume = 1f;
          
            SheildPowerUP.Update(gameTime);
            SheildRemaining--;
            
            String temp = SheildRemaining.ToString();
            if (SheildRemaining > 10)
            {
                SheildVisible = true;
                int temp2 = Convert.ToInt32(temp.Substring(0, temp.Length - 1));
                Sheildbar.frames[0] = new Rectangle(45, 400, 2, temp2);
                if (SheildRemaining > 230)
                    Sheildbar.TintColor = Color.Green;
                else Sheildbar.TintColor = Color.Red;
            }
            else SheildVisible = false;
                
            if (rand.Next(0, 1000) == 1 && !SheildPowerUP.isVisible && !SheildVisible)
            {
                SheildPowerUP.isVisible = true;
                SheildPowerUP.Location = new Vector2(rand.Next(0, 800), -50);
                SheildPowerUP.Velocity = new Vector2(0, 100);
            }
            if (SheildPowerUP.Location.Y > 700)
            {
                SheildPowerUP.Location = new Vector2(0, 0);
                SheildPowerUP.isVisible = false;
            }
            if (SheildPowerUP.IsCircleColliding(playerSprite.Center, playerSprite.CollisionRadius))
            {
                SheildRemaining = 460;
                SheildPowerUP.Location = new Vector2(0, 0);
                SheildPowerUP.isVisible = false;

            }
            if (!Destroyed)
            {
                playerSprite.Velocity = Vector2.Zero;

                shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                HandleKeyboardInput(Keyboard.GetState());
                HandleGamepadInput(GamePad.GetState(PlayerIndex.One));

                playerSprite.Velocity.Normalize();
                playerSprite.Velocity *= playerSpeed;

                EffectManager.Effect("ShipSmokeTrail").Trigger(new Vector2(playerSprite.Center.X, playerSprite.Center.Y + 10));
                playerSprite.Update(gameTime);
                imposeMovementLimits();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerShotManager.Draw(spriteBatch);
            if (SheildVisible)
            {
                Sheild.Draw(spriteBatch);
                Sheildbar.Draw(spriteBatch);
                if (rand.Next(0, 50) == 1) EffectManager.Effect("ShieldBounce").Trigger(playerSprite.Center);
                EffectManager.Effect("ShieldsUp").Trigger(playerSprite.Center);
            }
                        
            SheildPowerUP.Draw(spriteBatch);
            if (!Destroyed)
            {
                playerSprite.Draw(spriteBatch);
            }
        }

    }
}
