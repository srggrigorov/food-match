using FoodMatch.Game;
using FoodMatch.Menu;
using UnityEngine;
using Zenject;

namespace FoodMatch.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField]
        private AudioSource _musicSource;
        [SerializeField]
        private AudioSource _sfxSource;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelLoaderService>().AsSingle().NonLazy();
            
            if (!Container.HasBinding<SoundService>())
            {
                Container.Bind<SoundService>().AsSingle().WithArguments(_musicSource, _sfxSource).NonLazy();
            }
        }
    }
}
