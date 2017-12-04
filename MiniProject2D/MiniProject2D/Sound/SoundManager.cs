using System.Linq.Expressions;
using Microsoft.Xna.Framework.Audio;
using MiniProject2D.Resource;

namespace MiniProject2D.Sound
{
    class SoundManager
    {

        private class BackupObject
        {
            public float MusicVolumn;
            public float SoundVolumn;
            public bool IsMute;
        }

        private BackupObject backupObject;

        public static SoundManager Instance;

        private SoundEffectInstance currentMusicInstance;
        private SoundEffect currentMusic;
        private float soundVolumn = 1;

        public float SoundVolumn
        {
            get { return soundVolumn; }
            set { soundVolumn = value; }
        }

        public float MusicVolumn
        {
            get { return currentMusicInstance.Volume; }
            set { currentMusicInstance.Volume = value; }
        }


        public void PlaySound(SoundEffect music)
        {
            var instance = music.CreateInstance();
            instance.Volume = SoundVolumn;
            instance.Play();
        }

        public void Backup()
        {
            backupObject = new BackupObject()
            {
                MusicVolumn = currentMusicInstance.Volume,
                SoundVolumn = soundVolumn,
                IsMute = currentMusicInstance.State == SoundState.Paused
            };
        }

        public void Recover()
        {
            if (backupObject == null) return;
            currentMusicInstance.Volume = backupObject.MusicVolumn;
            soundVolumn = backupObject.SoundVolumn;
            if (backupObject.IsMute)
            {
                PauseMusic();
            }
            else
            {
                ResumeMusic();
            }
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


        public void StopMusic()
        {
            if (currentMusic == null) return;
            currentMusicInstance.Stop();
            currentMusicInstance.Dispose();
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
