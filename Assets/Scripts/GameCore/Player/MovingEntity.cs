using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MovingEntity
{
    private Transform _playerEntity;
    private Settings _settings;
    private Vector3 _currentDirectionV3;
    private float _speed;
    public ReactiveProperty<float> xLocalPos = new ReactiveProperty<float>();
    private Action _onFallingAct;

    private Tween fallingTween;

    public MovingEntity (Transform entity, Settings settings)
    {
        _playerEntity = entity;
        _settings = settings;
    }

    public void Restart()
    {
        fallingTween?.Kill();
        _playerEntity.localPosition = new Vector3(-0.25f, 1f, 0);
        xLocalPos.SetValueAndForceNotify(_playerEntity.localPosition.x);
        _currentDirectionV3 = Vector3.left;
        _speed = _settings.playerSpeed;
    }

    public void Move()
    {
        _playerEntity.Translate(_currentDirectionV3 * _speed * Time.deltaTime);

        if (_playerEntity.position.y < -15f)
        {
            _onFallingAct?.Invoke();
        }

    }

    public void ChangeDirectionOnFalling()
    {
        fallingTween = DOVirtual.Float(0, _speed * 5, 1f, var => { _currentDirectionV3 = Vector3.down * var; }).SetEase(Ease.OutQuad);
    }

    public void SubscribeForFalling(Action act)
    {
        _onFallingAct += act;

    }

    public void SetNewLocalPos()
    {
        if (_playerEntity.localPosition.x == xLocalPos.Value) return;
        xLocalPos.SetValueAndForceNotify(_playerEntity.localPosition.x);
    }
}
