using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverModalWin : BaseModalWindow
{
    private GameOverWinArgs _args;
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _toMenuBtn;

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
        gameObject.SetActive(true);
    }

    //[SerializeField] private TMP_Text messageTxt;
}

public class GameOverWinArgs : BaseModalWinArgs
{
    public Action restartAct;
    public Action backToMenuAct;
    // other args: message txt, bact to menu action, etc.
}
