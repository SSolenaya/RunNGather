using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuildPlayerEntityState : BasePlayerEntityState
{
    private PlanksCounter _planksCounter;
    private Vector3 _currentPlankPlace = Vector3.zero;
    private PlanksManager _planksManager;
    private AudioController _audioController;
    private Settings _settings;
    private List<Plank> _planksList = new List<Plank>();

    public BuildPlayerEntityState(
        PlayerEntity playerEntity, 
        PlanksCounter planksCounter, 
        PlanksManager planksManager, 
        AudioController aC, 
        Settings settings) : base(playerEntity)
    {
        _planksCounter = planksCounter;
        _planksManager = planksManager;
        _audioController = aC;
        _settings = settings;
    }

    public override void OnEnterState()
    {
        _characterAnimator.ChangeAnimationState(AnimationState.Run);
        Building();
    }

    public void Building()
    {
        RoadBlock currentBlock = _playerEntity.CurrentRoadBlock;
        if (_planksCounter.PlankNumber > 0)
        {
            if (currentBlock != null)
            {
                _currentPlankPlace = new Vector3(currentBlock.GetBlockEndXPos() - 0.5f, 0.95f, _playerEntity.transform.position.z);
            }
            BuildPlank(_currentPlankPlace);
            _currentPlankPlace += Vector3.left * 0.46f;             //  bear magic number
            _playerEntity.CurrentRoadBlock = null;
            _playerEntity.SetPlayerState<RunPlayerEntityState>();
        }
        else
        {
            _playerEntity.SetPlayerState<FallPlayerEntityState>();
        }
    }

    public void BuildPlank(Vector3 plankPos)
    {
        Plank plank = _planksManager.GetPlank();
        plank.transform.position = plankPos;
        plank.gameObject.SetActive(true);
        plank.SetDistanceToPlayer(_settings.roadStartDistance);
        _audioController.PlayBuildingSound();
        plank.gameObject.name = "Plank_" + _planksList.Count;
        var plankSubscription = _playerEntity.xLocalPos.Subscribe(plank.CheckForHiding);
        plank.SetSubscription(plankSubscription);
        _planksList.Add(plank);
        _planksCounter.PlankNumber--;
    }


}
