using DG.Tweening;
using MathRoom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainLogic
{
    
    
    [Inject] private Settings _settings;
    [Inject] private RoadController _roadController;
    [Inject] private EnvironmentObjectsController _environmentObjectsController;
    [Inject] private PlayerController _playerController;
    [Inject] private ModalWindowsController _modalWindowsController;
    [Inject] private MenuController _mainMenuController;
    [Inject] private AudioController _audioController;
    [Inject] private UICanvasRoot _rootCanvas;
    [Inject] private PlanksManager _planksManager;
    [Inject] private GatesController _gatesController;
    public GameMode GameMode
    {
        get
        {
            return _settings.gameMode;                         //  bear -> only math mode now
        }
        private set { }
    }

    private GameStateProvider _gameStateProvider;
    public GameState GameState
    {
        get
        {
            return _gameStateProvider.GetGameState();
        }
        set
        {
            _gameStateProvider.SetGameState(value);
        }
    }
    private RulesSettingsData _rulesSettingsData;
    public RulesSettingsData RulesSettingsData => _rulesSettingsData;
    private LevelTemplateNumberProvider _levelTemplateNumberProvider;
    public int LevelTemplateNumber
    {
        get
        {
            return _levelTemplateNumberProvider.LevelNumber;
        }
        set
        {
            _levelTemplateNumberProvider.LevelNumber = value;
        }
    }

    public void Init()
    {
        _gameStateProvider = new GameStateProvider(this, _audioController, _playerController, _mainMenuController, _modalWindowsController);
        _levelTemplateNumberProvider = new LevelTemplateNumberProvider(_settings);
    }

    public void Restart()
    {
        _gameStateProvider.SetGameState(GameState.wait);
        _rootCanvas.gameUIController.Reset(_settings.gameMode);
        _gatesController.Restart();
        _planksManager.Restart();
        _playerController.Restart();
        _roadController.Restart();         
        _environmentObjectsController.Restart();
        _playerController.SubscribeForPlayerPosition(_rootCanvas.gameUIController.ChangeDistanceText);
        _gatesController.SetNextUnsolvedGate();
    }

    public void SetMathSettings(RulesSettingsData rulesSettingsData)
    {
        _rulesSettingsData = rulesSettingsData;
    }

    private void SetGameMode(GameMode newMode)
    {
        if (GameMode == newMode)
        {
            return;
        }

        GameMode = newMode;
        _settings.gameMode = newMode;
    }

    public void SetGameMode(int optionNumber)
    {
        GameMode newGameMode = (GameMode)optionNumber;
        SetGameMode(newGameMode);
    }

    public void StartRunning()
    {
        _playerController.MakePlayerRun();
    }
    
    public void SetCharacterOption(CharacterType newCharacterType)
    {
        _settings.currentCharType = newCharacterType;
    }

}


public enum GameState
{
    wait,
    win,
    gameOver,
    none
}

public enum GameMode
{
    eternalRunning,
    templatedLevels,
    mathMode
    //randomLevels
}
