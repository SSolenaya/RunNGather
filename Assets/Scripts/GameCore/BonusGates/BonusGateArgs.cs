using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BonusGateArgs : AbstractGateArgs
{
    public GateOperatorArgs leftGateArgs;
    public GateOperatorArgs rightGateArgs;
}
