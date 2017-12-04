using Microsoft.Xna.Framework.Audio;
using MiniProject2D.Resource;

namespace MiniProject2D.Sound
{
    class SoundManager
    {
        public static SoundManager Instance;

        private SoundEffectInstance currentMusicInstance;
        private SoundEffect currentMusic;

        public void PlaySound(SoundEffect music)
        {
            music.Play();
        }

        public void PlayMusic(SoundEffect music)
        {
            if (currentMusic != null && currentMusic.Equals(music)) return;
            StopMusic();
            currentMusic = music;
            currentMusicInstance = music.CreateInstance();
            currentMusicInstance.Volume = 0.5f;
            currentMusicInstance.IsLooped = true;
            currentMusicInstance.Play();
        }

        public void SetVolumn(float vol)
        {
            currentMusicInstance.Volume = vol;
        }

        public void StopMusic()
        {
            if (currentMusic == null) return;
            currentMusicInstance.Stop();
        }

        public void PauseMusic()
        {
            if (currentMusic == null) return;
            currentMusicInstance.Pause();
        }

        public void ResumeMusic()
        {
            if (currentMusic == null) return;
            currentMusicInstance.Resume();
        }

        private SoundManager()
        {

        }

        static SoundManager()
        {
            Instance = new SoundManager();
        }


    }
}
