using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ModalWindowsController : MonoBehaviour
{
    [Inject] private UICanvasRoot _root;
    [Inject] private PrefabHolder _prefabHolder;
    private BaseModalWindow _currentModalWin;

    public void ShowModalWin<T>(BaseModalWinArgs args) where T: BaseModalWindow
     {
        foreach (var win in _prefabHolder.modalWindowList)
        {
            if (win is T result)
            {
                _currentModalWin = Instantiate (result, _root.uiRootCanvas.transform);
                _currentModalWin.transform.localPosition = Vector3.zero;
                _currentModalWin.transform.localScale = Vector3.one;
                _currentModalWin.Show(args);
                break;
            }
        }
     }
}
