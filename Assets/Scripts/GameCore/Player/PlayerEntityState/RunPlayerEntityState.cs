using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPlayerEntityState : BasePlayerEntityState
{
    private MovingEntity _movingEntity;

    public RunPlayerEntityState(PlayerEntity playerEntity, MovingEntity movingEntity) : base(playerEntity)
    {
        _movingEntity = movingEntity;
    }

    public override void OnEnterState()
    {
        _characterAnimator.ChangeAnimationState(AnimationState.Run);
    }

    public override void OnUpdateState()
    {
        SendRay();
        _movingEntity.Move();
        _movingEntity.SetNewLocalPos();
    }

    private void SendRay()
    {
        Ray ray = new Ray(_playerEntity.transform.position + Vector3.up * 1f, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 5, Color.green);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 20, _playerEntity.layerMask))
        {
            _playerEntity.SetPlayerState<BuildPlayerEntityState>();
            

        }
        else
        {
            if (_playerEntity.CurrentRoadBlock != null)
            {
                return;
            }
            RoadBlock hitBlock = hit.collider.gameObject.GetComponentInParent<RoadBlock>();
            if (hitBlock != null)
            {
                _playerEntity.CurrentRoadBlock = hitBlock;
                
            }

        }
    }
}
