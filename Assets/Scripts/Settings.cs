using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 0)]
public class Settings : ScriptableObject
{
    public int roadStartDistance;                       //             player's distance from current block for doing check of hiding 
    public float playerSpeed;
    public int minBlockLenght;
    public int maxBlockLenght;                                    
    public int roadEndDistance;                       //             player's distance from current block for building new blocks                                
    public int startingBlockNumber;                   //             block's amount that is built on start
    public int planksPoints;                          //             test temp bear - > remove or change planks to planks packs 
    public int minDistanceBetweenGates;

    [Range(1, 100)]
    public int plankChance;

    
    public List<BonusGateArgs> bonusGatesList = new();         
    public List<LevelData> levelTemplatesList = new();

    public CharacterType currentCharType;                           //  test bear temp

}


