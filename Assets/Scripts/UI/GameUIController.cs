using System.Globalization;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject _distanceObj; //TODOSALT именовение полей
    [SerializeField] private GameObject _taskObj;
    [SerializeField] private TMP_Text _taskText;
    [SerializeField] private TMP_Text playersOvercomeDistanceTxt;

    public void Reset(GameMode gameMode)
    {
        GameObjectsSetup(gameMode);
        playersOvercomeDistanceTxt.text = "0";
        _taskText.text = "";
    }

    private void GameObjectsSetup (GameMode gameMode)
    {
        _distanceObj.SetActive(gameMode == GameMode.eternalRunning);
        _taskObj.SetActive(gameMode == GameMode.mathMode);
    }

    public void ChangeDistanceText(float newPlayerPosX)
    {
        playersOvercomeDistanceTxt.text = (-1 * newPlayerPosX).ToString("0.0", new CultureInfo("en-US"));
    }

    public void ShowCurrentMathTask(string currentTask)
    {
        _taskText.text = currentTask;
    }

    public void OnTaskChanging(MathGate mathGate)
    {
       ShowCurrentMathTask(mathGate.GetCurrentTask());
    }
}
