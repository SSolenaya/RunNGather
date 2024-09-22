using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject distanceObj;
    [SerializeField] private GameObject _taskObj;
    [SerializeField] private TMP_Text _taskText;
    [SerializeField] private TMP_Text playersOvercomeDistanceTxt;

    public void Setup (GameMode gameMode)
    {
        distanceObj.SetActive(gameMode == GameMode.eternalRunning);
        _taskObj.SetActive(gameMode == GameMode.mathMode);
    }

    public void ChangeDistanceText(float newPlayerPosX)
    {
        playersOvercomeDistanceTxt.text = (-1*newPlayerPosX).ToString("0.0", new CultureInfo("en-US"));
    }

    public void ShowCurrentMathTask(string currentTask)
    {
        _taskText.text = currentTask;
    }

    public void OnTaskChanging(AbstractGate absGate)
    {
        MathGate mathGate = (MathGate)absGate;
        if (mathGate != null)
        {
            ShowCurrentMathTask(mathGate.GetCurrentTask());
        }
    }


    public void ReleaseUIElements(GameMode gameMode)
    {
        Setup(gameMode);
        playersOvercomeDistanceTxt.text = "0";
        _taskText.text = "";
    }
}
