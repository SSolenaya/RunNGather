using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private Transform _textPosition;
    private Action _onSuccessfulFinish;
    private BonusText _finishText;


    public void SubscribeForSuccessfulFinish(Action act)
    {
        _onSuccessfulFinish += act;
    }

    public void Release()
    {
        _onSuccessfulFinish = null;
        Destroy(_finishText.gameObject);
    }

    public void SetPlayerFinished() 
    {
        _onSuccessfulFinish?.Invoke();
    }

    public void SetFinishText(PrefabHolder prefabHolder, GameFieldHelper gameFieldHelper)
    {
        _finishText = Instantiate(prefabHolder.bonusTextPrefab, gameFieldHelper.worldCanvas);
        _finishText.transform.position = _textPosition.position;
        _finishText.ShowFinishText();
    }
}
