using System;
using UnityEngine;
using UniRx;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private PrefabHolder _prefabHolder;
    [Inject] private Settings _settings;
    [Inject] private GameFieldHelper _gameFieldHelper;
    [Inject] private MainLogic _mainLogic;
    [Inject] private DiContainer _diContainer;
    private PoolManager _plankPoolManager;
    public PlayerEntity _playerEntity;
    private CompositeDisposable _disposables = new CompositeDisposable();

    public void Restart()
    {
        if (_plankPoolManager == null)
        {
            _plankPoolManager = new PoolManager(_prefabHolder.plankPrefab, _settings.maxBlockLenght * _settings.startingBlockNumber, _gameFieldHelper, _diContainer);
        }
        _disposables.Clear();
        PlayerInstantiation(_plankPoolManager);
    }

    private void PlayerInstantiation(PoolManager pM)
    {
        if (_playerEntity == null)
        {
            _playerEntity = _diContainer.InstantiatePrefab(_prefabHolder.playerPrefab).GetComponent<PlayerEntity>();
            _playerEntity.transform.SetParent(_gameFieldHelper.gameObjParent);
            _playerEntity.Setup(pM);
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
        _playerEntity.SetPlayerState(PlayerState.idle);
    }

    public void MakePlayerRun()
    {
        _playerEntity.SetPlayerState(PlayerState.run);
    }

    public float GetPlayerDistance()
    {
        return -1*_playerEntity.xLocalPos.Value;
    }
}
