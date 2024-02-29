using System;
using UnityEngine;
using Zenject;

public class Plank : MonoBehaviour, IPoolItem, IPlankNumberChanger
{
    public bool IsInPool { get; set; }
    private PlanksManager _builder;
    private float _hidingDistance; 
    private IDisposable _sub;

    public void SetBuilder(PlanksManager builder)
    {
        _builder = builder;
    }

    public void SetDistanceToPlayer(float hidingDistance)
    {
        _hidingDistance = hidingDistance;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void CheckForHiding(float currentPlayerPosX)
    {
        if ((transform.position.x - currentPlayerPosX) > _hidingDistance)
        {
            HidePlank();
        }
    }

    public void Release()
    {
        
    }

    public void SetSubscription(IDisposable sub)
    {
        _sub = sub;
    }

    public void ChangePlankNumber(PlanksCounter planksCounter)
    {
        planksCounter.IncreasePlanksNumber();
        HidePlank();
    }

    public void HidePlank()
    {
        _sub?.Dispose();
        _builder?.HidePlank(this);
    }

}
