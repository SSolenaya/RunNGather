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
        _bonusText.gameObject.SetActive(true);
    }

    public void ShowFinishText()
    {
        _bonusText.color = Color.white;
        _bonusText.text = "FINISH";
        _bonusText.gameObject.SetActive(true);
    }
}
