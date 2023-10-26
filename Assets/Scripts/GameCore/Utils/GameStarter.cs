using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameStarter : MonoInstaller
{
    [SerializeField] private Zenject.SceneContext _sceneContext;
    [Inject] private MenuController _menuController;
    [Inject] private MainLogic _mainLogic;

    public override void InstallBindings()
    {
        _sceneContext.PostInstall += OnPostInstall;
    }

    private void OnPostInstall()
    {
        _menuController.ShowMainMenu();
        //_mainLogic.Restart();
        _sceneContext.PostInstall -= OnPostInstall;
    }
}
