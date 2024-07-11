using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Random = System.Random;

namespace FoodMatch.Game
{


    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField]
        private Canvas _loadingCanvas;
        [SerializeField]
        private Image _background;

        [SerializeField]
        private List<RectTransform> _foodImages;

        private static SceneSwitcher _instance;
        private ZenjectSceneLoader _sceneLoader;

        [Inject]
        private void Construct(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            _instance = this;
            DontDestroyOnLoad(this);
        }

        public async UniTask SwitchSceneAsync(string sceneName, LoadSceneMode loadSceneMode, Action<DiContainer> extraBindings = null)
        {
            extraBindings += container => container.Bind<SceneSwitcher>().FromInstance(this).AsSingle().NonLazy();
            var scene = _sceneLoader.LoadSceneAsync(sceneName, loadSceneMode, extraBindings);

            Vector3[] startPositions = new Vector3[_foodImages.Count];
            for (int i = 0; i < _foodImages.Count; i++)
            {
                startPositions[i] = _foodImages[i].position;
            }
            _loadingCanvas.gameObject.SetActive(true);
            Random rand = new Random();
            _background.DOFade(1, 0.3f).OnComplete(() =>
                {
                    for (int i = 0; i < _foodImages.Count; i++)
                    {

                        var rotateTween = _foodImages[i].DORotate(new Vector3(0, 0, 360), rand.Next(4, 9))
                            .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);

                        _foodImages[i].DOMove(new Vector3(startPositions[i].x, -100, startPositions[i].z),
                            rand.Next(150, 401)).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetSpeedBased(true);
                    }
                });


            await scene;
            for (int i = 0; i < _foodImages.Count; i++)
            {
                DOTween.Kill(_foodImages[i]);
                _foodImages[i].position = startPositions[i];
            }
            _background.DOFade(0, 0.3f).OnComplete(() => _loadingCanvas.gameObject.SetActive(false));
        }
    }
}
