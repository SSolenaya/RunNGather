using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPlayerEntityState : BasePlayerEntityState
{
    private MovingEntity _movingEntity;
    private bool _isRayCastAllowed;

    public RunPlayerEntityState(PlayerEntity playerEntity, MovingEntity movingEntity) : base(playerEntity)
    {
        _movingEntity = movingEntity;
    }

    public override void OnEnterState()
    {
        _characterAnimator.ChangeAnimationState(AnimationState.Run);
        _playerEntity.StartCoroutine(SendingRayDelay());
    }

    public override void OnUpdateState()
    {
        SendRay();
        _movingEntity.Move();
        _movingEntity.SetNewLocalPos();
    }

    private IEnumerator SendingRayDelay()
    {
        _isRayCastAllowed = false;
        yield return null;
        _isRayCastAllowed = true;
    }

    private void SendRay()
    {
        if ( !_isRayCastAllowed ) return;
        Ray ray = new Ray(_playerEntity.transform.position + Vector3.up * 1f, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 15, Color.green);
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
