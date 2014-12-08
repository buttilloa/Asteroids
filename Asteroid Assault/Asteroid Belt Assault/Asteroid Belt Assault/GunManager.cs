using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroid_Belt_Assault
{
    class GunManager
    {

        public int[] Shots = new int[5] { 100, 1, 100, 30, 10 };
        public int[] ShotsOG = new int[] {  100,     1,    100,    30,     10 };
        public enum CurrentGun           {Machine, Rocket, Laser, TriGun, Donut};
        public CurrentGun currentGun = CurrentGun.Machine;
       
        
        public GunManager()
        {
            //Shots = ShotsOG;
        }
        public void cycleGun(int direction)
        {
            int current = (int)currentGun;

           int Max = Enum.GetNames(typeof(CurrentGun)).Length - 1;

           if (direction > 0)
               if (current == Max)
                   current = 0;
               else
                current++;

            if (direction < 0)
                if (current == 0)
                    current = Max;
                else
                current--;

           currentGun = (CurrentGun)current;
        
        }

        public void FireShot(int Direction,PlayerManager playmanager, MouseState Ms) // -1=left 0=straight 1=right
        {
            Vector2 Location = playmanager.playerSprite.Location + playmanager.gunOffset;
            Location.X -= 3;
            Vector2 target = new Vector2(Ms.X, Ms.Y);
            Vector2 vel = target - Location;
            vel.Normalize();
            vel *= 1;
            Vector2 Velocity = vel;
                 
            if (currentGun == CurrentGun.Laser)
            {
                playmanager.PlayerShotManager.FireShot(Location, Velocity, currentGun);
                Shots[(int)currentGun]--;
            }
            else if (playmanager.shotTimer >= playmanager.minShotTimer)
            {
                if (currentGun == CurrentGun.TriGun)
                {

                    playmanager.PlayerShotManager.FireShot(Location, Velocity, true);
                    if (Shots[(int)currentGun] >= 2) playmanager.PlayerShotManager.FireShot(Location, new Vector2(Velocity.X -1,Velocity.Y), true);
                    if (Shots[(int)currentGun] >= 3) playmanager.PlayerShotManager.FireShot(Location, new Vector2(Velocity.X + 1, Velocity.Y), true);
                    Shots[(int)currentGun] -= 2;
                }
                else
                    playmanager.PlayerShotManager.FireShot(Location, Velocity, currentGun);
                Shots[(int)currentGun]--;
                playmanager.shotTimer = 0.0f;
            }
        }

        internal void relode()
        {
            int change = ShotsOG[(int)currentGun];
            Shots[(int)currentGun] = change;
          
        }
    }
    
    }

