using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text playersOvercomeDistanceTxt;

    public void ChangeDistanceText(float newPlayerPosX)
    {
        playersOvercomeDistanceTxt.text = (-1*newPlayerPosX).ToString("0.0", new CultureInfo("en-US"));
    }

}
