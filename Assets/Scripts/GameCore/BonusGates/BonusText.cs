using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusText : MonoBehaviour
{
    [SerializeField] private TMP_Text _bonusText;

    public void Setup(GateOperator gateOperator)
    {
        _bonusText.text = gateOperator.GetActionSymbol() + gateOperator.GetModifier();
    }
}
