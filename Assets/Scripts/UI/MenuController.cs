using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MenuController : MonoBehaviour
{
    [Inject] private UICanvasRoot _root;
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private MainLogic _mainLogic;
    [Inject] private AudioController _audioController;
    private StartingMenuWin _currentMenuWin;

    public void ShowMainMenu()
    {
        if (_currentMenuWin == null)
        {
            _currentMenuWin = Instantiate(_prefabHolder.mainMenuWinPrefab, _root.transform);
            _currentMenuWin.transform.localPosition = Vector3.zero;
            _currentMenuWin.transform.localScale = Vector3.one;
            _currentMenuWin.Setup(_mainLogic, _audioController);
        }
        _currentMenuWin.SetVisibility(true);
        

    }
}
