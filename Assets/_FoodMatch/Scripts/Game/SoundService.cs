using FoodMatch.Settings;
using UnityEngine;

namespace FoodMatch.Game
{
    public class SoundService
    {
        public SoundClips SoundClips { get; private set; }
        public bool IsMusicEnabled => !_musicSource.mute;
        public bool IsSoundEnabled => !_sfxSource.mute;

        private readonly AudioSource _musicSource;
        private readonly AudioSource _sfxSource;

        public SoundService(AssetsManager assetsManager, AudioSource musicSource, AudioSource sfxSource)
        {
            _musicSource = musicSource;
            _sfxSource = sfxSource;
            SoundClips = assetsManager.GetModuleSettings<SoundClips>();
        }

        public void PlaySoundOnce(AudioClip clip) => PlaySoundOnce(clip, _sfxSource.volume);

        public void PlaySoundOnce(AudioClip clip, float volume)
        {
            _sfxSource.PlayOneShot(clip, volume);
        }

        public void EnableSound(bool value) => _sfxSource.mute = !value;
        public void EnableMusic(bool value) => _musicSource.mute = !value;
    }
}
