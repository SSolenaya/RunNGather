using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateProvider
{
    private MainLogic _mainLogic;
    private MenuController _mainMenuController;
    private AudioController _audioController;
    private PlayerController _playerController;
    private ModalWindowsController _modalWindowsController;
    private GameState _gameState;

    public GameStateProvider(MainLogic mainLogic, AudioController audioController, PlayerController playerController, MenuController mainMenuController, ModalWindowsController modalWindowsController)
    {
        _mainLogic = mainLogic;
        _audioController = audioController;
        _playerController = playerController;
        _mainMenuController = mainMenuController;
        _modalWindowsController = modalWindowsController;
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
                _mainLogic.LevelTemplateNumber++;
                _playerController.PlayerWins();
                FinishLevelWinArgs finLvlArgs = new FinishLevelWinArgs();
                finLvlArgs.backToMenuAct += _mainMenuController.ShowMainMenu;
                finLvlArgs.nextLvlAct += _mainLogic.Restart;
                finLvlArgs.nextLvlAct += () => _playerController.MakePlayerRun();
                _modalWindowsController.ShowModalWin<FinishLevelModalWin>(finLvlArgs);
                break;
            case GameState.gameOver:
                _audioController.PlayFallingSound();
                Debug.Log("Game over !!!");
                GameOverWinArgs goArgs = new GameOverWinArgs();
                goArgs.restartAct += _mainLogic.Restart;
                goArgs.restartAct += () => _playerController.MakePlayerRun();
                goArgs.backToMenuAct += _mainMenuController.ShowMainMenu;
                if (_mainLogic.GameMode == GameMode.eternalRunning || _mainLogic.GameMode == GameMode.mathMode)
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

    public GameState GetGameState()
    {
        return _gameState;
    }
}
