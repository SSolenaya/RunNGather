using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockData
{
   [Range(6, 30)]
   public int length;
   public List<BonusGateArgs> gateArgs;
   public int mandatoryPlanksNumber;
   public int nextAbyssLength;
}
