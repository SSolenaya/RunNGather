using System.ComponentModel;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
public class SOInstaller : ScriptableObjectInstaller
{
    [SerializeField] private PrefabHolder _prefabHolder;
    [SerializeField] private Settings _settings;
    


    public override void InstallBindings()
    {
        Container.Bind<PrefabHolder>()
                 .FromInstance(_prefabHolder)
                 .AsSingle()
                 .NonLazy();

        Container.Bind<Settings>()
                .FromInstance(_settings)
                .AsSingle()
                .NonLazy();

        Container.Bind<GameFieldHelper>()
                 .FromComponentInNewPrefab(_prefabHolder.gameFieldHelperPrefab)
                 .AsSingle()
                 .NonLazy();

        Container.Bind<UICanvasRoot>()
                 .FromComponentInNewPrefab(_prefabHolder.uiCanvasRootPrefab)
                 .AsSingle()
                 .NonLazy();

    }
}