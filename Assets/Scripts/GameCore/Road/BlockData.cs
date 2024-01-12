using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockData
{
   public int length;
   public List<BonusGateArgs> gateArgs;
   public int mandatoryPlanksNumber;
   public int nextAbyssLength;
}
