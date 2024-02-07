using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainLogic
{
    public GameMode GameMode { get; private set; }
    private int _levelNember = 0;//  only for Templated Levels Mode
    public int LevelNumber 
    {
        get { return _levelNember; }
        private set
            {
            if (value >= _settings.levelTemplatesList.Count)
            {
                _levelNember = 0;
            }
            else
            {
                _levelNember = value;
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
    private GameState _gameState;


    public void Restart()
    {
        SetGameState(GameState.wait);
        _playerController.Restart();
        _roadController.Restart(); 
        _environmentObjectsController.Restart();
        _playerController.SubscribeForPlayerPosition(_rootCanvas.gameUIController.ChangeDistanceText); 

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
            case GameState.wait:
                break;
            case GameState.win:
                _audioController.PlayWinningSound();
                Debug.Log("Level finished !!!");
                LevelNumber++;
                _playerController.PlayerWins();
                FinishLevelWinArgs finLvlArgs = new FinishLevelWinArgs();
                finLvlArgs.backToMenuAct += _mainMenuController.ShowMainMenu;
                finLvlArgs.nextLvlAct += Restart;
                finLvlArgs.nextLvlAct += () => _playerController.MakePlayerRun();
                _modalWindowsController.ShowModalWin<FinishLevelModalWin>(finLvlArgs);
                break;
            case GameState.gameOver:
                _audioController.PlayFallingSound();
                Debug.Log("Game over !!!");
                GameOverWinArgs goArgs = new GameOverWinArgs();
                goArgs.restartAct += Restart;
                goArgs.restartAct += () => _playerController.MakePlayerRun();
                goArgs.backToMenuAct += _mainMenuController.ShowMainMenu;
                if (GameMode == GameMode.eternalRunning)
                {
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

    public void SetGameMode(GameMode newMode)
    {
        if (GameMode == newMode)
        {
            return;
        }
        GameMode = newMode;
    }

    public void SetGameMode(int optionNumber)
    {
        GameMode newGM = (GameMode)optionNumber;
        SetGameMode(newGM);
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    public void StartRunning()
    {
        _playerController.MakePlayerRun();
    }

    public void SubscribeForSoundMute(bool newSoundState)
    {
        _audioController.SwitchSound(newSoundState);
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
    templatedLevels
    //randomLevels
}
