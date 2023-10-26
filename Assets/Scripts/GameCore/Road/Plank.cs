using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using Zenject;

public class Plank : MonoBehaviour, IPoolItem, IPlankNumberChanger
{
    public bool IsInPool { get; set; }
    [Inject] private Settings settings;
    private PoolManager _poolManager;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void CheckForHiding(float currentPlayerPosX, float delta, Action act)
    {
        if ((transform.position.x - currentPlayerPosX) > delta)
        {
            SendItemToPool();
            act?.Invoke();
        }
    }

    public void SendItemToPool()
    {
        _poolManager.ReleaseItem(this);
    }

    public void Release()
    {
        
    }

    public void ChangePlankNumber(PlankChangerActor plankChangerActor)
    {
        plankChangerActor.PlankNumber += settings.planksPoints;
        _poolManager.ReleaseItem(this);
    }

    public void SetupPoolManager(PoolManager pm)
    {
        _poolManager = pm;
    }
}
