using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathGate : AbstractGate
{
    private BonusText _taskText;
    private MathGateArgs _currentGatesSettings;

    public override void SetGateSettings(AbstractGateArgs args)
    {
        _currentGatesSettings = (MathGateArgs)args;
    }

    public override void SetupGates()
    {
        SetupGateTask();
        base.SetupGates();
    }

    protected override void SetupLeftSemiGate()
    {
         MathGateOperatorArgs args = _currentGatesSettings.leftGateArgs;
        _leftBonus = SetOperationToGate(_leftGate, args.opType);
        _leftBonus.Setup(args.modifier, this);
        _leftBonusText = Instantiate(_prefabHolder.bonusTextPrefab, _gameFieldHelper.worldCanvas);
        _leftBonusText.transform.position = _leftTextPos.position;
        _leftBonusText.gameObject.name = "Left_text_" + gameObject.name;
        _leftBonusText.Setup(args.visibleValue.ToString());
    }

    protected override void SetupRightSemiGate()
    {
        MathGateOperatorArgs args = _currentGatesSettings.rightGateArgs;
        _rightBonus = SetOperationToGate(_rightGate, args.opType);
        _rightBonus.Setup(args.modifier, this);
        _rightBonusText = Instantiate(_prefabHolder.bonusTextPrefab, _gameFieldHelper.worldCanvas);
        _rightBonusText.transform.position = _rightTextPos.position;
        _rightBonusText.gameObject.name = "Right_text_" + gameObject.name;
        _rightBonusText.Setup(args.visibleValue.ToString());
    }

    private void SetupGateTask()
    {
        _taskText = Instantiate(_prefabHolder.bonusTextPrefab, _gameFieldHelper.worldCanvas);
        _taskText.transform.position = transform.position + 3*Vector3.up;
        _taskText.gameObject.name = "Task_text_" + gameObject.name;
        _taskText.Setup(_currentGatesSettings.gateTask);
    }

    public override void Release()
    {
        base.Release();
        if (_taskText != null)
        {
            Destroy(_taskText?.gameObject);
        }
    }


}
