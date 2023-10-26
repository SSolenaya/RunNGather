using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolItem
{
    GameObject GetGameObject();
    bool IsInPool { get; set; }
    void Release();
}
