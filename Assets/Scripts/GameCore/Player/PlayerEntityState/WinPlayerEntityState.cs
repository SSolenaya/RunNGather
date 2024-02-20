using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPlayerEntityState : BasePlayerEntityState
{
    public WinPlayerEntityState(PlayerEntity playerEntity) : base(playerEntity)
    {
    }

    public override void OnEnterState()
    {
        _characterAnimator.ChangeAnimationState(AnimationState.Win);
    }
}
