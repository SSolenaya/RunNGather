using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishLevelModalWin : BaseModalWindow
{
    private FinishLevelWinArgs _args;
    [SerializeField] private Button _nextLevelBtn;
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
        _args = args as FinishLevelWinArgs;
        _nextLevelBtn.onClick.RemoveAllListeners();
        _nextLevelBtn.onClick.AddListener(() => 
        {
            _args.nextLvlAct?.Invoke();
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

public class FinishLevelWinArgs : BaseModalWinArgs
{
    public Action nextLvlAct;
    public Action backToMenuAct;
    // other args: message txt, bact to menu action, etc.
}
