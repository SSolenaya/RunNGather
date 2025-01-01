using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StartingMenuWin : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _gameModeOptionsDropdown;     //  for changing the game mode between a simple running and a math mode
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _catcherSkinBtn;            //  temp bear
    [SerializeField] private Button _nosedmanSkinBtn;            //  temp bear
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private MathSettingsManager _mathSettingsManager;
    private MainLogic _mainLogic;

    public void Setup(MainLogic mainLogic, AudioController audioController)
    {
        _mainLogic = mainLogic;
        _mathSettingsManager.gameObject.SetActive(_mainLogic.GameMode == GameMode.mathMode);
        //SetupGameModeOptions();   // bear -> only math mode now
        SetupStartingButton();
        SetupSoundOptions(audioController);
        SetupSkinChoise();
    }

    private void SetupStartingButton()
    {
        _startBtn.onClick.RemoveAllListeners();
        _startBtn.onClick.AddListener(() => {
            if (_mainLogic.GameMode == GameMode.mathMode)
            {
                _mainLogic.SetMathSettings(_mathSettingsManager.RulesSettingsData);
                _mathSettingsManager.CheckSettings(out bool isAbleToStart);
                if (!isAbleToStart) return;
            }
            _mainLogic.Restart();
            _mainLogic.StartRunning();
            SetVisibility(false);
        });
    }

    private void SetupSkinChoise()
    {
        _catcherSkinBtn.onClick.AddListener(() => {
            _mainLogic.SetCharacterOption(CharacterType.catcher);
        });
        _nosedmanSkinBtn.onClick.AddListener(() => {
            _mainLogic.SetCharacterOption(CharacterType.nosedman);
        });
    }

    private void SetupSoundOptions(AudioController audioController)
    {
        _soundToggle.onValueChanged.AddListener((_) =>
        {
            audioController.SwitchSound(_);
        });
    }

    public void SetVisibility (bool isVisible)
    {
        _canvasGroup.alpha = isVisible? 1:0;
    }

    private void SetupGameModeOptions()
    {
        List<string> options = new List<string>();
        options = Enum.GetNames(typeof(GameMode)).ToList();
        _gameModeOptionsDropdown.ClearOptions();
        _gameModeOptionsDropdown.AddOptions(options);
        _gameModeOptionsDropdown.value = 0;
        _gameModeOptionsDropdown.onValueChanged.AddListener((_) => {
            _mainLogic.SetGameMode(_);
        });
    }

    private void RemovingListeners()
    {
        _catcherSkinBtn.onClick.RemoveAllListeners();
        _nosedmanSkinBtn.onClick.RemoveAllListeners();
        _soundToggle.onValueChanged.RemoveAllListeners();
        _gameModeOptionsDropdown.onValueChanged.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        RemovingListeners();
    }
}
