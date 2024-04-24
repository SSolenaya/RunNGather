using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BonusGate : AbstractGate
{
    private BonusGateArgs _currentGatesSettings;

    public override void SetGateSettings(AbstractGateArgs args)
    {
        _currentGatesSettings = (BonusGateArgs)args;
    }


    protected override void SetupLeftSemiGate()
    {
         GateOperatorArgs args = _currentGatesSettings.leftGateArgs;
        _leftBonus = SetOperationToGate(_leftGate, args.opType);
        _leftBonus.Setup(args.modifier, this);
        _leftBonusText = Instantiate(_prefabHolder.bonusTextPrefab, _gameFieldHelper.worldCanvas);
        _leftBonusText.transform.position = _leftTextPos.position;
        _leftBonusText.gameObject.name = "Left_text_"+ _parentBlockName;
        _leftBonusText.Setup(_leftBonus);
    }

    protected override void SetupRightSemiGate()
    {
         GateOperatorArgs args = _currentGatesSettings.rightGateArgs;
        _rightBonus = SetOperationToGate(_rightGate, args.opType);
        _rightBonus.Setup(args.modifier, this);
        _rightBonusText = Instantiate(_prefabHolder.bonusTextPrefab, _gameFieldHelper.worldCanvas);
        _rightBonusText.transform.position = _rightTextPos.position;
        _rightBonusText.gameObject.name = "Right_text_" + _parentBlockName;
        _rightBonusText.Setup(_rightBonus);
    }

    

    //public GateOperator SetRandomGate(GameObject go)
    //{
    //    var random = new System.Random();
    //    var randomOpType = random.RandomEnum<BonusOperationTypes>();
    //    return SetOperationToGate(go, randomOpType);
    //}

    

    
}


