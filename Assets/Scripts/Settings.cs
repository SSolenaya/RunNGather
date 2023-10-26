using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 0)]
public class Settings : ScriptableObject
{
    public int roadStartDistance = 8;                       //             player's distance from current block for doing check of hiding 
    public int playerSpeed = 5;
    public int minBlockLenght = 15;
    public int maxBlockLenght = 30;                                    
    public int roadEndDistance = 15;                       //             player's distance from current block for building new blocks                                
    public int startingBlockNumber = 10;                   //             block's amount that is built on start
    public int planksPoints = 1;                   //             test temp bear - > remove or change planks to planks packs 

    [Range(1, 100)]
    public int plankChance;

    
    public List<BonusGateArgs> bonusGatesList = new();          //  replace with levelsTemplatesList
    public List<LevelData> levelTemplatesList = new();

}


