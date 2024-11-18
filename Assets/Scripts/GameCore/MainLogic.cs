using DG.Tweening;
using MathRoom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainLogic
{
    public GameMode GameMode { 
        get {return GameMode.mathMode;}
        private set { value = GameMode.mathMode;} }
    private int _levelNumber = 0;//  only for Templated Levels Mode

    public int LevelNumber
    {
        get => _levelNumber;
        private set
        {
            if (value >= _settings.levelTemplatesList.Count)
            {
                _levelNumber = 0;
            }
            else
            {
                _levelNumber = value;
            }
        }
    }
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

    private GameState _gameState;
    private RulesSettingsData _rulesSettingsData;
    public RulesSettingsData RulesSettingsData => _rulesSettingsData;

    public void Setup()
    {
        SetGameMode(GameMode.mathMode);
    }

    public void SetMathSettings(RulesSettingsData rulesSettingsData)
    {
        _rulesSettingsData = rulesSettingsData;
    }

    //TODOSALT добавить setup и newgame
    public void Restart()
    {
        SetGameState(GameState.wait);
        _rootCanvas.gameUIController.Reset(_settings.gameMode);
        _gatesController.Restart();
        _planksManager.Restart();
        _playerController.Restart();
        _roadController.Restart();                  //  _gatesController.SetNextUnsolvedGate(); after the ending of building full road
        _environmentObjectsController.Restart();
        _playerController.SubscribeForPlayerPosition(_rootCanvas.gameUIController.ChangeDistanceText);
        _gatesController.SetNextUnsolvedGate();
    }

    public void SetGameState(GameState newState)
    {
        if (_gameState == newState)
        {
            return;
        }
        _gameState = newState;
        switch (newState)
        {
            //TODOSALT чем отличаетс€ от none?
            case GameState.wait:
                break;
            case GameState.win:
                _audioController.PlayWinningSound();
                Debug.Log("Level finished !!!");
                LevelNumber++;
                _playerController.PlayerWins();

                //TODOSALT не пон€тно что происходит. јргументы не нужны так как вс€ эта логика может быть в FinishLevelModalWin. FinishLevelModalWin не вызываетс€ с другими аргументымм
                FinishLevelWinArgs finLvlArgs = new FinishLevelWinArgs();
                finLvlArgs.backToMenuAct += _mainMenuController.ShowMainMenu;
                finLvlArgs.nextLvlAct += Restart;
                finLvlArgs.nextLvlAct += () => _playerController.MakePlayerRun();
                _modalWindowsController.ShowModalWin<FinishLevelModalWin>(finLvlArgs);
                break;
            case GameState.gameOver:
                _audioController.PlayFallingSound();
                Debug.Log("Game over !!!");
                //TODOSALT јргументы не нужны так как вс€ эта логика может быть в GameOverModalWin. GameOverModalWin не вызываетс€ с другими аргументымм
                GameOverWinArgs goArgs = new GameOverWinArgs();
                goArgs.restartAct += Restart;
                goArgs.restartAct += () => _playerController.MakePlayerRun();
                goArgs.backToMenuAct += _mainMenuController.ShowMainMenu;
                if (GameMode == GameMode.eternalRunning)
                { //TODOSALT вот тут норм. и то окно может вз€ть на пр€мую из _playerController
                    goArgs.distance = _playerController.GetPlayerDistance();
                }
                _modalWindowsController.ShowModalWin<GameOverModalWin>(goArgs);
                break;
            case GameState.none:
            default:
                Debug.LogError("Game state isn't defined");
                break;
        }
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
        //TODOSALT не сокращать переменные
        GameMode newGameMode = (GameMode)optionNumber;
        SetGameMode(newGameMode);
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    public void StartRunning()
    {
        _playerController.MakePlayerRun();
    }
    //TODOSALT в контреллер звуков
    public void SubscribeForSoundMute(bool newSoundState)
    {
        _audioController.SwitchSound(newSoundState);
    }

    public void SetCharacterOption(CharacterType newCharacterType)
    {
        _settings.currentCharType = newCharacterType;
    }

}


//TODOSALT а где гейм стейт игра?
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
