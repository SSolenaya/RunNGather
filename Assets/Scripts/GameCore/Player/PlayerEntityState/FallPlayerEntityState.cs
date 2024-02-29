using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlayerEntityState : BasePlayerEntityState
{
    private MovingEntity _movingEntity;

    public FallPlayerEntityState(PlayerEntity playerEntity, MovingEntity movingEntity) : base(playerEntity)
    {
        _movingEntity = movingEntity;
    }

    public override void OnEnterState()
    {
        _movingEntity.ChangeDirectionOnFalling();
        _characterAnimator.ChangeAnimationState(AnimationState.Fall);
    }

    public override void OnUpdateState()
    {
        _movingEntity.Move();
    }
}
