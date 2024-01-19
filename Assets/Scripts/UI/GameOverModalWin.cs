using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverModalWin : BaseModalWindow
{
    private GameOverWinArgs _args;
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _toMenuBtn;
    [SerializeField] private TMP_Text _distanceValueTxt;
    [SerializeField] private GameObject _distanceMsgObj;

    public override void Close()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    public override void Show(BaseModalWinArgs args)
    {
        _args = args as GameOverWinArgs;
        _restartBtn.onClick.RemoveAllListeners();
        _restartBtn.onClick.AddListener(() => {
            _args.restartAct?.Invoke();
            Close();
        });
        _toMenuBtn.onClick.AddListener(() =>
        {
            _args.backToMenuAct?.Invoke();
            Close();
        });
        if (_args.distance > 0)
        {
            _distanceValueTxt.text = _args.distance.ToString("0.0", new CultureInfo("en-US"));
            
        }
        _distanceMsgObj.SetActive(_args.distance > 0);
        gameObject.SetActive(true);
    }

}

public class GameOverWinArgs : BaseModalWinArgs
{
    public Action restartAct;
    public Action backToMenuAct;
    public float distance;
    // other args: message txt, bact to menu action, etc.
}
