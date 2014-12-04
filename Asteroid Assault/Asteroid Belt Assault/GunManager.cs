using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Belt_Assault
{
    class GunManager
    {
      
        public int[] Shots = new int[]{}; 
        public int[] ShotsOG = new int[] {  30,      3,    100,    30,     10 };
        public enum CurrentGun           {Machine, Rocket, Laser, TriGun, Donut};
        public CurrentGun currentGun = CurrentGun.Machine;
        
        public GunManager()
        {
            Shots = ShotsOG;
        }
        public void cycleGun(int direction)
        {
            int current = (int)currentGun;

           int Max = Enum.GetNames(typeof(CurrentGun)).Length - 1;

            if (direction > 0 && current != Max)
                current++;
            if (direction < 0 && current != 0)
                current--;

           currentGun = (CurrentGun)current;
        
        }
       
        public void updateGuns(GameTime gameTime)
        {
            

        }
        public void FireShot(int Direction,PlayerManager playmanager) // -1=left 0=straight 1=right
        {
            if (playmanager.shotTimer >= playmanager.minShotTimer)
            {
                Vector2 Location = playmanager.playerSprite.Location + playmanager.gunOffset;
               
                if(currentGun == CurrentGun.TriGun)
                {
                  
                    playmanager.PlayerShotManager.FireShot(Location, new Vector2(-1, -1), true);
                 if(Shots[(int)currentGun] >=2)   playmanager.PlayerShotManager.FireShot(Location, new Vector2(0, -1), true);
                 if (Shots[(int)currentGun] >= 3) playmanager.PlayerShotManager.FireShot(Location, new Vector2(1, -1), true);
                 Shots[(int)currentGun] -= 2;
                }
                else  
                    playmanager.PlayerShotManager.FireShot(Location,new Vector2(Direction, -1), currentGun);
                Shots[(int)currentGun]--;
                playmanager.shotTimer = 0.0f;
            }
        }
    }
    
    }

