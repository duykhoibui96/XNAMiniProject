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
        private SoundEffectInstance footSteps;
        private SoundEffect monsterEncounter;
        private SoundEffect win;
        private SoundEffect lose;

        private SoundManager()
        {
            gameMusic = ResManager.Instance.GameMusic.CreateInstance();
            gameMusic.IsLooped = true;
            gameMusic.Volume = 0.5f;
            menuMusic = ResManager.Instance.MenuMusic.CreateInstance();
            menuMusic.IsLooped = true;
            footSteps = ResManager.Instance.FootSteps.CreateInstance();
            footSteps.Pitch = 1;
            monsterEncounter = ResManager.Instance.MonsterEncounter;
            win = ResManager.Instance.WinSound;
            lose = ResManager.Instance.LoseSound;
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

        public void PlaySoundWhenWin()
        {
            win.Play();
        }
        public void PlaySoundWhenLose()
        {
            lose.Play();
        }
        public void PlayFootStepSound()
        {
            if (footSteps.State.Equals(SoundState.Stopped))
                footSteps.Play();
        }

        public void StopPlayingFootStepSound()
        {
            if (footSteps.State.Equals(SoundState.Playing))
                footSteps.Stop();
        }
    }
}
