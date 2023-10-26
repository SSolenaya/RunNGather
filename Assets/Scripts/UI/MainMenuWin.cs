using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenuWin : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _gameModeOptionsDropdown;
    [SerializeField] private UnityEngine.UI.Button _startBtn;
    [SerializeField] private UnityEngine.UI.Toggle _soundToggle;
    private MainLogic _mainLogic;

    public void Setup(MainLogic mainLogic)
    {
        _mainLogic = mainLogic;
        List<string> options = new List<string>();
        options = Enum.GetNames(typeof(GameMode)).ToList();
        _gameModeOptionsDropdown.ClearOptions();
        _gameModeOptionsDropdown.AddOptions(options);
        _gameModeOptionsDropdown.value = 0;
        _gameModeOptionsDropdown.onValueChanged.RemoveAllListeners();
        _gameModeOptionsDropdown.onValueChanged.AddListener((_) => {
            _mainLogic.SetGameMode(_);
        });

        _startBtn.onClick.RemoveAllListeners();
        _startBtn.onClick.AddListener(() => {
            _mainLogic.Restart();
            _mainLogic.StartRunning();
            gameObject.SetActive(false);
        });

        _soundToggle.OnValueChangedAsObservable().Subscribe(_ => Debug.Log("Sound new state: " + _.ToString())); //  TODO: audioController
    }
}
