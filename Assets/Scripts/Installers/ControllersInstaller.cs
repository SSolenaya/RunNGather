using UnityEngine;
using Zenject;

public class ControllersInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BonusGatesController>().AsSingle().NonLazy();

        Container.Bind<PlayerController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        Container.Bind<RoadController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        Container.Bind<ModalWindowsController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        Container.Bind<MainLogic>().AsSingle().NonLazy();

        Container.Bind<MenuController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        
        
    }
}