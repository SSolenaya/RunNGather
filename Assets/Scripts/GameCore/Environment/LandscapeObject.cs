using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeObject : MonoBehaviour, IPoolItem
{
    public bool IsInPool { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Release()
    {
        
    }
}
