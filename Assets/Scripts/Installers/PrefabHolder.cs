using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabHolder", menuName = "ScriptableObjects/PrefabHolder", order = 1)]
public class PrefabHolder : ScriptableObject
{
    public RoadBlock roadBlockPrefab;
    public PlayerEntity playerPrefab;
    public Plank plankPrefab;
    public BonusGate bonusGatePrefab;
    public BonusText bonusTextPrefab;
    public FinishLine finishLinePrefab;
    public GameFieldHelper gameFieldHelperPrefab;
    public UICanvasRoot uiCanvasRootPrefab;
    public List<BaseModalWindow> modalWindowList;
    public MainMenuWin mainMenuWinPrefab;
    public List <CharacterViewData> modelsList;

    public CharacterAnimator GetModelPrefabByType(CharacterType type)
    {
        foreach (var item in modelsList)
        {
            if(item.iD == type)
            {
                return item.model;
            }
        }
        Debug.LogError("PrefabHolder does not have model of this type: " + type.ToString());
        return null;
    }
}
