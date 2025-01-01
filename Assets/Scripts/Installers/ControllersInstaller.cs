using UnityEngine;
using Zenject;

public class ControllersInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GatesController>().AsSingle();

        Container.Bind<PlanksManager>().AsSingle();

        Container.Bind<CharacterModelsController>().AsSingle();

        Container.Bind<PlayerController>().FromNewComponentOnNewGameObject().AsSingle();

        Container.Bind<RoadController>().FromNewComponentOnNewGameObject().AsSingle();

        Container.Bind<EnvironmentObjectsController>().FromNewComponentOnNewGameObject().AsSingle();

        Container.Bind<ModalWindowsController>().FromNewComponentOnNewGameObject().AsSingle();

        Container.Bind<MainLogic>().AsSingle();

        Container.Bind<MenuController>().FromNewComponentOnNewGameObject().AsSingle();
        
        
    }
}