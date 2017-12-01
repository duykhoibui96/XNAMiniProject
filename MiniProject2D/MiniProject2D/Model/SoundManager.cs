using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using MiniProject2D.Resource;

namespace MiniProject2D.Model
{
    class SoundManager
    {
        public static SoundManager Instance;

        private SoundEffectInstance gameMusic;
        private SoundEffectInstance menuMusic;
        private SoundEffect monsterEncounter;

        private SoundManager()
        {
            gameMusic = ResManager.Instance.GameMusic.CreateInstance();
            gameMusic.IsLooped = true;
            menuMusic = ResManager.Instance.MenuMusic.CreateInstance();
            menuMusic.IsLooped = true;
            monsterEncounter = ResManager.Instance.MonsterEncounter;
        }

        static SoundManager()
        {
            Instance = new SoundManager();
        }

        public void PlayGameMusic()
        {
            if (gameMusic.State.Equals(SoundState.Stopped))
                gameMusic.Play();
        }

        public void StopPlayingGameMusic()
        {
            if (gameMusic.State.Equals(SoundState.Playing))
                gameMusic.Stop();
        }

        public void PlayMenuMusic()
        {
            if (menuMusic.State.Equals(SoundState.Stopped))
                menuMusic.Play();
        }

        public void StopPlayingMenuMusic()
        {
            if (menuMusic.State.Equals(SoundState.Playing))
                menuMusic.Stop();
        }

        public void PlaySoundWhenEncounterMonster()
        {
            monsterEncounter.Play();
        }
    }
}
