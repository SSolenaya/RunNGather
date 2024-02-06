using System;
using UnityEngine;

public abstract class BaseModalWindow : MonoBehaviour
{

    public abstract void Show(BaseModalWinArgs args);
    public abstract void Close();

}

public abstract class BaseModalWinArgs{}




