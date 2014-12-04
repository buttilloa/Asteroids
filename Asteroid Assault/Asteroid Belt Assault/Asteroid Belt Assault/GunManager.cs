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
        public int Shots = 10;

        public enum CurrentGun {Machine, Rocket, Laser, TriGun, Donut};
        public CurrentGun currentGun = CurrentGun.Machine;

        public GunManager()
        {

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
                playmanager.PlayerShotManager.FireShot(
                 playmanager.playerSprite.Location + playmanager.gunOffset,
                 new Vector2(Direction, -1),
                 true);
                Shots--;
                playmanager.shotTimer = 0.0f;
            }
        }
    }
    
    }

