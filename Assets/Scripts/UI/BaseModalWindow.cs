using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public abstract class BaseModalWindow : MonoBehaviour
{

    public abstract void Show(BaseModalWinArgs args);
    public abstract void Close();

}

public abstract class BaseModalWinArgs{}




