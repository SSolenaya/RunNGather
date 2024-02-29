using System;
using UnityEngine;
using UniRx;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private MainLogic _mainLogic;
    [Inject] private DiContainer _diContainer;
    public PlayerEntity _playerEntity;
    private CompositeDisposable _disposables = new CompositeDisposable();

    public void Restart()
    {
        _disposables.Clear();
        PlayerInstantiation();
    }

    private void PlayerInstantiation()
    {
        if (_playerEntity == null)
        {
            _playerEntity = _diContainer.InstantiatePrefab(_prefabHolder.playerPrefab).GetComponent<PlayerEntity>();
            _playerEntity.transform.SetParent(_gameFieldHelper.gameObjParent);
            _playerEntity.Setup();
            _playerEntity.SubscribeForFalling(() => _mainLogic.SetGameState(GameState.gameOver));
        }
        _playerEntity.Restart();
    }

    public void SubscribeForPlayerPosition(Action<float> act)
    {
        _playerEntity.xLocalPos.Subscribe<float>(x => act.Invoke(x)).AddTo(_disposables);
    }

    public void SetPlayerIdle()
    {
        _playerEntity.SetPlayerState<IdlePlayerEntityState>();
    }

    public void MakePlayerRun()
    {
        _playerEntity.SetPlayerState<RunPlayerEntityState>();
    }

    public void PlayerWins()
    {
        _playerEntity.SetPlayerState<WinPlayerEntityState>();
    }

    public float GetPlayerDistance()
    {
        return -1*_playerEntity.xLocalPos.Value;
    }
}
