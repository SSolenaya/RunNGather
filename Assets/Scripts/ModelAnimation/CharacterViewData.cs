using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterViewData
{
    public CharacterType iD;
    public CharacterAnimator model;
}

public enum CharacterType
{
    cowboy,
    test        //  test bear
}
