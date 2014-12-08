using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;


namespace Asteroid_Belt_Assault
{
    public static class SoundManager
    {
        private static List<SoundEffect> explosions = new
            List<SoundEffect>();
        public static List<SoundEffectInstance> Music = new
            List<SoundEffectInstance>();
        private static Song FinalSong;
        private static int explosionCount = 4;
        private static int MusicCount = 17;
        

        private static SoundEffect playerShot;
        private static SoundEffect enemyShot;

        private static Random rand = new Random();

        public static void Initialize(ContentManager content)
        {
            try
            {
                playerShot = content.Load<SoundEffect>(@"Sounds\Pew");
                enemyShot = content.Load<SoundEffect>(@"Sounds\boop");

                for (int x = 1; x <= explosionCount; x++)
                {
                    explosions.Add(
                        content.Load<SoundEffect>(@"Sounds\Explosion" +
                            x.ToString()));
                }
                for (int x = 0; x < MusicCount; x++)
                {
                    

                    SoundEffect sfx = content.Load<SoundEffect>(@"Music\b" +
                            (x+1).ToString());
                    SoundEffectInstance sfxi = sfx.CreateInstance();
                    sfxi.IsLooped = true;
                    sfxi.Volume = 0;

                    Music.Add(sfxi);
                }
            }
            catch
            {
                Debug.Write("SoundManager Initialization Failed");
            }
            PlayMusic();
        }

        public static void PlayExplosion()
        {
            try
            {
                explosions[rand.Next(0, explosionCount)].Play();
            }
            catch
            {
                Debug.Write("PlayExplosion Failed");
            }
        }
        public static void PlayMusic()
        {
            Music[0].Volume = 1f;
            

           for (int x = 0; x < Music.Count(); x++)
            {

                Music[x].Play();

            }
        
        }
        public static void PlayPlayerShot()
        {
            try
            {
                playerShot.Play();
            }
            catch
            {
                Debug.Write("PlayPlayerShot Failed");
            }
        }

        public static void PlayEnemyShot()
        {
            try
            {
                enemyShot.Play();
            }
            catch
            {
                Debug.Write("PlayEnemyShot Failed");
            }
        }

    }
}
